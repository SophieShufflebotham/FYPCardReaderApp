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

            if(App.LOCATION_ID == 0)
            {
                await DisplayAlert("No location set", "A location must first be configured", "OK");
                ChangeLocation(null, null);
            }

            LocationNameLabel.Text = $"This location is: {App.LOCATION_NAME}";
        }

        private async void DisplayMissingPersons(object sender, EventArgs e)
        {
            MissingUsersView userListPage = new MissingUsersView();
            NavigationPage navPage = new NavigationPage(userListPage);

            await Application.Current.MainPage.Navigation.PushModalAsync(navPage);
        }

        private void ResetEmergencyStatus(object sender, EventArgs e)
        {
            RestService service = new RestService();
            service.GetResetStatus();
        }

        private async void ChangeLocation(object sender, EventArgs e)
        {
            ChangeLocationView changeLocationPage = new ChangeLocationView();
            NavigationPage navPage = new NavigationPage(changeLocationPage);

            await Application.Current.MainPage.Navigation.PushModalAsync(navPage);
        }
    }
}
