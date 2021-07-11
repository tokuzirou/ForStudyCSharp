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
    internal static class ControllWeatherAPI
    {
        public static string ApiKey { get; set; }

        internal static async Task<Dictionary<DateTime, string>> FetchAsync()
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=1850147&units=metric&lang=ja&appid=" + ApiKey;
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
                try
                {
                    using (WebResponse myHttpWebResponse = await myHttpWebResponseTask)
                    {
                        stopwatch.Stop();
                        TimeSpan timeSpan = stopwatch.Elapsed;
                        Console.WriteLine($"ダウンロード経過時間: {timeSpan.Milliseconds}msec");
                        Console.WriteLine($"task number {taskId} is done.");
                        Console.WriteLine("ダウンロード完了");
                        Stream myHttpWebResponseStream = myHttpWebResponse.GetResponseStream();
                        using (var streamReader = new StreamReader(myHttpWebResponseStream))
                        {
                            string jsonString = streamReader.ReadToEnd();
                            jsonDict[dateTime] = jsonString;
                        }
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                try
                {
                    await Task.Delay(1000);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return jsonDict;
        }

        internal static async Task<JsonElement> DeserializeAsync(string jsonString)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            JsonDocument document = default;
            using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(jsonString)))
            {
                try
                {
                    using (Task<JsonDocument> documentTask = JsonDocument.ParseAsync(memoryStream))
                        document = await documentTask;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return document.RootElement.Clone();
        }

        internal static async Task Move(string directoryPath)
        {
            try
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(directoryPath);
                    Directory.SetCurrentDirectory(directoryPath);
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal static async Task Save(JsonElement collection, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                JsonElement document = collection.GetProperty("weather");
                JsonElement.ArrayEnumerator weatherCollection = document.EnumerateArray(); 
                Task[] tasks = new Task[3];
                //LINQで書きたい
                foreach (JsonElement weather in weatherCollection)
                { 
                    TakeWeatherIconAsync(weather);
                    tasks[0] = Task.Run(() =>
                    {
                        sw.WriteLine("id:" + weather.GetProperty("id"));
                    });
                    tasks[1] = Task.Run(() =>
                    {
                        sw.WriteLine("main:" + weather.GetProperty("main"));
                    });
                    tasks[2] = Task.Run(() =>
                    {
                        sw.WriteLine("description:" + weather.GetProperty("description"));
                    });
                    try
                    {
                        await Task.WhenAll(tasks);
                    }catch(AggregateException agex)
                    {
                        Console.WriteLine(agex.ToString());
                    }
                }
            }

            void TakeWeatherIconAsync(JsonElement weather)
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
