using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FYPCardReaderApp.Models;
using System.Collections.ObjectModel;
using FYPCardReaderApp.Responses;

namespace FYPCardReaderApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MissingUsersView : ContentPage
    {
        public ObservableCollection<Person> PersonList { get; set; }
        RestService service { get; set; }
        public MissingUsersView()
        {
            InitializeComponent();
            service = new RestService();
            Person[] MissingPersons = service.GetMissingUsers().Result;
            PersonList = new ObservableCollection<Person>(MissingPersons);

            BindingContext = this;
        }

        public async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int index = e.SelectedItemIndex;
            Person selectedPerson = PersonList[index];
            IndividualUserResponse resp = new IndividualUserResponse();
            resp.UserId = selectedPerson.Id;
            var result = await DisplayAlert("Alert", $"Mark {selectedPerson.Forename} {selectedPerson.Surname} as safe?", "Yes", "No");

            if(result == true)
            {
                service.PostUserIsSafe(resp);
                PersonList.RemoveAt(index);
            }


        }
    }
}