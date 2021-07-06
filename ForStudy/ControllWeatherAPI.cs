using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Timers;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForStudy
{
    internal class ControllWeatherAPI
    {
        internal async Task<string> Fetch(string apiKey)
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=1850147&units=metric&lang=ja&appid=" + apiKey;
            WebRequest myWebRequest = WebRequest.Create(uri);
            Task<WebResponse> myHttpWebResponseTask = myWebRequest.GetResponseAsync();
            //Taskが実行している間の処理
            DateTime time;
            DateTime startTime = DateTime.Now;
            //Taskが実行完了するまで、無限ループ
            while (!myHttpWebResponseTask.IsCompleted)
            {
                time = DateTime.Now;
                using (Timer timer = new Timer(1000))
                {
                    timer.Elapsed += (sender, e) =>
                    {
                        Console.WriteLine($"ダウンロード経過時間: {time - startTime}");
                    };
                    timer.Start();
                }
                Console.Clear();
                Console.WriteLine($"ダウンロード経過時間: {time - startTime}");
            }
            //タスクの実行完了を待機
            WebResponse myHttpWebResponse = await myHttpWebResponseTask;
            //継続処理
            Console.WriteLine($"task number {myHttpWebResponseTask.Id} is done.");
            Console.WriteLine("ダウンロード完了");
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
