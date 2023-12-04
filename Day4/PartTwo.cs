namespace Day4;

static class PartTwo
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
                                          .ToList())
                           .ToList();

        var scratchcardCounter = new int[rawInput.Count];
        Array.Fill(scratchcardCounter, 1);

        for(var i = 0; i < rawInput.Count; i++)
        {
            var intersectCount = rawInput[i][0].Intersect(rawInput[i][1]).Count();
            for(var j = 1; j <= intersectCount; j++)
            {
                scratchcardCounter[i + j] += scratchcardCounter[i];
            }
        }

        return scratchcardCounter.Sum();
    }
}