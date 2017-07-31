using System;

namespace Tests.ZeroMq.CommandQuery
{
    public class BaseClass
    {
        public void ShowOnConsole()
        {
            Console.WriteLine($"\n {ToString()} \n");
        }
    }
}