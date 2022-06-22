using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CAIR_Schedule.Models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CAIR_Schedule
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (!App.Current.Properties.TryGetValue("logged", out _))
            {
                App.Current.Properties["logged"] = false;
            }

            if (App.Current.Properties["logged"] is false)
            {
                ToModalPage(this, null);
            }
            else SetData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetData();
        }
        private void SetData()
        {
            Database database = Database.getDatabase();
            List<Item> items_list = database.getData();
            Items = new ObservableCollection<Item>();
            foreach (var item in items_list)
            {
                Items.Add(new Item
                {
                    Group_name = item.Group_name,
                    Comment = item.Comment,
                    Date_time = item.Date_time,
                    Name = item.Name,
                });
            }
            LessonsListView.ItemsSource = Items;
        }

        public ObservableCollection<Item> Items { get; set; }
        private async void ToModalPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Page1(), true);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            App.Current.Properties["logged"] = false;
            ToModalPage(this, null);
        }

        private void LessonsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void LessonsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
