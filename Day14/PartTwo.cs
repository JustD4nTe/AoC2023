
namespace Day14;

static class PartTwo
{
    static readonly string test1 = "../../../Day14/test1.txt";
    static readonly string input = "../../../Day14/input.txt";

    public static int Solve()
    {
        var platform = File.ReadAllLines(input)
                           .Select(x => x.Select(ParseTile)
                                         .ToArray())
                           .ToArray();
        var buff = new List<int>();
        for (var cycle = 0; cycle < 200; cycle++)
        {
            TiltToNorth(platform);
            TiltToWest(platform);
            TiltToSouth(platform);
            TiltToEast(platform);
            var temp = CalculateLoadOfNorthBeam(platform);
            if(!buff.Contains(temp))
                Console.WriteLine(cycle);
            buff.Add(temp);
        }
        Console.WriteLine(string.Join(", ", buff)); // 0 - 121 uniqe + 18 in cycle
        // var foo = buff.Values.Where(x => x.Count > 1).Select(x => x.Count);
        return CalculateLoadOfNorthBeam(platform);
    }

    private static void PrintPlatform(Tile?[][] platform)
    {
        for (var y = 0; y < platform.Length; y++)
        {
            for (var x = 0; x < platform[y].Length; x++)
            {
                if (platform[y][x] is null)
                    Console.Write('.');
                else if (platform[y][x] is RoundedRock)
                    Console.Write('O');
                else if (platform[y][x] is CubeShapedRock)
                    Console.Write('#');
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static Tile? ParseTile(char symbol)
        => symbol switch
        {
            'O' => new RoundedRock(),
            '#' => new CubeShapedRock(),
            _ => null
        };

    private static void TiltToNorth(Tile?[][] platform)
    {
        for (var x = 0; x < platform[0].Length; x++)
        {
            for (var y = 1; y < platform.Length; y++)
            {
                if (platform[y][x] is not RoundedRock || platform[y - 1][x] is not null)
                    continue;

                var newY = GetMostNorthestAvailablePosition(x, y, platform) ?? 0;
                if (newY == y)
                    continue;
                platform[newY][x] = platform[y][x];
                platform[y][x] = null;

                while (y + 1 < platform.Length && platform[y + 1][x] is RoundedRock)
                {
                    y++;
                    newY++;
                    platform[newY][x] = platform[y][x];
                    platform[y][x] = null;
                }
            }
        }
    }

    private static void TiltToWest(Tile?[][] platform)
    {
        for (var y = 0; y < platform.Length; y++)
        {
            for (var x = 1; x < platform[0].Length; x++)
            {
                if (platform[y][x] is not RoundedRock || platform[y][x - 1] is not null)
                    continue;

                var newX = GetMostWestestAvailablePosition(x, y, platform) ?? 0;
                if (newX == x)
                    continue;
                platform[y][newX] = platform[y][x];
                platform[y][x] = null;

                while (x + 1 < platform[0].Length && platform[y][x + 1] is RoundedRock)
                {
                    x++;
                    newX++;
                    platform[y][newX] = platform[y][x];
                    platform[y][x] = null;
                }
            }
        }
    }

    private static void TiltToSouth(Tile?[][] platform)
    {
        for (var x = 0; x < platform[0].Length; x++)
        {
            for (var y = platform.Length - 2; y >= 0; y--)
            {
                if (platform[y][x] is not RoundedRock || platform[y + 1][x] is not null)
                    continue;

                var newY = GetMostSouthestAvailablePosition(x, y, platform) ?? platform.Length - 1;
                if (newY == y)
                    continue;
                platform[newY][x] = platform[y][x];
                platform[y][x] = null;

                while (y > 0 && platform[y - 1][x] is RoundedRock)
                {
                    y--;
                    newY--;
                    platform[newY][x] = platform[y][x];
                    platform[y][x] = null;
                }
            }
        }
    }

    private static void TiltToEast(Tile?[][] platform)
    {
        for (var y = 0; y < platform.Length; y++)
        {
            for (var x = platform[0].Length - 2; x >= 0; x--)
            {
                if (platform[y][x] is not RoundedRock || platform[y][x + 1] is not null)
                    continue;

                var newX = GetMostEastestAvailablePosition(x, y, platform) ?? platform[0].Length - 1;
                if (newX == x)
                    continue;
                platform[y][newX] = platform[y][x];
                platform[y][x] = null;

                while (x > 0 && platform[y][x - 1] is RoundedRock)
                {
                    x--;
                    newX--;
                    platform[y][newX] = platform[y][x];
                    platform[y][x] = null;
                }
            }
        }
    }

    private static int? GetMostNorthestAvailablePosition(int currX, int currY, Tile?[][] platform)
    {
        for (var y = currY - 1; y >= 0; y--)
        {
            if (platform[y][currX] is not null)
                return y + 1;
        }

        return null;
    }

    private static int? GetMostWestestAvailablePosition(int currX, int currY, Tile?[][] platform)
    {
        for (var x = currX - 1; x >= 0; x--)
        {
            if (platform[currY][x] is not null)
                return x + 1;
        }

        return null;
    }

    private static int? GetMostSouthestAvailablePosition(int currX, int currY, Tile?[][] platform)
    {
        for (var y = currY + 1; y < platform.Length; y++)
        {
            if (platform[y][currX] is not null)
                return y - 1;
        }

        return null;
    }

    private static int? GetMostEastestAvailablePosition(int currX, int currY, Tile?[][] platform)
    {
        for (var x = currX + 1; x < platform[0].Length; x++)
        {
            if (platform[currY][x] is not null)
                return x - 1;
        }

        return null;
    }

    private static int CalculateLoadOfNorthBeam(Tile?[][] platform)
    {
        var rowLoadCounter = platform.Length;
        var totalLoad = 0;

        for (var y = 0; y < platform.Length; y++, rowLoadCounter--)
            totalLoad += platform[y].Count(x => x is RoundedRock) * rowLoadCounter;

        return totalLoad;
    }

    private class Tile;
    private class RoundedRock : Tile;
    private class CubeShapedRock : Tile;
}