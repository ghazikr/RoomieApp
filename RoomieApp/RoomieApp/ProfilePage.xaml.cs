using FacebookLogin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RoomieApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(FacebookProfile fp)
        {
            InitializeComponent();
            BindingContext = fp;
        }
    }
}