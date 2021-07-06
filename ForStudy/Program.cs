using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ForStudy
{
    class Program
    {
        static async Task Main()
        {
            string apiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            Dictionary<DateTime, string> keyValuePairs = ControllWeatherAPI.Fetch(apiKey).Result;
            foreach (KeyValuePair<DateTime, string> keyValuePair in keyValuePairs)
            {
                DateTime dateTime = keyValuePair.Key;
                string jsonString = keyValuePair.Value;
                string datePath = dateTime.ToString("yyyy-MM-dd-HH-mm-ss");
                string directoryPath = @"C:\Users\user\weather\current\" + datePath;
                string filePath = datePath + ".txt";
                JsonElement jsonElement = ControllWeatherAPI.Deserialize(jsonString);
                ControllWeatherAPI.Save(jsonElement, filePath, directoryPath);
            }
            Console.WriteLine("作業終了");
            Console.ReadLine();
        }
    }
}
