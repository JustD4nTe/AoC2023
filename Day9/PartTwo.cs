namespace Day9;

static class PartTwo
{
    static readonly string test1 = "../../../Day9/test1.txt";
    static readonly string input = "../../../Day9/input.txt";
    public static long Solve()
    {
        var rawInput = File.ReadAllLines(input)
                           .Select(x => x.Split(" ")
                                         .Select(int.Parse))
                           .ToArray();

        var triangle = new List<List<int>>();
        var sumOfNextHistoryValue = 0;

        foreach (var line in rawInput)
        {
            triangle.Add([.. line]);

            CreateTriangle(triangle);
            FillHistoricLeftSideOfTriangle(triangle);

            sumOfNextHistoryValue += triangle[0][0];
            triangle.Clear();
        }

        return sumOfNextHistoryValue;
    }

    private static void CreateTriangle(List<List<int>> triangle)
    {
        var i = 0;
        do
        {
            var newLine = new List<int>();

            for (var j = 0; j < triangle[i].Count - 1; j++)
            {
                newLine.Add(triangle[i][j + 1] - triangle[i][j]);
            }

            triangle.Add(newLine);
            i++;
        } while (!triangle[^1].All(x => x == 0));
    }

    private static void FillHistoricLeftSideOfTriangle(List<List<int>> triangle)
    {
        triangle[^1].Insert(0, 0);

        for (var i = triangle.Count - 2; i >= 0; i--)
        {
            triangle[i].Insert(0, triangle[i][0] - triangle[i + 1][0]);
        }
    }
}