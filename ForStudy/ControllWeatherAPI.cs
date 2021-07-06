using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForStudy
{
    internal class ControllWeatherAPI
    {
        internal static async Task<Dictionary<DateTime, string>> Fetch(string apiKey)
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=1850147&units=metric&lang=ja&appid=" + apiKey;
            Console.WriteLine("何回取得しますか?");
            int count = int.Parse(Console.ReadLine());
            IEnumerable<int> countArray = Enumerable.Range(0, count);
            string jsonString = default(string);
            Dictionary<DateTime, string> jsonDict = new Dictionary<DateTime, string>();
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                foreach (var index in countArray)
                {
                    WebRequest myWebRequest = WebRequest.Create(uri);
                    DateTime dateTime = DateTime.Now;
                    stopwatch.Start();
                    Task<WebResponse> myHttpWebResponseTask = myWebRequest.GetResponseAsync();
                    int taskId = myHttpWebResponseTask.Id;
                    WebResponse myHttpWebResponse = await myHttpWebResponseTask;
                    stopwatch.Stop();
                    TimeSpan timeSpan = stopwatch.Elapsed;
                    Console.WriteLine($"ダウンロード経過時間: {timeSpan.Milliseconds}msec");
                    Console.WriteLine($"task number {myHttpWebResponseTask.Id} is done.");
                    Console.WriteLine("ダウンロード完了");
                    Stream myHttpWebResponseStream = myHttpWebResponse.GetResponseStream();
                    using (var streamReader = new StreamReader(myHttpWebResponseStream))
                    {
                        jsonString = streamReader.ReadToEnd();
                    }
                    myHttpWebResponse.Close();
                    await Task.Delay(1000);
                    jsonDict[dateTime] = jsonString;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return jsonDict;
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
