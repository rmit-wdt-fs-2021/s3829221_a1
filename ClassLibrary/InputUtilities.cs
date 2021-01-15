using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class InputUtilities
    {

        // Mask user input
        public static string EnterPwd()
        {
            var pwd = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (!char.IsControl(keyInfo.KeyChar))
                    pwd += keyInfo.KeyChar;
            }
            while (key != ConsoleKey.Enter);

            return pwd;
        }


        // Check if a value is within the specified range
        public static bool IsInRange(this int value, int min, int max) 
            => value >= min && value <= max;


        // Enter menu option - exception handling with defensive programming
        public static int EnterOption(int min, int max)
        {
            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out int option) || !option.IsInRange(min, max))
                {
                    Console.WriteLine("Invalid input, please enter again.");
                    Console.WriteLine();
                    continue;
                }
                else
                    return option;
            }
        }


        public static decimal EnterPositiveNum()
        {
            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine();

                if (!decimal.TryParse(input, out decimal num) || num <= 0)
                {
                    Console.WriteLine("Invalid input, please enter again.");
                    Console.WriteLine();
                    continue;
                }
                else
                    return num;
            }
        }
    }
}
