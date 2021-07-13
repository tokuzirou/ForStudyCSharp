using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA;
using OpenQA.Selenium;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace ForStudyOperatingBlowser
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            GetCookies getCookies = new GetCookies();

            Command commandCookies = new Command("cookie", "to get all cookie.")
            {
                Handler = CommandHandler.Create(async () =>
                {
                    await getCookies.GetAllCookiesAsync();
                    string directoryPath = @"C:\Users\user\cookies_edge";
                    string filePath = "cookies.txt";
                    await getCookies.GetIOAsync(directoryPath, filePath);
                })
            };

            RootCommand rootCommand = new RootCommand("to get all cookie.")
            {
                Handler = CommandHandler.Create(async () => {
                    await getCookies.GetAllCookiesAsync();
                    string directoryPath = @"C:\Users\user\cookies_edge";
                    string filePath = "cookies.txt";
                    await getCookies.GetIOAsync(directoryPath, filePath);
                })
            };
            rootCommand.Add(commandCookies);
            //ルートコマンド入力(アプリ名入力)時はヘルプがデフォルト表示

            return rootCommand.InvokeAsync(args);
        }
    }
}
