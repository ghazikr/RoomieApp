using DurianCode.PlacesSearchBar;
using RoomieApp.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoomieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookForRoom : ContentPage
    {
        private readonly GooglePlacesAPI googlePlacesApi;
        public static string radioBtnGender = "Any";
        private readonly string GooglePlacesApiKey = "AIzaSyA9rCfAhwnyRtAKLCddDmJq4EuYh3gnGT0"; // Replace this with your api key
        private List<pl> listPlaces;
        private pl p;
        private Place place;
        public LookForRoom()
        {
            InitializeComponent();
            search_bar.ApiKey = GooglePlacesApiKey;
            search_bar.Type = PlaceType.Cities;
            search_bar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
            search_bar.TextChanged += Search_Bar_TextChanged;
            search_bar.MinimumSearchText = 2;
            results_list.ItemSelected += Results_List_ItemSelected;
            googlePlacesApi = new GooglePlacesAPI();
            listPlaces = new List<pl>();
        }

        private void Search_Bar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            results_list.ItemsSource = result.AutoCompletePlaces;
            spinner.IsRunning = false;
            spinner.IsVisible = false;

            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                results_list.IsVisible = true;
        }



        private void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                results_list.IsVisible = false;
                spinner.IsVisible = true;
                spinner.IsRunning = true;
            }
            else
            {
                results_list.IsVisible = false;
                spinner.IsRunning = false;
                spinner.IsVisible = false;
            }
        }

        private async void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            results_list.SelectedItem = null;
            search_bar.Text = prediction.Description;
            place = await Places.GetPlace(prediction.Place_ID, GooglePlacesApiKey);


            /*if (place != null)
                await DisplayAlert(
                    place.Name, string.Format("Lat: {0}\nLon: {1}", place.Latitude, place.Longitude), "OK");
    */
            
            
            p = new pl
            {
                name = place.Name,
                latitude = place.Latitude.ToString(),
                longitude = place.Longitude.ToString()

            };
            results_list.IsVisible = false;

        }
        

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (place != null)
            {


                var list = await googlePlacesApi.getPlacesLocation(approxRent.Text + "&" + radioBtnGender);
                var listPlacesNearBy = await googlePlacesApi.getNearbyPlaces(p, list);
                listPlaces = listPlacesNearBy;
                var a = 3;
            }
            await Navigation.PushAsync(new AvailableRooms(listPlaces));
        }

        private void radio_button_clicked(object sender, EventArgs e)
        {
            var b =sender as Button;
            b.BorderColor = Color.FromHex("#1E59A7");
            
            switch (b.Text)
            {
                case "Male":
                    btnFemale.BorderColor = Color.FromHex("#95989A");
                    btnAny.BorderColor = Color.FromHex("#95989A");
                    radioBtnGender = "Male";
                    break;
                case "Female":
                    btnMale.BorderColor = Color.FromHex("#95989A");
                    btnAny.BorderColor = Color.FromHex("#95989A");
                    radioBtnGender = "Female";
                    break;
                case "Any":
                    btnMale.BorderColor = Color.FromHex("#95989A");
                    btnFemale.BorderColor = Color.FromHex("#95989A");
                    radioBtnGender = "Any";
                    break;
            }
                 
            
        }
    }
}