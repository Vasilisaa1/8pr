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
        private const int DailyLimit = 50; // лимит в день

        public static async Task<DataResponse> GetWeatherCached(string city)
        {
            using (var db = new WDB())
            {
                // 1. Пробуем взять из кэша
                var cache = db.Cache.FirstOrDefault(x => x.City == city);

                if (cache != null && (DateTime.Now - cache.Save).TotalHours < 1)
                {
                    // данные свежие – возвращаем из БД
                    return JsonConvert.DeserializeObject<DataResponse>(cache.Dataa);
                }

                // 2. Проверяем лимит на сегодня
                var today = DateTime.Today;
                var usage = db.ApiUsage.FirstOrDefault(x => x.Day == today);

                if (usage == null)
                {
                    usage = new ApiWeather { Day = today, Count = 0 };
                    db.ApiUsage.Add(usage);
                }

                if (usage.Count >= DailyLimit)
                {
                    throw new Exception($"Лимит {DailyLimit} запросов на сегодня исчерпан");
                }

                // 3. Идём во внешние API
                var (lat, lon) = await GeocodingService.GetCoordinates(city);
                if (lat == 0 && lon == 0)
                    throw new Exception("Город не найден");

                var data = await GetWeather.Get(lat, lon);

                // 4. Увеличиваем счётчик запросов
                usage.Count++;
                db.SaveChanges();

                // 5. Обновляем кэш в БД
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
                    cache.Lat = lat;
                    cache.Lon = lon;
                    cache.Dataa = JsonConvert.SerializeObject(data);
                    cache.Save = DateTime.Now;
                }

                db.SaveChanges();

                return data;
            }
        }
    }
}
