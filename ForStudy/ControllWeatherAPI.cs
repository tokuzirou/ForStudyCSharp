using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text.Json;

namespace ForStudy
{
    internal static class ControllWeatherAPI
    {
        internal static string Fetch(string apiKey)
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=1850147&units=metric&lang=ja&appid=" + apiKey;
            WebRequest myWebRequest = WebRequest.Create(uri);
            HttpWebResponse myHttpWebResponse = myWebRequest.GetResponse() as HttpWebResponse;
            Stream myHttpWebResponseStream = myHttpWebResponse.GetResponseStream();
            string jsonString;
            using(var streamReader = new StreamReader(myHttpWebResponseStream))
            {
                jsonString = streamReader.ReadToEnd();
            }
            myHttpWebResponse.Close();
            return jsonString;
        }

        internal static JsonElement Deserialize(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                return document.RootElement.Clone();
            }
        }

        internal static void Save(JsonElement collection, string filePath, string directoryPath)
        {
            JsonElement document = collection.GetProperty("weather");
            JsonElement.ArrayEnumerator weatherCollection = document.EnumerateArray();
            Directory.CreateDirectory(directoryPath);
            Directory.SetCurrentDirectory(directoryPath);
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (JsonElement weather in weatherCollection)
                {
                    sw.WriteLine("id:" + weather.GetProperty("id"));
                    sw.WriteLine("main:" + weather.GetProperty("main"));
                    sw.WriteLine("description:" + weather.GetProperty("description"));
                    TakeWeatherIcon(weather);
                }
            }

            void TakeWeatherIcon(JsonElement weather)
            {
                string weatherIcon = weather.GetProperty("icon").ToString();
                string donloadOption = "2x.png";
                string uri = @"https://openweathermap.org/img/wn/" + weatherIcon + '@' + donloadOption;
                WebClient webClient = new WebClient();
                webClient.DownloadFile(uri, donloadOption);
            }
        }
    }
}
