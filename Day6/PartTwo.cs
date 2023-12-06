using System.Text.RegularExpressions;

namespace Day6;

static class PartTwo
{
    static readonly string test1 = "../../../Day6/test1.txt";
    static readonly string input = "../../../Day6/input.txt";
    public static ulong Solve()
    { 
        var rawInput = File.ReadAllLines(input);

        var raceTime = long.Parse(Regex.Replace(rawInput[0][6..], "\\s+\\s", ""));
        var raceDistance = long.Parse(Regex.Replace(rawInput[1][10..], "\\s+\\s", ""));

        ulong winWayCount = 0;

        for(long speed = 0; speed < raceTime; speed++)
        {
            var distance = (raceTime - speed) * speed;

            if(distance > raceDistance)
                winWayCount++;
        }

        return winWayCount;
    }
}