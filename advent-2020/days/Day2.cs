namespace advent2020
{
    public class Day2 : BaseDay
    {
        public Day2() : base() { }

        protected override string SolveFirst()
        {
            int result = 0;

            foreach (string line in InputFromFile)
            {
                // Get the password to test
                string password = line.Split(':')[1].Trim();

                // Get this line's password policy and divide into usable pieces
                string rule = line.Split(':')[0].Trim();
                int lowerBound = int.Parse(rule.Split(' ')[0].Split('-')[0]);
                int upperBound = int.Parse(rule.Split(' ')[0].Split('-')[1]);
                char targetCharacter = char.Parse(rule.Split(' ')[1]);

                // Go through and count the instances of the target character
                int charCount = 0;
                foreach (char c in password)
                    if (c == targetCharacter)
                        charCount++;

                // Add to the count of valid passwords
                if (charCount >= lowerBound && charCount <= upperBound)
                    result++;
            }

            return result.ToString();
        }

        protected override string SolveSecond()
        {
            int result = 0;

            foreach (string line in InputFromFile)
            {
                // Get the password to test
                string password = line.Split(':')[1].Trim();

                // Get this line's password policy and divide into usable pieces
                string rule = line.Split(':')[0].Trim();
                int lowerBound = int.Parse(rule.Split(' ')[0].Split('-')[0]) - 1;
                int upperBound = int.Parse(rule.Split(' ')[0].Split('-')[1]) - 1;
                char targetCharacter = char.Parse(rule.Split(' ')[1]);

                int charCount = 0;

                if (password[lowerBound] == targetCharacter)
                    charCount++;

                if (password[upperBound] == targetCharacter)
                    charCount++;

                if (charCount == 1)
                    result++;
            }

            return result.ToString();
        }
    }
}
