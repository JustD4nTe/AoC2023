using System.Data;
using System.Text;

namespace Day3;

static class PartTwo
{
    static readonly string test1 = "../../../Day3/test1.txt";
    static readonly string input = "../../../Day3/input.txt";

    static readonly List<(int, int)> coordMartix =
    [
      (-1,-1),
      (0,-1),
      (1,-1),
      (-1,0),
      (0,0),
      (1,0),
      (-1,1),
      (0,1),
      (1,1),
    ];

    public static int Solve()
    {
        var rawInput = File.ReadAllText(input)
                           .Split("\n")
                           .Select(x => x.ToCharArray())
                           .ToArray();
        int finalSum = 0;

        for (int y = 0; y < rawInput.Length - 1; y++)
        {
            for (int x = 0; x < rawInput[0].Length; x++)
            {
                if (rawInput[y][x] != '*')
                    continue;

                finalSum += CalculateGearRatio(rawInput, y, x);
            }
        }

        return finalSum;
    }

    private static int GetNumber(char[][] rawInput, int y, int x)
    {
        StringBuilder number = new();
        number.Append(rawInput[y][x]);

        var postX = x;
        var preX = x;

        while (++postX < rawInput[y].Length && char.IsDigit(rawInput[y][postX]))
            number.Append(rawInput[y][postX]);

        while (--preX >= 0 && char.IsDigit(rawInput[y][preX]))
            number.Insert(0, rawInput[y][preX]);

        return int.Parse(number.ToString());
    }

    private static int CalculateGearRatio(char[][] rawInput, int y, int x)
    {
        HashSet<int> gears = [];
        foreach (var coord in coordMartix)
        {
            var yChange = y + coord.Item2;
            var xChange = x + coord.Item1;

            if (yChange < 0 || yChange >= rawInput.Length)
                continue;
            if (xChange < 0 || xChange >= rawInput[yChange].Length)
                continue;

            var buff = rawInput[yChange][xChange];

            if (char.IsDigit(buff))
                gears.Add(GetNumber(rawInput, yChange, xChange));
        }
        
        if(gears.Count == 2)
            return gears.Aggregate((a, x) => a * x);
        
        return 0;
    }
}