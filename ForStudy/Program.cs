using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForStudy
{
    class Program
    { 
        static async Task Main()
        {
            string apiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            ControllWeatherAPI controllWeather = new ControllWeatherAPI();
            bool checkContinuing;
            //Mainメソッドでループしているが、ControllWeatherAPI.cs内でTaskをコレクションして、一定数たまったら、
            //LINQでラムダ式に対してタスク継続処理すればよい

            do
            {
                Task<string> jsonString = controllWeather.Fetch(apiKey);
                JsonElement collection = ControllWeatherAPI.Deserialize(jsonString.Result);
                string datePath = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                string filePath = datePath + ".txt";
                string directoryPath = @"C:\Users\user\weather\current\" + datePath;
                ControllWeatherAPI.Save(collection, filePath, directoryPath);
                Console.WriteLine("終了");
                //あと何回続けるか、にしたい
                Console.WriteLine("続けますか Yes, No:");
                string readString = Console.ReadLine();
                checkContinuing = readString == "Yes" ? true : false;
                await Task.Delay(1000);
            } while (checkContinuing);
        
            Console.ReadLine();
        }
    }
}
