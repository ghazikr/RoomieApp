using FacebookLogin.Models;
using RoomieApp.Models;
using RoomieApp.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RoomieApp
{
    public partial class MainPage : MasterDetailPage
    {
        public static FacebookProfile currentFacebookProfile;
        public MainPage(FacebookProfile fp)
        {
            InitializeComponent();
            BindingContext = fp;
            currentFacebookProfile = fp;
            profIcon.Source = fp.Picture.Data.Url;
            NavigationPage.SetHasNavigationBar(this, false);

            Detail = new NavigationPage(new LookForRoom())
            {
                BarBackgroundColor = Color.FromHex("#0A8754")
            };

            //Nav Drawer

            var items = new List<MenuItemNav>()
            {
                new MenuItemNav
                {
                    itemName = "Look for a room",
                    itemIcon = "search.png"
                },
                new MenuItemNav
                {
                    itemName = "Roommate",
                    itemIcon = "roommate.png"
                },
                new MenuItemNav
                {
                    itemName = "My places",
                    itemIcon = "ic_launcher.png"
                },

                new MenuItemNav
                {
                    itemName = "Logout",
                    itemIcon = "logout.png"
                }

            };
            listView.ItemsSource = items;


        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var tappedItem = e.SelectedItem as MenuItemNav;
            //(e.SelectedItem as ViewCell).View.BackgroundColor = Color.Brown;
            if (tappedItem.itemName == "Look for a room")
            {

                Detail = new NavigationPage(new LookForRoom())
                {
                    BarBackgroundColor = Color.FromHex("#0A8754")
                };
                IsPresented = false;
            }
            else if (tappedItem.itemName == "My places")
            {
                Detail = new NavigationPage(new MyPlaces(currentFacebookProfile))
                {
                    BarBackgroundColor = Color.FromHex("#0A8754")
                };
                IsPresented = false;
            }

            else if (tappedItem.itemName == "Roommate")
            {
                Detail = new NavigationPage(new LookRoommates())
                {
                    BarBackgroundColor = Color.FromHex("#0A8754")
                };
                IsPresented = false;
            }

            else if (tappedItem.itemName == "Logout")
            {
                Detail = new NavigationPage(new Page1())
                {
                    BarBackgroundColor = Color.FromHex("#0A8754")
                };
                IsPresented = false;
            }

            /*else if (tappedItem.itemName == "Pets for Adoption")
            {
                Detail = new NavigationPage(new PetsForAdoption(user.id))
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = false;
            }
            else if (tappedItem.itemName == "Logout")
            {
                Detail = new NavigationPage(new LoginUser())
                {
                    BarBackgroundColor = Color.FromHex("#894183")
                };
                IsPresented = false;
            }*/
            listView.SelectedItem = null;

        }






    }
}