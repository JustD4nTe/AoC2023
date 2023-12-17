namespace Day14;

static class PartOne
{
    static readonly string test1 = "../../../Day14/test1.txt";
    static readonly string input = "../../../Day14/input.txt";

    public static int Solve()
    {
        var platform = File.ReadAllLines(input)
                           .Select(x => x.Select(ParseTile)
                                         .ToArray())
                           .ToArray();

        TiltToNorth(platform);
        return CalculateLoadOfNorthBeam(platform);
    }

    private static Tile ParseTile(char symbol)
        => symbol switch
        {
            'O' => new RoundedRock(),
            '#' => new CubeShapedRock(),
            '.' => new EmptySpace(),
            _ => throw new Exception("Symbol does not exists")
        };

    private static void TiltToNorth(Tile[][] platform)
    {
        bool isAnyRockMoved;
        do
        {
            isAnyRockMoved = false;

            for (var y = 1; y < platform.Length; y++)
            {
                for (var x = 0; x < platform[y].Length; x++)
                {
                    if (CanRockMove(x, y, platform))
                    {
                        platform[y - 1][x] = platform[y][x];
                        platform[y][x] = new EmptySpace();
                        isAnyRockMoved = true;
                    }
                }
            }
        } while (isAnyRockMoved);
    }

    private static bool CanRockMove(int currX, int currY, Tile[][] platform)
        => currY >= 1
        && platform[currY][currX] is RoundedRock
        && platform[currY - 1][currX] is EmptySpace;

    private static int CalculateLoadOfNorthBeam(Tile[][] platform)
    {
        var rowLoadCounter = platform.Length;
        var totalLoad = 0;
        
        for(var y = 0; y < platform.Length; y++, rowLoadCounter--)
            totalLoad += platform[y].Count(x => x is RoundedRock) * rowLoadCounter;

        return totalLoad;
    }

    private class Tile
    {
        public readonly char Symbol;
        public Tile(char symbol)
        {
            Symbol = symbol;
        }
    }
    private class EmptySpace : Tile
    {
        public EmptySpace() : base('.') { }
    }
    private class RoundedRock : Tile
    {
        public RoundedRock() : base('O') { }
    }
    private class CubeShapedRock : Tile
    {
        public CubeShapedRock() : base('O') { }
    }
}