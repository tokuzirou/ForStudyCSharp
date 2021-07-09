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
        private readonly Object lockObject = new object();

        public IWebDriver Driver { get; }

        public GetCookies()
        {
            this.Driver = new EdgeDriver(service);
        }

        public IReadOnlyCollection<Cookie> GetAllCookies()
        { 
            IReadOnlyCollection<Cookie> cookies = Driver.Manage().Cookies.AllCookies;
            return cookies;
        }

        public async Task WriteCookieFileAsync(IReadOnlyCollection<Cookie> cookies, string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            //List<Task> tasks = default(List<Task>);
            ////呼び出し元に戻り、タスクの完了を待つ
            //await Task.Run(() =>
            //{
            //    if (!Directory.Exists(directoryPath))
            //        Directory.CreateDirectory(absolutePath);
            //    if (!File.Exists(absolutePath))
            //        File.Create(absolutePath).Close();
            //    Console.WriteLine("CDCCC");
            //    Console.WriteLine("FFFASSSJEDWHIW");
            //}).ContinueWith(async (_) =>
            //{
            //    //非同期ファイル生成処理
            //    //呼び出し元(ContinueWithのデリゲート引数部分)に戻る
            //    //次のContinueWithがあるので、先にそれを実行
            //    //完了したらTaskを即実行
            //    await Task.Run(() =>
            //    {
            //        if (!File.Exists(absolutePath))
            //            File.Create(absolutePath).Close();
            //        Console.WriteLine("CDCCC");
            //    });

            //    //上のTaskを待機したのちに実行
            //    //非同期ファイル生成を待てる処理
            //    Console.WriteLine("AAAAA");
            //}).ContinueWith((_) =>
            //{
            //    //非同期ファイル生成処理を待てない処理(下の処理より優先度高い)←上から下の同期処理にもどるから
            //    Console.WriteLine("FFFFFFF");
            //});

            //非同期ファイル生成処理を待てない処理(上の処理より優先度低い)
            Console.WriteLine("1,2");
            await Task.Run(() =>
            {
                lock (lockObject)
                {
                    using (StreamWriter sw = new StreamWriter(absolutePath))
                    {
                        sw.WriteLine(cookies.ToString());
                        Console.WriteLine("GGGGGGGG");
                    }
                }
            });
        }

        public async Task getIOAsync(string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            //呼び出し元に戻り、タスクの完了を待つ
            await Task.Run(() =>
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(absolutePath);
                if (!File.Exists(absolutePath))
                    File.Create(absolutePath).Close();
                Console.WriteLine("CDCCC");
                Console.WriteLine("FFFASSSJEDWHIW");
            }).ContinueWith(async (_) =>
            {
                //非同期ファイル生成処理
                //呼び出し元(ContinueWithのデリゲート引数部分)に戻る
                //次のContinueWithがあるので、先にそれを実行
                //完了したらTaskを即実行

                await Task.Run(() =>
                {
                    lock (lockObject)
                    {
                        if (!File.Exists(absolutePath))
                            File.Create(absolutePath).Close();
                        Console.WriteLine("CDCCC");
                    }
                });

                //上のTaskを待機したのちに実行
                //非同期ファイル生成を待てる処理
                Console.WriteLine("AAAAA");
            }).ContinueWith((_) =>
            {
                //非同期ファイル生成処理を待てない処理(下の処理より優先度高い)←上から下の同期処理にもどるから
                Console.WriteLine("FFFFFFF");
            });
        }
    }
}
