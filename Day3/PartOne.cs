using System.Data;

namespace Day3;

static class PartOne
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
            string number = "";
            for (int x = 0; x < rawInput[0].Length; x++)
            {
                if (!char.IsDigit(rawInput[y][x]))
                {
                    number = "";
                    continue;
                }

                number += rawInput[y][x];
                if (IsAdjacentToSymbol(rawInput, y, x))
                {
                    while (x + 1 < rawInput[y].Length && char.IsDigit(rawInput[y][x + 1]))
                        number += rawInput[y][++x];

                    finalSum += int.Parse(number);
                    number = "";
                }
            }
        }

        return finalSum;
    }

    private static bool IsAdjacentToSymbol(char[][] rawInput, int y, int x)
    {
        foreach (var coord in coordMartix)
        {
            var yChange = y + coord.Item2;
            var xChange = x + coord.Item1;

            if (yChange < 0 || yChange >= rawInput.Length)
                continue;
            if (xChange < 0 || xChange >= rawInput[yChange].Length)
                continue;

            var buff = rawInput[yChange][xChange];

            if (buff == '.' || char.IsDigit(buff))
                continue;

            return true;
        }

        return false;
    }
}