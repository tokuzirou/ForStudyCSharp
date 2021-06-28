using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForStudy
{
    class ControllAsync
    {
        //Parallelを使ったマルチスレッド処理
        internal void MultiThread()
        {
            const int N = 3;

            //id: 仕事数
            //N: 使いまわす回数
            //スレッドプールを使いまわす処理
            Parallel.For(0, N, id =>
            {
                Random rnd = new Random();

                for (int i = 0; i < 4; i++)
                {
                    //スレッドプールを作る処理
                    Thread.Sleep(rnd.Next(50, 100));

                    //並行処理したい箇所
                    Console.WriteLine("{0} (ID:{1})", i, id);
                }
            });
        }



        internal void ExecuteAsyncWork()
        {
            Task<string> t = Task.Run(() => { return AsyncWork(); });
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine("処理A実行:" + i);
            }
            Console.WriteLine(t.Result);
            Console.ReadLine();
        }

        private string AsyncWork()
        {
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine("処理B実行:" + i);
            }
            return "完了";
        }
    }
}

