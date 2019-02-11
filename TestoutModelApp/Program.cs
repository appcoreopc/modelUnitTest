using System;
using UnitTestGenerator.Reflection;

namespace TestoutModelApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var model = new ModelReflection();
            model.GenerateTest(@"E:\C3PostTrade.Automation.SolaceContainerApi\src\C3PostTrade.Automation.SolaceContainerApi.Interfaces\bin\Debug\netcoreapp2.1\C3PostTrade.Automation.SolaceContainerApi.Interfaces.dll", 
                "test");
        }
    }
}
