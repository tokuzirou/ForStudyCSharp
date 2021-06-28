using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ForStudy
{
    class ControllScraping
    {
        internal static void Test()
        {
            //EdgeDriverOptionsの設定
            //なぜかEdgeOptionsからはバイナリ設定もドライバパスの設定もできない
            //EdgeOptions options = new EdgeOptions();

            //EdgeDriverServiceの設定
            EdgeDriverService service = EdgeDriverService.CreateDefaultService(@"C:\webDriver", "msedgedriver.exe");
            //serviceのコマンドプロンプトウインドウの非表示
            //service.HideCommandPromptWindow = true;
            //serviceの初期診断情報の抑制
            //service.SuppressInitialDiagnosticInformation = true;
            //serviceの詳細ログの表示
            //service.UseVerboseLogging = true;

            //EdgeDriverを起動
            IWebDriver webDriver = new EdgeDriver(service);

            //URL遷移
            webDriver.Navigate().GoToUrl(@"https://google.com");

            //検索ボックス
            IWebElement serachBox = webDriver.FindElement(By.CssSelector("input[title=検索]"));

            //検索文字列入力
            serachBox.SendKeys("github");

            //検索
            serachBox.Submit();

            //戻る
            webDriver.Navigate().Back();

            //進む
            webDriver.Navigate().Forward();

            //更新
            webDriver.Navigate().Refresh();

            //URL取得
            Console.WriteLine(webDriver.Url);

            //タイトル取得
            Console.WriteLine(webDriver.Title);

            //現在のウインドウを参照
            string currentWindow = webDriver.CurrentWindowHandle;
            Console.WriteLine(currentWindow);

            //現在操作中のウインドウ(または、タブ)の数
            Console.WriteLine(webDriver.WindowHandles.Count);

            //タブを増やす(使えない2021/6/15)
            //webDriver.SwitchTo().NewWindow(WindowType.Tab);

            //一番最初に開いたURLのウインドウ(または、タブ)のハンドルを取得
            webDriver.SwitchTo().Window(currentWindow);

            //Windowサイズを取得
            int width = webDriver.Manage().Window.Size.Width;
            int height = webDriver.Manage().Window.Size.Height;
            Console.WriteLine("{0} {1}", width, height);

            //Windowサイズを変更
            webDriver.Manage().Window.Size = new System.Drawing.Size(500, 500);

            //Windowの座標を取得
            int X = webDriver.Manage().Window.Position.X;
            int Y = webDriver.Manage().Window.Position.Y;
            Console.WriteLine("{0} {1}", X, Y);

            //Windowの座標を変更
            webDriver.Manage().Window.Position = new Point(0, 0);

            //Windowの最大化
            webDriver.Manage().Window.Maximize();

            //Windowの最小化
            webDriver.Manage().Window.Minimize();

            //全画面モード
            webDriver.Manage().Window.FullScreen();

            //スクリーンショット
            //IWebElementインターフェースにも使える
            Screenshot screenshot = (webDriver as ITakesScreenshot).GetScreenshot();
            //セーブ
            screenshot.SaveAsFile(@"C:\Users\user\OneDrive\画像\スクリーンショット\test.png", ScreenshotImageFormat.Png);

            //タブを終了する
            webDriver.Close();

            //ブラウザーを閉じる
            webDriver.Quit();
        }
    }
}
