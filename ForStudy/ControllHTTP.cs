using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace ForStudy
{
    class ControllHTTP
    {
        internal static void Download()
        {
            WebClient webClient = new WebClient();
            string downloadDirectory = @"C:\downloads";
            Directory.CreateDirectory(downloadDirectory);
            Directory.SetCurrentDirectory(downloadDirectory);
            string downloadURL = @"https://google.co.jp";
            string downloadOption = "google.html";
            webClient.DownloadFile(downloadURL, downloadOption);
        }
    }
}
