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

        public IReadOnlyCollection<Cookie> GetAllCookies()
        { 
            IReadOnlyCollection<Cookie> cookies = Driver.Manage().Cookies.AllCookies;
            Console.WriteLine("A");
            Task.Delay(10000).ContinueWith((_) => Console.WriteLine("VVV"));
            return cookies;
        }

        public Task GetIOAsync(string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            //タスクオブジェクトを返す(呼び出し元でawaitさせて、別のスレッドでタスクを起動させる)
            //呼び出し元のスレッドがフリーズしないことに意味がある＝タスク処理が遅いかもだけど、UIがフリーズしない
            return Task.Run(() =>
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
                Task.Run(() =>
                {
                    //排他的に処理しても、こっちが先に実行されると、absolutePathにファイルが存在するとは限らないのでエラー
                    //変える必要有
                    //Taskをnewだけして、下のメソッドの中でawaitで呼び出す形式するか、ラムダにするか
                    using (StreamWriter sw = new StreamWriter(absolutePath))
                    {
                        sw.WriteLine(Cookies.ToString());
                        Console.WriteLine("GGGGGGGG");
                    }
                });
                Console.WriteLine("継続taskCの終了(上のタスクは別の非同期スレッドに飛ばされたまま)");
                Task.Delay(2000).ContinueWith((task) => Console.WriteLine(task.Status));
            });
        }
    }
}
