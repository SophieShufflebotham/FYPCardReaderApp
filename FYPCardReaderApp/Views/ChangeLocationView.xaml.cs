using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FYPCardReaderApp.Models;
using System.Collections.ObjectModel;

namespace FYPCardReaderApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeLocationView : ContentPage
    {
        public ObservableCollection<Location> LocationList { get; set; }
        int SelectedIndex { get; set; }
        int PreviousSelectionIndex { get; set; }

        RestService service { get; set; }
        public ChangeLocationView()
        {
            int index = 0;
            InitializeComponent();
            service = new RestService();
            Location[] AvailableLocations = service.GetAllLocations().Result;
            LocationList = new ObservableCollection<Location>(AvailableLocations);

            for(int i = 0; i < LocationList.Count; i++)
            {
                if(LocationList[i].LocationId.Equals(App.LOCATION_ID.ToString()))
                {
                    index = i;
                    break;
                }
            }

            LocationListView.SelectedItem = LocationList[index];
            PreviousSelectionIndex = index;
            BindingContext = this;
        }

/*        public async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(initialised)
            {

            }
        }*/


        public async void ListItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

            Location selectedLocation = (Location)LocationListView.SelectedItem;
            //Location selectedLocation = LocationList[index];

            if(selectedLocation != LocationList[PreviousSelectionIndex])
            {
                var result = await DisplayAlert("Alert", $"Set Location #{selectedLocation.LocationId} - {selectedLocation.LocationName} as this device's location?", "Yes", "No");

                if (!result)
                {
                    LocationListView.SelectedItem = LocationList[PreviousSelectionIndex];
                }
                else
                {
                    App.LOCATION_ID = int.Parse(selectedLocation.LocationId);
                    App.LOCATION_NAME = selectedLocation.LocationName;
                }

            }
        }
    }
}