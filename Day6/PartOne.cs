using System.Text.RegularExpressions;

namespace Day6;

static class PartOne
{
    static readonly string test1 = "../../../Day6/test1.txt";
    static readonly string input = "../../../Day6/input.txt";
    public static long Solve()
    { 
        var rawInput = File.ReadAllLines(input);

        var times = Regex.Replace(rawInput[0][6..], "\\s+\\s", " ")
                                    .Trim()
                                    .Split(" ")
                                    .Select(int.Parse)
                                    .ToArray();
        var distances = Regex.Replace(rawInput[1][10..], "\\s+\\s", " ")
                                        .Trim()
                                        .Split(" ")
                                        .Select(int.Parse)
                                        .ToArray();

        long result = 1;

        for(var i = 0; i < times.Length; i++)
        {
            var winWayCount = 0;

            for(var speed = 0; speed < times[i]; speed++)
            {
                var distance = (times[i] - speed) * speed;

                if(distance > distances[i])
                    winWayCount++;
            }

            result *= winWayCount;
        }

        return result;
    }
}