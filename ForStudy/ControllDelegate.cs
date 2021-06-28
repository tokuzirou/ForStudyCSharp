using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForStudy
{
    //名前付きデリゲート
    delegate void Del(string message);
    class ControllDelegate
    {
        private void consolename(string name)
        {
            Console.WriteLine(name);
        } 

        internal void test()
        {
            Del d = consolename;
            d("Hello");
        }
    }

    //匿名メソッド
    delegate void Del2(string message);
    class ControllDelegate2
    {
        internal void test()
        {
            Del2 d = x =>
            {
                Console.WriteLine(x);
            };
            d("Hello");
        }
    }

    //ラムダ式
    delegate void Del3(string message);
    class ControllDelegate3
    {
        internal void test3()
        {
            Del3 d = x =>
            {
                Console.WriteLine(x);
            };
            d("Hello");
        }
    }

    //Actionデリゲート変数
    class ControllDelegate4
    {
        Action<string> d = x =>
        {
            Console.WriteLine(x);
        };

        internal void test()
        {
            d("Hello");
        }
    }

    //Actionデリゲート引数
    class ControllDelegate5
    {
        internal void test(Action<string> action)
        {
            action("hello");
        }

        internal void test2()
        {
            test(x =>
            {
                Console.WriteLine(x);
            });
        }
    }
}
