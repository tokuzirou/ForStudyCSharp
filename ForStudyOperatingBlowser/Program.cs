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
            //オブジェクトを渡したときは、実行されない
            //Task<IReadOnlyCollection<Cookie>> cookieTask = Task.Run(() => getCookies.GetAllCookies());
            //when i write the awaitable code, the code is executing.
            //IReadOnlyCollection<Cookie> cookie = await cookieTask;

            //下と同様awaitはCookieをとってきて、Aを表示するとこまで待つけど、その中のTaskは別スレッドなので待たないでBvを表示
            getCookies.Cookies = await Task.Run(() => getCookies.GetAllCookies());
            Console.WriteLine("Bv");
            string directoryPath = @"C:\Users\user\cookies_edge";
            string filePath = "cookies.txt";
            //awaitは飛ばした先でのTaskは感知して待つけど、飛ばした先でのさらに先の飛ばした先が存在する場合、それは待たずに下に行く
            await getCookies.GetIOAsync(directoryPath, filePath);
            Console.WriteLine("aaa");
            Console.Read();
        }
    }
}
