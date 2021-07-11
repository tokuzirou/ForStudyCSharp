using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForStudy
{
    internal class ControllString
    {
        private static int? o = default;
        private static string str = "Hello";
        private static string str1 = default;
        private static bool judge = default;
        //staticクラスなのでNullable型の変数を宣言できない
        //private Nullable nullable = default;

        internal static bool Controll()
        {
            str1 = str;
            str = "HELLO";
            judge = (Object)str == (Object)str1;
            return judge;
        }

        internal static bool Controll1()
        {
            //この場合はstring型のdefault値=なにもない状態が出力
            //Console.WriteLine(str1);
            //Object.GetType()がNullableでないので、例外
            //Console.WriteLine(str1.GetType());
            //Console.WriteLine(Nullable.GetUnderlyingType(str1.GetType()));
            Console.WriteLine(o.GetType());
            return Object.ReferenceEquals(str, str1);
        }
    }
}
