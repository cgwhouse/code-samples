using System;
using System.Collections.Generic;

namespace advent2020
{
    public class Day4 : BaseDay
    {
        public Day4() : base() { }

        protected override string SolveFirst()
        {
            int result = 0;

            // Keep passports here
            List<Dictionary<string, string>> passportList = new List<Dictionary<string, string>>();

            // Current passport handle
            Dictionary<string, string> currentPassport = new Dictionary<string, string>();

            string[] REQ_FIELDS = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            // This ignores the last one
            foreach (string line in InputFromFile)
            {
                // If the current line is a blank one, we must be done getting passport info, so add curr to collection and reset 
                if (string.IsNullOrEmpty(line))
                {
                    passportList.Add(currentPassport);
                    currentPassport = new Dictionary<string, string>();
                    continue;
                }

                // If we're here, we have stuff to add about currentPassport, so parse the line
                string[] parsedLine = line.Split(' ');
                foreach (string KeyValPair in parsedLine)
                {
                    string[] splitItem = KeyValPair.Split(':');
                    currentPassport[splitItem[0]] = splitItem[1];
                }
            }

            // Add the last passport you sloppy boy (C# doesn't add the last blank line to the array of lines)
            passportList.Add(currentPassport);

            foreach (Dictionary<string, string> passport in passportList)
            {
                bool valid = true;
                // Check for all required fields
                foreach (string field in REQ_FIELDS)
                {
                    if (!passport.ContainsKey(field))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    result++;
            }

            return result.ToString();
        }

        protected override string SolveSecond()
        {
            int result = 0;

            // Keep passports here
            List<Dictionary<string, string>> passportList = new List<Dictionary<string, string>>();

            // Current passport handle
            Dictionary<string, string> currentPassport = new Dictionary<string, string>();

            string[] REQ_FIELDS = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            // This ignores the last one
            foreach (string line in InputFromFile)
            {
                // If the current line is a blank one, we must be done getting passport info, so add curr to collection and reset 
                if (string.IsNullOrEmpty(line))
                {
                    passportList.Add(currentPassport);
                    currentPassport = new Dictionary<string, string>();
                    continue;
                }

                // If we're here, we have stuff to add about currentPassport, so parse the line
                string[] parsedLine = line.Split(' ');
                foreach (string KeyValPair in parsedLine)
                {
                    string[] splitItem = KeyValPair.Split(':');
                    currentPassport[splitItem[0]] = splitItem[1];
                }
            }

            // Add the last passport you sloppy boy (C# doesn't add the last blank line to the array of lines)
            passportList.Add(currentPassport);
            List<Dictionary<string, string>> fieldsPresent = new List<Dictionary<string, string>>();
            foreach (Dictionary<string, string> passport in passportList)
            {
                bool requiredFieldsPresent = true;

                // Check for all required fields
                foreach (string field in REQ_FIELDS)
                {
                    if (!passport.ContainsKey(field))
                    {
                        requiredFieldsPresent = false;
                        break;
                    }
                }

                if (requiredFieldsPresent)
                {
                    fieldsPresent.Add(passport);
                }
            }

            foreach (Dictionary<string, string> passport in fieldsPresent)
            {
                // Validate each field
                bool valuesAreValid = true;
                foreach (string key in passport.Keys)
                {
                    string value = passport[key];

                    try
                    {
                        switch (key)
                        {
                            case "byr":
                                if (value.Length != 4 || int.Parse(value) < 1920 || int.Parse(value) > 2002)
                                    valuesAreValid = false;
                                break;
                            case "iyr":
                                if (value.Length != 4 || int.Parse(value) < 2010 || int.Parse(value) > 2020)
                                    valuesAreValid = false;
                                break;
                            case "eyr":
                                if (value.Length != 4 || int.Parse(value) < 2020 || int.Parse(value) > 2030)
                                    valuesAreValid = false;
                                break;
                            case "hgt":
                                // Validate that the end is cm or in
                                string last2 = value.Substring(value.Length - 2);
                                if (last2 != "cm" && last2 != "in")
                                {
                                    valuesAreValid = false;
                                    break;
                                }

                                // Get the number off the front and do the value checks
                                int firstNum = int.Parse(value.Replace(last2, ""));
                                if (last2 == "cm" && (firstNum < 150 || firstNum > 193))
                                {
                                    valuesAreValid = false;
                                    break;
                                }
                                else if (last2 == "in" && (firstNum < 59 || firstNum > 76))
                                {
                                    valuesAreValid = false;
                                    break;
                                }
                                break;
                            case "hcl":
                                // Ensure starts with hashtag and has 6 characters to follow
                                if (value.Length != 7 || value[0] != '#')
                                {
                                    valuesAreValid = false;
                                    break;
                                }

                                foreach (char c in value.Substring(1))
                                {
                                    if (!"0123456789".Contains(c) && !"abcdef".Contains(c))
                                    {
                                        valuesAreValid = false;
                                        break;
                                    }
                                }
                                break;
                            case "ecl":
                                List<string> valid = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                                if (!valid.Contains(value))
                                    valuesAreValid = false;
                                break;
                            case "pid":
                                if (value.Length != 9)
                                {
                                    valuesAreValid = false;
                                    break;
                                }

                                foreach (char c in value)
                                {
                                    if (!"0123456789".Contains(c))
                                    {
                                        valuesAreValid = false;
                                        break;
                                    }
                                }
                                break;
                            case "cid": // optional
                                break;
                            default:
                                throw new Exception("Unexpected key");
                        }
                    }
                    catch (Exception) { valuesAreValid = false; }

                    if (!valuesAreValid)
                        break;
                }

                if (valuesAreValid)
                    result++;
            }

            return result.ToString();
        }
    }
}
