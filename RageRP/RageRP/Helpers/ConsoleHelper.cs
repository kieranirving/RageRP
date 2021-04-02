using System;

namespace RageRP.Server.Helpers
{
    public static class c
    {
        public static void Write(string text)
        {
            Console.Write($"[RageRP] {text}");
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine($"[RageRP] {text}");
        }

        public static void WriteLines(int line = 1, bool bottom = false)
        {
            for(int i = 0; i < line; i++)
            {
                Console.WriteLine("");
            }
            if(bottom) Console.WriteLine("____________________________________________________");
        }
    }
}
