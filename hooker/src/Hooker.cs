using System;
using System.IO;

using UnityEngine;

namespace wolfired.com.hooker
{
    public class Hooker
    {
        public static void HookBegin(string msg)
        {
            Debug.Log("wolfired.com.hooker.Hooker.HookBegin @ " + msg);
        }

        public static void HookEnd(string msg)
        {
            Debug.Log("wolfired.com.hooker.Hooker.HookEnd @ " + msg);
        }
    }
}
