
using System.Net.Http;
using Newtonsoft.Json;
using Weather.Models;

namespace Weather.Classes
{
    public static class GeocodingService
    {
        private static readonly string GeocodingUrl = "https://api.weather.yandex.ru/v2/forecast";
        private static readonly string ApiKey = "cf722107-8ced-4c06-b5ca-3a2957ac17c7"; // Замените на реальный ключ

        public static async Task<(double lat, double lon)> GetCoordinatesAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("Название города не может быть пустым");

            string url = $"{GeocodingUrl}?apikey={ApiKey}&geocode={Uri.EscapeDataString(city)}&format=json";

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(response);

                if (geocodingResponse?.Response?.GeoObjectCollection?.FeatureMember?.Count > 0)
                {
                    var pos = geocodingResponse.Response.GeoObjectCollection.FeatureMember[0]
                        .GeoObject.Point.Position;

                    var coords = pos.Split(' ');
                    if (coords.Length == 2)
                    {
                        return (double.Parse(coords[1]), double.Parse(coords[0])); // Широта, Долгота
                    }
                }

                throw new Exception("Координаты для указанного города не найдены");
            }
        }
    }
}