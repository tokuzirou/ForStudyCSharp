using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForStudy
{
    class Program
    {
        static async Task Main()
        {
            ControllWeatherAPI.ApiKey = "95712e3e136a6e7457e6fc6502f0afe3";
            Dictionary<DateTime, string> keyValuePairs = await ControllWeatherAPI.FetchAsync();
            try
            {
                await Task.Run(async () =>
                {
                    foreach (var (directoryPath, filePath, jsonElementTask) in from KeyValuePair<DateTime, string> keyValuePair in keyValuePairs
                                                                               let jsonString = keyValuePair.Value
                                                                               let jsonElementTask = ControllWeatherAPI.DeserializeAsync(jsonString)
                                                                               let dateTime = keyValuePair.Key
                                                                               let datePath = dateTime.ToString("yyyy-MM-dd-HH-mm-ss")
                                                                               let directoryPath = @"C:\Users\user\weather\current\" + datePath
                                                                               let filePath = datePath + ".txt"
                                                                               select (directoryPath, filePath, jsonElementTask))
                    {
                        await ControllWeatherAPI.Move(directoryPath);
                        await ControllWeatherAPI.Save(await jsonElementTask, filePath);
                    }
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("作業終了");
            Console.ReadLine();
        }
    }
}
