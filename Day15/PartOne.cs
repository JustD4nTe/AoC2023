namespace Day15;

static class PartOne
{
    static readonly string test1 = "../../../Day15/test1.txt";
    static readonly string input = "../../../Day15/input.txt";

    public static int Solve()
    {
        return File.ReadAllLines(input)[0]
                   .Split(",")
                   .Sum(x => HashAlgorithm(x.ToCharArray()));
    }

    private static int HashAlgorithm(char[] str)
    {
        var currValue = 0;

        for (var i = 0; i < str.Length; i++)
        {
            currValue += str[i];
            currValue *= 17;
            currValue %= 256;
        }

        return currValue;
    }
}