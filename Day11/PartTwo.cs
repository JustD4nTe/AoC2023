namespace Day11;

static class PartTwo
{
    static readonly string test1 = "../../../Day11/test1.txt";
    static readonly string input = "../../../Day11/input.txt";

    public static long Solve()
    {
        var img = File.ReadAllLines(input)
                           .Select(x => x.ToCharArray())
                           .ToArray();

        var galaxies = GetGalaxies(img);
        ExpandGalaxies(galaxies, img);

        long result = 0;

        for (var i = 0; i < galaxies.Length - 1; i++)
        {
            result += galaxies[(i + 1)..].Sum(x => galaxies[i] - x);
        }

        return result;
    }

    private static void ExpandGalaxies(Position[] galaxies, char[][] img)
    {
        const int EXPAND_SIZE = 1_000_000 - 1;
        for (int y = 0, expandedY = 0; y < img.Length; y++, expandedY++)
        {
            if (img[y].All(x => x == '.'))
            {
                for (var i = 0; i < galaxies.Length; i++)
                {
                    if (galaxies[i].Y > expandedY)
                        galaxies[i] = new Position(galaxies[i].X, galaxies[i].Y + EXPAND_SIZE);
                }
                expandedY += EXPAND_SIZE;
            }
        }

        for (int x = 0, expandedX = 0; x < img[0].Length; x++, expandedX++)
        {
            if (img.Select(p => p[x]).All(p => p == '.'))
            {
                for (var i = 0; i < galaxies.Length; i++)
                {
                    if (galaxies[i].X > expandedX)
                        galaxies[i] = new Position(galaxies[i].X + EXPAND_SIZE, galaxies[i].Y);
                }
                expandedX += EXPAND_SIZE;
            }
        }

    }

    private static Position[] GetGalaxies(char[][] img)
    {
        var galaxyPositions = new List<Position>();

        for (var y = 0; y < img.Length; y++)
        {
            for (var x = 0; x < img[y].Length; x++)
            {
                if (img[y][x] == '#')
                    galaxyPositions.Add(new(x, y));
            }
        }

        return galaxyPositions.ToArray();
    }

    private record Position(long X, long Y)
    {
        public static long operator -(Position p1, Position p2)
            => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }
}