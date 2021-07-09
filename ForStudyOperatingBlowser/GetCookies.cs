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
        private EdgeDriverService service = EdgeDriverService.CreateDefaultService(@"C:\webDriver", "msedgedriver.exe");
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

        public async Task WriteCookieFile(IReadOnlyCollection<Cookie> cookies, string directoryPath, string filePath)
        {
            string absolutePath = directoryPath + @"\" + filePath;
            await Task.Run(() =>
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(absolutePath);
            }).ContinueWith(async (task) =>
            {
                task.Wait();
                //ファイルタスクは待たずに、usingに移行する
                //Createする前にStreamWriterでabsolutePathに触れる場合はWaitで待つか、同期処理に戻すか、lockステートメントでブロック
                await Task.Run(() =>
                {
                    if (!File.Exists(absolutePath))
                        File.Create(absolutePath);
                });
            });
            using(StreamWriter sw = new StreamWriter(absolutePath))
            {
                await Task.Run(() => sw.WriteLine(cookies.ToString()));
            }
        }
    }
}
