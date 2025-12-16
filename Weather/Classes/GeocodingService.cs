
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Weather.Models;

namespace Weather.Classes
{
    public static class GeocodingService
    {
        public static string ApiKey = "e69374b5-3e13-4228-8235-6bb574aa5cc4";

        public static async Task<(float lat, float lon)> GetCoordinates(string city)
        {
            try
            {
                string url = $"https://geocode-maps.yandex.ru/1.x/?apikey={ApiKey}&geocode={city}&format=json";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    var featureMember = json["response"]?["GeoObjectCollection"]?["featureMember"];

                    if (featureMember == null || !featureMember.Any())
                    {

                        return (0, 0);
                    }
                    
                    var pos = json["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"].ToString();
                    string[] coords = pos.Split(' ');

                    float lon = Convert.ToSingle(coords[0].Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                    float lat = Convert.ToSingle(coords[1].Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

                    return (lat, lon);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка геокодера: {ex.Message}");
                return (0, 0);
            }
        }
    }
}