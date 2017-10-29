
using FacebookLogin.Models;
using RoomieApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoomieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPlaces : ContentPage
    {
        private FacebookProfile _facebookProfile;
        private dbServices DbServices = new dbServices();
        public MyPlaces(FacebookProfile facebookProfile)
        {
            InitializeComponent();
            
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);// SetHasBackButton(this, false);
            _facebookProfile = facebookProfile;
            
            

        }

        protected async override void OnAppearing()
        {
            var x = await DbServices.getAllLocationsByUserId(_facebookProfile.Id);
            if (x.Count > 0)
            {
                labPlaces.IsVisible = false;
                lv.ItemsSource = x;
            }
               
           
        }
    }
}