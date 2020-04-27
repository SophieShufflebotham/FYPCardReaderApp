using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYPCardReaderApp
{
    public partial class App : Application
    {
        public static int LOCATION_ID = 1;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
