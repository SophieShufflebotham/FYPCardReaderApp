using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FYPCardReaderApp.Interfaces;

namespace FYPCardReaderApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            ICardReaderService service = DependencyService.Get<ICardReaderService>();
            service.StartListening();
            await Application.Current.MainPage.DisplayAlert("NFC", "NFC Started", "OK");
        }
    }
}
