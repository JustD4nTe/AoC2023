namespace Day4;

static class PartOne
{
    static readonly string test1 = "../../../Day4/test1.txt";
    static readonly string input = "../../../Day4/input.txt";
    public static int Solve()
    {
        var rawInput = File.ReadAllLines(input)
                           .Select(x => x.Split(":")[1]
                                         .Split("|")
                                         .Select(y => y.Trim()
                                                       .Replace("  ", " ")
                                                       .Split(" ")
                                                       .Select(z => int.Parse(z)))
                                          .ToList());

        var result = 0;

        foreach (var line in rawInput)
        {
            var intersectCount = line[0].Intersect(line[1]).Count();
            result += (int)Math.Pow(2, intersectCount - 1);
        }

        return result;
    }
}