using System.IO;

namespace advent2020
{
    public abstract class BaseDay
    {
        protected string[] InputFromFile;
        public string FirstResult => SolveFirst();
        public string SecondResult => SolveSecond();

        public BaseDay()
        {
            InputFromFile = File.ReadAllLines($"inputs/day{Program.SELECTED_DAY}.txt");
        }

        protected abstract string SolveFirst();
        protected abstract string SolveSecond();
    }
}
