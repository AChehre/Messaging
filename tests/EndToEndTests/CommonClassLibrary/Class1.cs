using System;

namespace CommonClassLibrary
{
    public static class Common
    {
        public static void Show(string message)
        {
            Console.WriteLine(message);
        }


        public static void ScreenEnd()
        {
            ScreenTop("Th End ------------------------!");
        }

        public static void ScreenTopClient()
        {
            ScreenTop("Client");
        }

        public static void ScreenTopServer()
        {
            ScreenTop("Server");
        }

        public static void ScreenTop(string title)
        {
            var dashes = new string('-', title.Length + 20);

            Console.WriteLine(dashes);
            Console.WriteLine($"|{new string(' ', 9)}{title}{new string(' ', 9)}|");
            Console.WriteLine(dashes);
        }
    }
}