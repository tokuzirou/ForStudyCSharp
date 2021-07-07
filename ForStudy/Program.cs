using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForStudy
{
    class Program
    {
        static void Main()
        {
            ControllWeatherAPI controllWeatherAPI = new ControllWeatherAPI();
            controllWeatherAPI.apiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            Task<Dictionary<DateTime, string>> keyValuePairs = controllWeatherAPI.FetchAsync();
            foreach (KeyValuePair<DateTime, string> keyValuePair in keyValuePairs.Result)
            {
                DateTime dateTime = keyValuePair.Key;
                string jsonString = keyValuePair.Value;
                string datePath = dateTime.ToString("yyyy-MM-dd-HH-mm-ss");
                string directoryPath = @"C:\Users\user\weather\current\" + datePath;
                string filePath = datePath + ".txt";
                Task<JsonElement> jsonElementTask = controllWeatherAPI.DeserializeAsync(jsonString);
                ControllWeatherAPI.Save(jsonElementTask.Result, filePath, directoryPath);
            }
            Console.WriteLine("作業終了");
            Console.ReadLine();
        }
    }
}
