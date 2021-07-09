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
            IReadOnlyCollection<Cookie> cookies = await Task.Run(() => getCookies.GetAllCookies());
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            Task task = getCookies.WriteCookieFileAsync(cookies, directoryPath, filePath);
            //呼び出し元(メインスレッド)に戻った時にすぐに待機処理が実行される
            //当然完了通知はタスクが完了してからとなる
            task.Wait();
            Console.WriteLine("完了しました");
            Console.Read();
        }
    }
}
