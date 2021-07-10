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
            Task<IReadOnlyCollection<Cookie>> cookieTask = getCookies.GetAllCookiesAsync();
            Console.WriteLine("awaitで戻ってきた！");
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            //async Task Mainの呼び出し元であるvoid Mainに戻り、cookieTaskの実行完了を待つまで、下の行には行かない
            getCookies.Cookies = await cookieTask;
            Console.WriteLine("awaitでCookiesを取得できた！");
            Task getCookieTask = getCookies.GetIOAsync(directoryPath, filePath);
            await getCookieTask;
            Console.WriteLine("awaitでTask型のオブジェクトを終わるまで待機した！");
            Console.Read();
        }
    }
}
