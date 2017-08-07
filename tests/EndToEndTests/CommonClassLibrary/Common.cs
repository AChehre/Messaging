using System;

namespace CommonClassLibrary
{
    public static class Common
    {
        public static void Show(string message)
        {
            var dateTime = DateTime.Now;
            Console.WriteLine($"[{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{dateTime.Millisecond}]   {message}");
        }

        public static void ScreenEnd()
        {
            var dashes = new string('-', 33);

            Console.WriteLine(dashes);
            Console.WriteLine($"{new string(' ', 15)}END{new string(' ', 15)}");
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