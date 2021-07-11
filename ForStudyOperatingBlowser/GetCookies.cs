using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ForStudyOperatingBlowser
{
    class GetCookies
    {
        private readonly EdgeDriverService service = EdgeDriverService.CreateDefaultService(@"C:\webDriver", "msedgedriver.exe");
        private IReadOnlyCollection<Cookie> cookies = default;

        public IWebDriver Driver { get; }

        public GetCookies()
        {
            this.Driver = new EdgeDriver(service);
        }

        public Task GetAllCookiesAsync()
        {
            Task task = default;
            try
            {
                task = Task.Run(() =>
                {
                    cookies = Driver.Manage().Cookies.AllCookies;
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return task;
        }

        public Task GetIOAsync(string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            //タスクオブジェクトを返す(呼び出し元でawaitさせて、別のスレッドでタスクを起動させる)
            //呼び出し元のスレッドがフリーズしないことに意味がある＝タスク処理が遅いかもだけど、UIがフリーズしない
            Task task = default;
            try
            {
                task = Task.Run(() =>
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
                    //タスクが生成されるたびに、別のスレッドに処理自体は移動している
                    //WriteFile().Wait();
                    //タスクの中にタスクは書けるが非推奨
                    //await Task.Run(() => {処理});
                    //その場合は、今回のように継続タスクとして書く
                    //因みに、上記はメソッドの中にTaskがあるので、あまりよくないので、新しく継続を作成して、呼ぶ
                    Console.WriteLine("継続taskCの終了");
                }).ContinueWith(async (_) =>
                {
                    using (StreamWriter sw = new StreamWriter(absolutePath))
                    {
                        await sw.WriteLineAsync(cookies.ToString());
                        Console.WriteLine("GGGGGGGG");
                    }
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return task;
        }
    }
}
