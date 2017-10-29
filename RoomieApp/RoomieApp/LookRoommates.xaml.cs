using Newtonsoft.Json;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RoomieApp.Models;
using RoomieApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DurianCode.PlacesSearchBar;
using RoomieApp.Services;

namespace RoomieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookRoommates : ContentPage
    {
        private string imgURL;
        private MediaFile file;
        private string radioBtnGender = "Any";
        
        private dbServices dbServices;
        private readonly GooglePlacesAPI googlePlacesApi;
        private Place place=null;
        private readonly string GooglePlacesApiKey = "AIzaSyA9rCfAhwnyRtAKLCddDmJq4EuYh3gnGT0"; // Replace this with your api key
        private List<pl> listPlaces;
        

        public LookRoommates()
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
            dbServices = new dbServices();
            pickPhoto.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(
                    new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                    });


                if (file == null)
                    return;


            };
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
            
            results_list.IsVisible = false;





        }

        private async void btn_add_flat(object sender, EventArgs e)
        {
           

            var WebSiteUrl =dbServices.serverUrl + "/RoomieApp";
            var pathImage = await UploadFile();
            var FullPathImage = WebSiteUrl + "/Upload/" + pathImage;
            var x = 3;

            if (!string.IsNullOrWhiteSpace(pathImage))
            {


                 Flat flat = new Flat()
                 {

                     description = desc.Text,
                     price= Int32.Parse(approxRent.Text),
                     id_user=MainPage.currentFacebookProfile.Id,
                     sex_preference="Male",
                     longitude=place.Longitude.ToString(),
                     latitude = place.Latitude.ToString(),
                     name=place.Name,
                     imageURL=FullPathImage,
                     contactNumber= contactNumber.Text


                 };
                
                 var status1 = await dbServices.addFlat(flat);
                
                if (status1 == "1")
                {
                    await DisplayAlert("Success", "Flat Added", "ok");
                    var mainPage = ((NavigationPage)Application.Current.MainPage).CurrentPage;
                    var masterDetailPage = mainPage as MasterDetailPage;
                    if (masterDetailPage != null)
                    {
                        masterDetailPage.Detail = new NavigationPage(new MyPlaces(MainPage.currentFacebookProfile))
                        {
                            BarBackgroundColor = Color.FromHex("#0A8754")
                        };

                       

                    }


                   }
                else
                {
                    await DisplayAlert("Error", "Try again !", "Ok");
                }
            }
        }

        private void radio_button_clicked(object sender, EventArgs e)
        {
            var b = sender as Button;
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

        private async Task<string> UploadFile()
        {
            // made copy of file and rename it
            IFile OriginalFile = await FileSystem.Current.GetFileFromPathAsync(file.Path);

            var newFile = Path.GetDirectoryName(OriginalFile.Path) + DateTime.Now.Ticks + Path.GetExtension(file.Path);

            DependencyService.Get<IFileHelper>().Copy(OriginalFile.Path, newFile);

            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(file.GetStream()),
                "\"file\"",
                $"\"{newFile}\"");

            var httpClient = new HttpClient();

            var uploadServiceBaseAddress = dbServices.serverUrl + "/RoomieApp/upload.php";


            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);

            var serverResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            var serverResponseDeserialized = JsonConvert.DeserializeObject<IDictionary<string, string>>(serverResponse);
            string pathImage = "";

            serverResponseDeserialized.TryGetValue("path", out pathImage);
            // delete file
            //file.Dispose();
            return pathImage;
        }
    }
}