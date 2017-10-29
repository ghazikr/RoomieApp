using FacebookLogin.Models;
using FacebookLogin.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoomieApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private FacebookProfile facebookProfile;
        public Page1()
        {
            InitializeComponent();

        }
        public Page1(FacebookProfile facebookProfile)
        {
            this.facebookProfile = facebookProfile;

        }

        private async void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FacebookProfileCsPage());

        }
    }
}