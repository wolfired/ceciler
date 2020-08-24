using System;
using System.Reflection;
using wolfired.com.injecter;

namespace wolfired.com
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("inject begin\n\n");

            Injecter.Inject(args);

            Console.WriteLine("\n\ninject end");
        }
    }
}
