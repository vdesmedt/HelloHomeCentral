using HelloHome.Central.Repository;
using System;
using System.Linq;

namespace HelloHome.Central.Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using(var ctx = new HelloHomeContext())
            {
                var nodes = ctx.Nodes;
                Console.WriteLine($"Node count:{nodes.ToList().Count}");
            }
        }
    }
}
