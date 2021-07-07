using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace ForStudy
{
    class Program
    {
        static void Main()
        {
            ControllWeatherAPI controllWeatherAPI = new ControllWeatherAPI();
            controllWeatherAPI.apiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            Task<Dictionary<DateTime, string>> keyValuePairs = controllWeatherAPI.FetchAsync();
            foreach (var (directoryPath, filePath, jsonElementTask) in from KeyValuePair<DateTime, string> keyValuePair in keyValuePairs.Result
                                                                       let dateTime = keyValuePair.Key
                                                                       let jsonString = keyValuePair.Value
                                                                       let datePath = dateTime.ToString("yyyy-MM-dd-HH-mm-ss")
                                                                       let directoryPath = @"C:\Users\user\weather\current\" + datePath
                                                                       let filePath = datePath + ".txt"
                                                                       let jsonElementTask = controllWeatherAPI.DeserializeAsync(jsonString)
                                                                       select (directoryPath, filePath, jsonElementTask))
            {
                ControllWeatherAPI.Save(jsonElementTask.Result, filePath, directoryPath);
            }

            Console.WriteLine("作業終了");
            Console.ReadLine();
        }
    }
}
