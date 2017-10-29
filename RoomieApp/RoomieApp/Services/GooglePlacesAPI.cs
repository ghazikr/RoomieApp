using Newtonsoft.Json;
using RoomieApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoomieApp.Services
{
    public class pl
    {
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public double price { get; set; }
        public double distanceToSelectedPlace { get; set; }
        public string imageURL { get; set; }
        public string description { get; set; }
        public string contact_number { get; set; }

    }

    public class GooglePlacesAPI
    {
        private readonly string comparePlacesURL =
                "https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={0}&destinations={1}&key=AIzaSyC_TEotfWeoWAvKfaK1UXjF8f27LeODYsc"
            ;

       
        
        public async Task<List<pl>> getPlacesLocation(string priceANDgender)
        {
            var w=priceANDgender.Split('&');
            var price = w[0];
            var radioBtnGender = w[1];
            var client = new HttpClient();

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("price", price);
            values.Add("radioBtnGender", radioBtnGender);
            var content = JsonConvert.SerializeObject(values);
            
            //var content = JsonConvert.SerializeObject("");


            var response = await client.PostAsync(dbServices.serverUrl + "/RoomieApp/getAllPlacesLocation.php",
                new StringContent(content, Encoding.UTF8, "text/json"));
            var serverResponse = await response.Content.ReadAsStringAsync();

            var PlacesLocations = JsonConvert.DeserializeObject<List<pl>>(serverResponse);


            //            var list = new List<KeyValuePair<string, string>>() {
            //                new KeyValuePair<string, string>("A", 1),
            //
            //            };
            return PlacesLocations;
        }

        public async Task<List<pl>> getNearbyPlaces(pl selectedPlace, List<pl> list)
        {
            var listNearby = new List<pl>();
            var latSelectedPlace = selectedPlace.latitude;
            var lonSelectedPlace = selectedPlace.longitude;
            var selectedPlaceDetails = latSelectedPlace + "," + lonSelectedPlace;
            for (var i = 0; i < list.Count; i++)
            {
                //var PlaceNearBy = await googlePlacesApi.comparePlaces(p, list[i]);
                // listNearby.Add(PlaceNearBy);

                var lat = list[i].latitude;
                var lon = list[i].longitude;
                var placeDetails = lat + "," + lon;
                var client = new HttpClient();
                var jsonResult =
                    await client.GetStringAsync(string.Format(comparePlacesURL, selectedPlaceDetails, placeDetails));
                var res = JsonConvert.DeserializeObject<ComparePlacesResult>(jsonResult);
                var distance = res.rows[0].elements[0].distance.value;

                           
                list[i].distanceToSelectedPlace = distance;

                if (distance / 1000 < 30)

                    listNearby.Add(list[i]);
            }

            return listNearby;
        }
    }
}