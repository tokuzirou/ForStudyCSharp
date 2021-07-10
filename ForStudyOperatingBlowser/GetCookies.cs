using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ForStudyOperatingBlowser
{
    class GetCookies
    {
        private readonly EdgeDriverService service = EdgeDriverService.CreateDefaultService(@"C:\webDriver", "msedgedriver.exe");

        public IWebDriver Driver { get; }
        public IReadOnlyCollection<Cookie> Cookies { get; set; }

        public GetCookies()
        {
            this.Driver = new EdgeDriver(service);
        }

        public async Task<IReadOnlyCollection<Cookie>> GetAllCookiesAsync()
        {
            IReadOnlyCollection<Cookie> cookies = default;
            await Task.Run(() =>
            {
                cookies = Driver.Manage().Cookies.AllCookies;
            });
            return cookies;
        }

        public async Task GetIOAsync(string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            //タスクオブジェクトを返す(呼び出し元でawaitさせて、別のスレッドでタスクを起動させる)
            //呼び出し元のスレッドがフリーズしないことに意味がある＝タスク処理が遅いかもだけど、UIがフリーズしない
   　　     await Task.Run(() =>
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(absolutePath);
                Console.WriteLine("taskAの終了");
            }).ContinueWith((_) =>
            {
                Console.WriteLine("継続taskBの始まり");
                //上のTaskが完了次第、ContinueWithは実行される
                //CdontinueWithをTaskにすることも可能
                //awaitしない場合、当然このスレッド(非同期スレッド)ではフリーズし、メインスレッドではしない
                //ちなみに、Task.Runで非同期スレッドに飛ばされるので、この継続ブロックを終了するのを待たずに、次の継続が実行される可能性有
                if (!File.Exists(absolutePath))
                    File.Create(absolutePath).Close();
                Console.WriteLine("CDCCC");
                Console.WriteLine("継続taskBの終了");
            }).ContinueWith((_) =>
            {
                Console.WriteLine("継続taskCの始まり");
                //ここでawaitだとasync Mainの最後のコンソールが表示されることになるので、Waitで明示的に待つ
                WriteFile().Wait();
            });

            async Task WriteFile()
            {
                using (StreamWriter sw = new StreamWriter(absolutePath))
                {
                    sw.WriteLine(Cookies.ToString());
                    Console.WriteLine("GGGGGGGG");
                }
                Console.WriteLine("継続taskCの終了(上のタスクは別の非同期スレッドに飛ばされたまま)");
                await Task.Delay(2000).ContinueWith((taska) => Console.WriteLine(taska.Status));
            }
        }
    }
}
