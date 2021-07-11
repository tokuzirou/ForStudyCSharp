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
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            //async Task Mainの呼び出し元であるvoid Mainに戻り、cookieTaskの実行完了を待つまで、下の行には行かない
            await getCookies.GetAllCookiesAsync();
            Console.WriteLine("awaitでcookiesを取得できた！");
            //上同様
            await getCookies.GetIOAsync(directoryPath, filePath);
            Console.WriteLine("awaitでTask型のオブジェクトを終わるまで待機した！");
            Console.Read();
        }
    }
}
