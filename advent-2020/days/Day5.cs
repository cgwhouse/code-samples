using System;
using System.Collections.Generic;

namespace advent2020
{
    public class Day5 : BaseDay
    {
        public Day5() : base() { }

        protected override string SolveFirst()
        {
            int result = 0;

            foreach (string boardingPass in InputFromFile)
            {
                int row = determineRow(boardingPass);
                int column = determineColumn(boardingPass);

                int seatID = row * 8 + column;
                if (seatID > result)
                    result = seatID;
            }

            return result.ToString();
        }

        protected override string SolveSecond()
        {
            // We know theoretical total capacity of the plane, so build a collection with all of that filled out
            Dictionary<int, List<int>> planeMap = new Dictionary<int, List<int>>();
            for (int row = 0; row < 128; row++)
            {
                List<int> columns = new List<int>();
                for (int column = 0; column < 8; column++)
                {
                    columns.Add(column);
                }

                planeMap[row] = columns;
            }

            foreach (string boardingPass in InputFromFile)
            {
                int row = determineRow(boardingPass);
                int column = determineColumn(boardingPass);

                // Remove the newly found seat from the planeMap, process of elimination
                planeMap[row].Remove(column);

                // If every seat on this row has been found and removed, also remove the row 
                if (planeMap[row].Count == 0)
                {
                    planeMap.Remove(row);
                }
            }

            // Every row in the plane map should now have all 8 seats in it, or just 1. Find the row with only one seat, that's our seat
            int myRow = 0;
            int myColumn = 0;
            foreach (int row in planeMap.Keys)
            {
                if (planeMap[row].Count == 1)
                {
                    myRow = row;
                    myColumn = planeMap[row][0];
                    break;
                }
            }

            return (myRow * 8 + myColumn).ToString();
        }

        private int determineRow(string boardingPass)
        {
            return determineRowOrColumn(128, boardingPass.Substring(0, 7));
        }

        private int determineColumn(string boardingPass)
        {
            return determineRowOrColumn(8, boardingPass.Substring(7, 3));
        }

        private int determineRowOrColumn(int startingLength, string boardingPassSection)
        {
            int lowerBound = 0;
            int upperBound = startingLength - 1;

            foreach (char direction in boardingPassSection)
            {
                startingLength /= 2;

                switch (direction)
                {
                    case 'F':
                    case 'L':
                        upperBound -= startingLength;
                        break;
                    case 'B':
                    case 'R':
                        lowerBound += startingLength;
                        break;
                    default:
                        throw new Exception("Unexpected direction");
                }
            }

            return lowerBound;
        }
    }
}
