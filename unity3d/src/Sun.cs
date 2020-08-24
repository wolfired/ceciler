using System;

namespace wolfired.com.unity3d
{
    public class Hooker
    {
        public static void HookBegin(string msg)
        {
            Console.WriteLine("wolfired.com.unity3d.Hooker.HookBegin @ " + msg);
        }

        public static void HookEnd(string msg)
        {
            Console.WriteLine("wolfired.com.unity3d.Hooker.HookEnd @ " + msg);
        }
    }
    public class Sun
    {
        public string name { get; set; }
        public string toupper(string txt)
        {
            Console.WriteLine("Input string: " + txt);
            Console.WriteLine(this.name);
            return txt.ToUpper();
        }

        public string tolower(string txt)
        {
            Console.WriteLine("Input string: " + txt);
            this.name = "Sun";
            return txt.ToLower();
        }
    }
}
