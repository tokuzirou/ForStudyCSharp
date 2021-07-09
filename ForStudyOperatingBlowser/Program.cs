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
        static async Task Main(string[] args)
        {
            GetCookies getCookies = new GetCookies();
            IReadOnlyCollection<Cookie> cookies = getCookies.GetAllCookies();
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            await getCookies.WriteCookieFile(cookies, directoryPath, filePath);
            Console.WriteLine("少々お待ちください...");
            Console.Read();
        }
    }
}
