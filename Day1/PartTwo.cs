using System.Text.RegularExpressions;

namespace Day1;

static class PartTwo
{
    static readonly string test2 = "../../../Day1/test2.txt";
    static readonly string input = "../../../Day1/input.txt";
    
    public static long Solve()
    {
        List<(string, string)> spelledOdLetters =
        [
            ("one", "o1e"),
            ("two", "t2o"),
            ("three", "t3e"),
            ("four", "f4r"),
            ("five", "f5e"),
            ("six", "s6x"),
            ("seven", "s7n"),
            ("eight", "e8t"),
            ("nine", "n9e"),
        ];

        var rawInput = File.ReadAllLines(input);
        var sum = 0;
        foreach (var line in rawInput)
        {
            string convertedLine = line;

            foreach (var spelledOdLetter in spelledOdLetters)
            {
                convertedLine = convertedLine.Replace(spelledOdLetter.Item1, spelledOdLetter.Item2);
            }
            
            convertedLine = Regex.Replace(convertedLine, "[a-zA-Z]", "");
            sum += int.Parse(convertedLine[0].ToString() + convertedLine[^1]);
        }

        return sum;
    }
}