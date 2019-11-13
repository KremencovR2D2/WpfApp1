using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите начальное значение интервала:");
            var a = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Введите конечное значение интервала:");
            var b = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Случайно сгенерированный массив чисел:");
            int[] randomArray = new int[20];
            List<int> itemsInRange = new List<int>();
            List<int> itemsOutOfRange = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < randomArray.Length; i++)
            {
                randomArray[i] = rand.Next(0, 100);
                Console.Write(randomArray[i] + " ");
            }
            Console.WriteLine();
            foreach (var item in randomArray)
            {
                if (item > a && item < b)
                {
                    itemsInRange.Add(item);
                }
                else
                {
                    itemsOutOfRange.Add(item);
                }
            }
            var inRangeCount = itemsInRange.Count();
            var outOfRangeCount = itemsOutOfRange.Count();
            itemsInRange.AddRange(itemsOutOfRange);
            Console.WriteLine();
            Console.WriteLine("Количество элементов в интервале - " + inRangeCount + ", количество элементов вне интервала - " + outOfRangeCount + ":");
            itemsInRange.ForEach(x => Console.Write(x + " "));
            Console.ReadLine();
        }
    }
}
