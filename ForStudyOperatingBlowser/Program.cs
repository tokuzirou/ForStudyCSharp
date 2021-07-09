using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA;
using OpenQA.Selenium;

namespace ForStudyOperatingBlowser
{
    class Program
    {
        static async Task Main()
        {
            GetCookies getCookies = new GetCookies();
            IReadOnlyCollection<Cookie> cookies = await Task.Run(() => getCookies.GetAllCookies());
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            //getIOAsyncとWriteCookieFileAsyncはスレッドセーフ
            await getCookies.getIOAsync(directoryPath, filePath);
            await getCookies.WriteCookieFileAsync(cookies, directoryPath, filePath);
            //await Task.Run(() => { for (int i = 0; i < 100000; i++)
            //        Console.WriteLine(i);
            //});
            Console.Read();
        }
    }
}
