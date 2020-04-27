using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FYPCardReaderApp.Interfaces;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using Android.Nfc;
using Android.Nfc.Tech;
using FYPCardReaderApp.Models;
using FYPCardReaderApp.Responses;

[assembly: Dependency(typeof(FYPCardReaderApp.Droid.Services.CardReaderService))]
namespace FYPCardReaderApp.Droid.Services
{
    class CardReaderService : Java.Lang.Object, NfcAdapter.IReaderCallback, ICardReaderService
    {
        private static readonly string TAG = "AccessCardReader";

        // AID for our loyalty card service.
        private static readonly string ACCESS_CARD_AID = "FF69696969";
        // ISO-DEP command HEADER for selecting an AID.
        // Format: [Class | Instruction | Parameter 1 | Parameter 2]
        private static readonly string SELECT_APDU_HEADER = "00A40400";
        // "OK" status word sent in response to SELECT AID command (0x9000)
        private static readonly byte[] SELECT_OK_SW = { (byte)0x90, (byte)0x00 };

        public NfcReaderFlags READER_FLAGS = NfcReaderFlags.NfcA | NfcReaderFlags.SkipNdefCheck;
        public void OnTagDiscovered(Tag tag)
        {
            IsoDep isoDep = IsoDep.Get(tag);

            if (isoDep == null)
            {
                Console.WriteLine("[CODE] Unsupported Tag");
                //navigator.failureNavigation("Unsupported Tag");
            }
            else
            {
                Console.WriteLine("[CODE] IsoDep Found");
                isoDep.Connect();
                byte[] command = BuildSelectApdu(ACCESS_CARD_AID);
                try
                {
                    byte[] result = isoDep.Transceive(command);

                    // If AID is successfully selected, 0x9000 is returned as the status word (last 2
                    // bytes of the result) by convention. Everything before the status word is
                    // optional payload, which is used here to hold the account number.
                    int resultLength = result.Length;
                    byte[] statusWord = { result[resultLength - 2], result[resultLength - 1] };
                    byte[] payload = new byte[resultLength - 2];
                    Array.Copy(result, payload, resultLength - 2);
                    bool arrayEquals = SELECT_OK_SW.Length == statusWord.Length;

                    for (int i = 0; i < SELECT_OK_SW.Length && i < statusWord.Length && arrayEquals; i++)
                    {
                        arrayEquals = (SELECT_OK_SW[i] == statusWord[i]);
                        if (!arrayEquals)
                            break;
                    }
                    if (arrayEquals)
                    {
                        // The remote NFC device will immediately respond with its stored account number
                        string payloadString = System.Text.Encoding.UTF8.GetString(payload);
                        Console.WriteLine($"Recieved Payload: {payloadString}");
                        OnPayloadReceived(payloadString);
                        Device.BeginInvokeOnMainThread(async () => {
                            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Success", $"Recieved Payload {payloadString}", "OK");
                        });
                    }
                }
                catch (TagLostException e)
                {
                    isoDep.Close();
                    Console.WriteLine("[CODE] Caught tag loss error");
                    Device.BeginInvokeOnMainThread(async () => {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", $"Tag Lost Error", "OK");
                    });
                }

            }

        }

        public void StartListening()
        {
            Activity activity = CrossCurrentActivity.Current.Activity;

            NfcAdapter nfc = NfcAdapter.GetDefaultAdapter(activity);
            CardReaderService callback = new CardReaderService();
            if (nfc != null)
            {
                Console.WriteLine("[CODE] Reader Started");
                nfc.EnableReaderMode(activity, callback, READER_FLAGS, null);
            }
        }

        public void StopListening()
        {

        }

        public static byte[] BuildSelectApdu(string aid)
        {
            // Format: [CLASS | INSTRUCTION | PARAMETER 1 | PARAMETER 2 | LENGTH | DATA]
            return HexStringToByteArray(SELECT_APDU_HEADER + (aid.Length / 2).ToString("X2") + aid);
        }

        private static byte[] HexStringToByteArray(string s)
        {
            int len = s.Length;
            if (len % 2 == 1)
            {
                throw new ArgumentException("Hex string must have even number of characters");
            }
            byte[] data = new byte[len / 2]; //Allocate 1 byte per 2 hex characters
            for (int i = 0; i < len; i += 2)
            {
                ushort val, val2;
                // Convert each chatacter into an unsigned integer (base-16)
                try
                {
                    val = (ushort)Convert.ToInt32(s[i].ToString() + "0", 16);
                    val2 = (ushort)Convert.ToInt32("0" + s[i + 1].ToString(), 16);
                }
                catch (Exception)
                {
                    continue;
                }

                data[i / 2] = (byte)(val + val2);
            }
            return data;
        }

        private async void OnPayloadReceived(string payload)
        {
            if(payload != "0" && payload != "")
            {
                SetAccessTimeResponse rep = new SetAccessTimeResponse();
                rep.userId = payload;
                rep.locationId = App.LOCATION_ID;
                RestService service = new RestService();
                service.PostTimeRequest<SetAccessTimeResponse>(rep);
            }
        }
    }
}