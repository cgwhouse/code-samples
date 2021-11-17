using System;

namespace advent2020
{
    public class Day3 : BaseDay
    {
        public Day3() : base() { }

        protected override string SolveFirst()
        {
            int result = 0;

            int currentHorizontalPosition = 0;
            foreach (string line in InputFromFile)
            {
                // Start with checking for tree at current position
                if (line[currentHorizontalPosition] == '#')
                    result++;

                for (int i = 0; i < 3; i++)
                {
                    // Before going to top of for loop, update currentHorizontalPosition by resetting or incrementing
                    if (currentHorizontalPosition == line.Length - 1)
                    {
                        currentHorizontalPosition = 0;
                    }
                    else
                    {
                        currentHorizontalPosition++;
                    }
                }
            }

            return result.ToString();
        }

        protected override string SolveSecond()
        {
            Tuple<int, int>[] slopes = new Tuple<int, int>[]
            {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(3, 1),
                new Tuple<int, int>(5, 1),
                new Tuple<int, int>(7, 1),
                new Tuple<int, int>(1, 2),
            };

            Int64 finalResult = 1;

            foreach (Tuple<int, int> slope in slopes)
            {
                Int64 result = 0;
                int currentHorizontalPosition = 0;
                int currentRow = 0;

                foreach (string line in InputFromFile)
                {
                    // We're on a new row. If down slope is 2, an odd row doesn't count for us
                    if (slope.Item2 == 2 && currentRow % 2 != 0)
                    {
                        currentRow++;
                        continue;
                    }

                    // We've just moved down to the next row. Are we on a tree? If so, does it count?
                    if (line[currentHorizontalPosition] == '#')
                        result++;

                    // Horizontal movement loop
                    for (int i = 0; i < slope.Item1; i++)
                    {
                        // Reset to 0 if we are currently at the end of the line
                        if (currentHorizontalPosition == line.Length - 1)
                            currentHorizontalPosition = 0;
                        else
                            currentHorizontalPosition++;
                    }

                    currentRow++;
                }

                finalResult *= result;
            }

            return finalResult.ToString();
        }
    }
}
