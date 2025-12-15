using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Models;

namespace Weather.Classes
{
   
        public static class GetWeather
        {
            public static string Url = "https://api.weather.yandex.ru/v2/forecast";
            public static string Key = "demo_yandex_weather_api_key_ca6d09349ba0"; // Замените на реальный

            public static async Task<DataResponse> Get(float lat, float lon)
            {
                string url = $"{Url}?lat={lat}&lon={lon}".Replace(",", ".");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Yandex-Weather-Key", Key);
                    client.Timeout = TimeSpan.FromSeconds(10);

                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DataResponse>(content);
                }
            }
        }

    
}
