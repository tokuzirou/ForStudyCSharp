using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForStudy
{
    internal class ControllWeatherAPI
    {
        public string apiKey { get; set; }

        internal async Task<Dictionary<DateTime, string>> FetchAsync()
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=1850147&units=metric&lang=ja&appid=" + apiKey;
            Console.WriteLine("何回取得しますか?");
            int count = int.Parse(Console.ReadLine());
            IEnumerable<int> countArray = Enumerable.Range(0, count);
            Dictionary<DateTime, string> jsonDict = new Dictionary<DateTime, string>();
            Stopwatch stopwatch = new Stopwatch();
            foreach (var (myWebRequest, dateTime) in from index in countArray
                                                     let myWebRequest = WebRequest.Create(uri)
                                                     let dateTime = DateTime.Now
                                                     select (myWebRequest, dateTime))
            {
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
                    string jsonString = streamReader.ReadToEnd();
                    jsonDict[dateTime] = jsonString;
                }

                myHttpWebResponse.Close();
                await Task.Delay(1000);
            }

            return jsonDict;
        }

        internal async Task<JsonElement> DeserializeAsync(string jsonString)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(jsonString)))
            {
                using (Task<JsonDocument> documentTask = JsonDocument.ParseAsync(memoryStream))
                {
                    JsonDocument document = await documentTask;
                    return document.RootElement.Clone();
                }
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
                    TakeWeatherIcon(weather);
                    sw.WriteLine("id:" + weather.GetProperty("id"));
                    sw.WriteLine("main:" + weather.GetProperty("main"));
                    sw.WriteLine("description:" + weather.GetProperty("description"));
                }
            }

            void TakeWeatherIcon(JsonElement weather)
            {
                string weatherIcon = weather.GetProperty("icon").ToString();
                string donloadOption = "2x.png";
                string uri = @"https://openweathermap.org/img/wn/" + weatherIcon + '@' + donloadOption;
                Uri uri1 = new Uri(uri);
                WebClient webClient = new WebClient();
                webClient.DownloadFileAsync(uri1, donloadOption);
            }
        }
    }
}
