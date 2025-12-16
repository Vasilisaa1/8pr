using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Models;

namespace Weather.Classes
{
    public class WeatherS
    {
        private const int DailyLimit = 3;

        public static async Task<DataResponse> GetWeatherCached(string city)
        {
            using (var db = new WDB())
            {
                var cache = db.Cache.FirstOrDefault(x => x.City == city);

                if (cache != null && (DateTime.Now - cache.Save).TotalHours < 1)
                {
                    return JsonConvert.DeserializeObject<DataResponse>(cache.Dataa);
                }
                var today = DateTime.Today;

                var usage = db.ApiUsage.FirstOrDefault(x => x.Day == today);
                if (usage == null)
                {
                    usage = new ApiWeather { Day = today, Count = 0 };
                    db.ApiUsage.Add(usage);
                }

                if (usage.Count >= DailyLimit)
                    throw new Exception("Лимит запросов на сегодня исчерпан");
                var (lat, lon) = await GeocodingService.GetCoordinates(city);
                var data = await GetWeather.Get(lat, lon);

                usage.Count++;
                db.SaveChanges();
                if (cache == null)
                {
                    cache = new Cash
                    {
                        City = city,
                        Lat = lat,
                        Lon = lon,
                        Dataa = JsonConvert.SerializeObject(data),
                        Save = DateTime.Now
                    };
                    db.Cache.Add(cache);
                }
                else
                {
                    cache.Dataa = JsonConvert.SerializeObject(data);
                    cache.Save = DateTime.Now;
                }

                db.SaveChanges();

                return data;
            }
        }
    }
}