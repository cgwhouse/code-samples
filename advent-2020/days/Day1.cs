using System.Collections.Generic;

namespace advent2020
{
    public class Day1 : BaseDay
    {
        public Day1() : base() { }

        protected override string SolveFirst()
        {
            string result = "0";

            HashSet<string> numberSet = new HashSet<string>(InputFromFile);

            foreach (string n in numberSet)
            {
                int first = int.Parse(n);
                int second = 2020 - first;

                if (numberSet.Contains(second.ToString()))
                {
                    result = (second * first).ToString();
                    break;
                }
            }

            return result;
        }

        protected override string SolveSecond()
        {
            string result = "0";

            HashSet<string> numberSet = new HashSet<string>(InputFromFile);

            foreach (string first in numberSet)
            {
                int firstDifference = 2020 - int.Parse(first);

                foreach (string second in numberSet)
                {
                    int secondDifference = firstDifference - int.Parse(second);

                    if (numberSet.Contains(secondDifference.ToString()))
                    {
                        result = (int.Parse(first) * int.Parse(second) * secondDifference).ToString();
                        break;
                    }
                }
            }

            return result;
        }
    }
}
