using System;
using System.IO;
using System.Linq;
using System.Reflection;
using wolfired.com.unity3d;

namespace wolfired.com.unity3p
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("unity3p!!\n");

            Sun sun = new Sun();
            Console.WriteLine(sun.tolower("ABCDE"));
            Console.WriteLine(sun.toupper("abcde"));
        }
    }
}
