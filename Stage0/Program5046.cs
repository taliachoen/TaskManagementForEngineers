partial class Program
{
    private static void Main(string[] args)
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

    private static void Welcome8812()
    {

    }

}

