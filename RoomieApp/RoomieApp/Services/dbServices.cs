using FacebookLogin.Models;
using Newtonsoft.Json;
using RoomieApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoomieApp.Services
{
    public class dbServices
    {
        public static string serverUrl = "http://192.168.101.11";
        public async Task<List<pl>> getAllLocationsByUserId(string userId)
        {
            var client = new HttpClient();
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("UserID", userId);
            var content = JsonConvert.SerializeObject(values);


            var response = await client.PostAsync(serverUrl + "/RoomieApp/getAllLocationsByUserId.php",
                new StringContent(content, Encoding.UTF8, "text/json"));
            var serverResponse = await response.Content.ReadAsStringAsync();

            var Places = JsonConvert.DeserializeObject<List<pl>>(serverResponse);



            return Places;
        }

        public async Task<string> addFlat(Flat f)
        {
            var client = new HttpClient();

            var content = JsonConvert.SerializeObject(f);


            var response = await client.PostAsync(serverUrl + "/RoomieApp/AddFlat.php",
                new StringContent(content, Encoding.UTF8, "text/json"));

            //read the json response of the server
            var serverResponse = await response.Content.ReadAsStringAsync();

            var serverResponseDeserialized = JsonConvert.DeserializeObject<IDictionary<string, string>>(serverResponse);
            string status = "";

            serverResponseDeserialized.TryGetValue("status", out status);


            return status;
        }
        public async Task<string> addUserIfNotExist(FacebookProfile fp)
        {

            var client = new HttpClient();

            var content = JsonConvert.SerializeObject(fp);


            var response = await client.PostAsync(serverUrl + "/RoomieApp/addUserIfNotExist.php",
                new StringContent(content, Encoding.UTF8, "text/json"));

            //read the json response of the server
            var serverResponse = await response.Content.ReadAsStringAsync();

            var serverResponseDeserialized = JsonConvert.DeserializeObject<IDictionary<string, string>>(serverResponse);
            string status = "";

            serverResponseDeserialized.TryGetValue("status", out status);


            return status;
        }

    }


}
