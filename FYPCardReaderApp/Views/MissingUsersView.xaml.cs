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
    public partial class MissingUsersView : ContentPage
    {
        public ObservableCollection<Person> PersonList { get; set; }
        public MissingUsersView()
        {
            InitializeComponent();
            RestService service = new RestService();
            Person[] MissingPersons = service.GetMissingUsers().Result;
            PersonList = new ObservableCollection<Person>(MissingPersons);

            BindingContext = this;
        }
    }
}