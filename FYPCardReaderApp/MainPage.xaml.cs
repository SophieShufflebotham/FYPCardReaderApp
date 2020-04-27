using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FYPCardReaderApp.Interfaces;
using FYPCardReaderApp.Models;
using FYPCardReaderApp.Views;

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

        private async void DisplayMissingPersons(object sender, EventArgs e)
        {
            Console.WriteLine("dun eet?");
            MissingUsersView userListPage = new MissingUsersView();
            NavigationPage navPage = new NavigationPage(userListPage);

            await Application.Current.MainPage.Navigation.PushModalAsync(navPage);
        }

        private void ResetEmergencyStatus(object sender, EventArgs e)
        {
            RestService service = new RestService();
            service.GetResetStatus();
        }
    }
}
