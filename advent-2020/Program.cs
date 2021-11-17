using System;
using System.Collections.Generic;

namespace advent2020
{
    public class Program
    {
        public static int SELECTED_DAY;

        static void Main(string[] args)
        {
            const string DAY_NOT_IMPLEMENTED = "Day Not Implemented";
            const string INVALID_RESULT_ARGUMENT = "Invalid Result Argument";

            HashSet<int> IMPLEMENTED_DAYS = new HashSet<int> { 1, 2, 3, 4, 5, 6 };

            // Get selected day from program args
            SELECTED_DAY = int.Parse(args[0]);

            // Throw if we don't have a class matching the day that was provided, because everything will bomb out if so
            if (!IMPLEMENTED_DAYS.Contains(SELECTED_DAY))
                throw new Exception(DAY_NOT_IMPLEMENTED);

            // Determine which Day class we want to instantiate
            Type dayType = Type.GetType($"advent2020.Day{SELECTED_DAY}");

            // Instantiate the Day class given the info above
            BaseDay dayInstance = (BaseDay)Activator.CreateInstance(dayType);

            // Get selected result (i.e. part 1 or part 2) from args
            int selectedResult = int.Parse(args[1]);

            // Determine which result to print
            string result;
            switch (selectedResult)
            {
                case 1:
                    result = dayInstance.FirstResult;
                    break;
                case 2:
                    result = dayInstance.SecondResult;
                    break;
                default:
                    throw new Exception(INVALID_RESULT_ARGUMENT);
            }

            Console.WriteLine($"Result: {result}");
        }
    }
}
