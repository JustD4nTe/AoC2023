using System.Text.RegularExpressions;

namespace Day1;

static class PartOne
{
    static string test1 = "../../../Day1/test1.txt";
    static string input = "../../../Day1/input.txt";
    public static long Solve()
    {
        var rawInput = File.ReadAllLines(input);
        var sum = 0;
        foreach (var line in rawInput)
        {
            var strNumber = Regex.Replace(line, "[a-zA-Z]", "");
            sum += int.Parse(strNumber[0].ToString() + strNumber[^1]);
        }

        return sum;
    }
}