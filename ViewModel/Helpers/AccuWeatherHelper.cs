using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete?apikey={0}&q={1}";
        public const string API_KEY = "GIpST6gWUB6VHMa1GSGBQjHUAFqhv3pD";
        public const string CURRENT_CONDITIONS_ENDPOINT = "currentconditions/v1/{0}?apikey={1}";



        //WRITE A METHOD THAT WILL MAKE A REQUEST TO THE API
        public static async Task<List<City>> GetCities(string query)
        {
            List<City> cities = new List<City>();
            string URL = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY,query);
            //now we have a URL so we are ready to make a request 

            using(HttpClient client= new HttpClient())
            {
                //make a request
                var response = await client.GetAsync(URL);
                var json= await response.Content.ReadAsStringAsync();

                cities=  JsonConvert.DeserializeObject<List<City>>(json);

            }
            return cities;
        }

        public static async Task<CurrentConditions> GetCurrentCondition(string cityKey)
        {
            CurrentConditions currentConditions = new CurrentConditions();
            string URL = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT,cityKey, API_KEY);
            //now we have a URL so we are ready to make a request 

            using (HttpClient client = new HttpClient())
            {
                //make a request
                var response = await client.GetAsync(URL);
                var json = await response.Content.ReadAsStringAsync();

                //to get current condition of one city out of the list of current conditions use the first or default method 

                currentConditions = (JsonConvert.DeserializeObject<List<CurrentConditions>>(json)).FirstOrDefault();

            }


            return currentConditions;
        }
    }
}
