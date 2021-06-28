using System;
using System.Text.Json;

namespace ForStudy
{
    class Program
    { 
        static void Main()
        {
            string apiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            string jsonString = ControllWeatherAPI.Fetch(apiKey);
            JsonElement collection = ControllWeatherAPI.Deserialize(jsonString);
            string datePath = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string filePath = datePath + ".txt";
            string directoryPath = @"C:\Users\user\weather\current\" + datePath;
            ControllWeatherAPI.Save(collection, filePath, directoryPath);
            Console.WriteLine("終了");
        
            Console.ReadLine();
        }
    }
}
