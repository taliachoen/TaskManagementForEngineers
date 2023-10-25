
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage0 {

    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5046();
            //Console.Readkey();
            Welcome8812();
        }

        static partial void Welcome8812();

        private static void Welcome5046()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application ", userName);
        }

    }
}


