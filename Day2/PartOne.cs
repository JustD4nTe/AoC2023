namespace Day2;

static class PartOne
{
    static string test1 = "../../../Day2/test1.txt";
    static string input = "../../../Day2/input.txt";
    public static long Solve()
    {
        var rawInput = File.ReadAllLines(input);

        const int numberOfGames = 100;
        var games = new List<Game>();

        for(var i = 0; i < numberOfGames; i++)
        {
            var currGame = new Game(i + 1);

            var setsOfRevealedCubesInGame = rawInput[i].Split(":")[1]
                                                       .Split(";");

            foreach (var setOfRevealedCubesInGame in setsOfRevealedCubesInGame)
            {
                var revealedCubes = setOfRevealedCubesInGame.Split(",");
                foreach (var revealedCube in revealedCubes)
                {
                    var cubeInfo = revealedCube.Split(" ");
                    var cubeCount = int.Parse(cubeInfo[1]);
                    Enum.TryParse(cubeInfo[2], true, out CubeColor cubeColor);
                    currGame.AddCubeSet(new(cubeCount, cubeColor));
                }
            }

            games.Add(currGame); 
        }

        return games.Where(x => x.IsPossible(14, 12, 13)).Sum(x => x.Id);
    }

    private enum CubeColor
    {
        Blue,
        Red,
        Green
    }

    private record CubeSet(int CubeCount, CubeColor CubeColor);

    private record Game(int Id)
    {
        private readonly List<CubeSet> _cubeSets = [];
        public int Id = Id;

        public void AddCubeSet(CubeSet cubeSet) => _cubeSets.Add(cubeSet);

        public int GetCubeCount(CubeColor cubeColor) 
            => GetSingleColorCubes(cubeColor).Sum(x => x.CubeCount);

        public bool IsPossible(int blueCount, int redCount, int greenCount)
            => GetSingleColorCubes(CubeColor.Blue).All(x => x.CubeCount <= blueCount)
               && GetSingleColorCubes(CubeColor.Red).All(x => x.CubeCount <= redCount)
               && GetSingleColorCubes(CubeColor.Green).All(x => x.CubeCount <= greenCount);

        private IEnumerable<CubeSet> GetSingleColorCubes(CubeColor cubeColor) 
            => _cubeSets.Where(x => x.CubeColor == cubeColor);
    }
}