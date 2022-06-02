using System;

namespace Amalgamate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine();
            Amalgamation a = new();
            a.Generate();
            return;
        }
    }
}
