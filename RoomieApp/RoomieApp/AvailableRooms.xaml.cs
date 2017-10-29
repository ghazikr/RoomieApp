using RoomieApp.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoomieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvailableRooms : ContentPage
    {

        public AvailableRooms(List<pl> listplaces)
        {
            InitializeComponent();
            if(listplaces.Count > 0)
            {
                listView.ItemsSource = listplaces;
                rooms.IsVisible = false;
            }

             




            var a = 5;
            // listView = listplaces;

        }
        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var parm = ((Image)sender).ClassId;
            await DisplayAlert("Success", "The owner's contact number: "+parm, "ok");
        }
    }
}