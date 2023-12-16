using System.Text.RegularExpressions;

namespace Day10;

static class PartTwo
{
    static readonly string test3 = "../../../Day10/test3.txt";
    static readonly string test4 = "../../../Day10/test4.txt";
    static readonly string test5 = "../../../Day10/test5.txt";
    static readonly string test6 = "../../../Day10/test6.txt";
    static readonly string input = "../../../Day10/input.txt";

    static readonly Dictionary<Direction, Move> directions = new()
    {
        [Direction.West] = new(-1, 0, Direction.West),
        [Direction.North] = new(0, -1, Direction.North),
        [Direction.East] = new(1, 0, Direction.East),
        [Direction.South] = new(0, 1, Direction.South)
    };

    public static long Solve()
    {
        var field = File.ReadAllLines(input)
                        .Select(x => x.ToCharArray()
                                      .Select(Tile.Convert)
                                      .ToArray())
                        .ToArray();

        var loopPoints = new List<Position>();

        var sp = GetStartPosition(field);

        var possibleDirections = new List<Position>();

        field[sp.Y][sp.X] = Loop.Create(field[sp.Y][sp.X]);

        MoveFromStartPosition(field, sp, possibleDirections);
        loopPoints.AddRange(possibleDirections);
        GoThoughPipes(field, possibleDirections, loopPoints);

        // idk why, but i had duplicated first two positions
        loopPoints = loopPoints[2..];
        // and didn't have start position
        loopPoints.Add(new(sp.X, sp.Y));

        for (var y = 0; y < field.Length; y++)
        {
            for (var x = 0; x < field[y].Length; x++)
            {
                if (field[y][x] is Loop)
                    continue;

                if (IsInLoop(field, loopPoints, x, y))
                    field[y][x] = new Nest();
                else
                    field[y][x] = new Empty();
            }
        }

        // for (var y = 0; y < field.Length; y++)
        // {
        //     for (var x = 0; x < field[y].Length; x++)
        //     {
        //         Console.Write(field[y][x].ToString());
        //     }
        //     Console.WriteLine();
        // }

        return field.SelectMany(x => x).Count(x => x is Nest);
    }

    private static Position GetStartPosition(Tile[][] field)
    {
        for (var y = 0; y < field.Length; y++)
        {
            for (var x = 0; x < field[y].Length; x++)
            {
                if (field[y][x] is Start)
                    return new Position(x, y);
            }
        }

        throw new Exception("Start position is not found.");
    }

    private static void MoveFromStartPosition(Tile[][] field, Position sp, List<Position> possibleDirections)
    {
        foreach (var direction in directions.Values)
        {
            var oppositeDirection = GetOppositeDirection(direction.Direction);
            // nextPosition
            var np = sp.Move(direction);
            if (np.Validate(field) && field[np.Y][np.X] is Pipe pipe)
            {
                if (pipe.FirstSide == oppositeDirection || pipe.SecondSide == oppositeDirection)
                {
                    possibleDirections.Add(np);
                }
            }
        }
    }

    private static void GoThoughPipes(Tile[][] field, List<Position> possibleDirections, List<Position> loopPoints)
    {
        do
        {
            var buff = possibleDirections.Distinct().ToArray();
            possibleDirections.Clear();
            foreach (var position in buff)
            {
                var pipe = (Pipe)field[position.Y][position.X];

                var nextPosition = pipe.MoveThrough(field, position);
                if (nextPosition is not null)
                    possibleDirections.Add(nextPosition);

                field[position.Y][position.X] = Loop.Create(pipe);
                loopPoints.Add(position);
            }
        } while (possibleDirections.Count != 0);
    }

    private static bool IsInLoop(Tile[][] field, List<Position> loopPosition, int currX, int currY)
    {
        var rightBeam = string.Join("", loopPosition.Where(x => x.Y == currY && x.X > currX)
                                                    .OrderBy(x => x.X)
                                                    .Select(x => field[x.Y][x.X].Symbol));
        if(rightBeam.Length == 0)
            return false;

        var horizontalPseudoWallRegex = new Regex(@"(L{1}[-]*J{1})|(F{1}[-]*7{1})");
        var horizontalWallRegex = new Regex(@"(L{1}[-]*7{1})|(F{1}[-]*J{1})");

        // first create simple walls where wall tries go 'diagonally'
        var simplerRightBeam = horizontalWallRegex.Replace(rightBeam, "|");
        // remove empty walls like => L___J
        simplerRightBeam = horizontalPseudoWallRegex.Replace(simplerRightBeam, "");

        // point inside loop should have odd number of walls in every direction
        return IsOdd(simplerRightBeam.Length);
    }
    
    private static bool IsOdd(int value) => value % 2 == 1;

    private static Direction GetOppositeDirection(Direction direction)
        => direction switch
        {
            Direction.West => Direction.East,
            Direction.East => Direction.West,
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            _ => throw new Exception()
        };

    private record Tile(char Symbol)
    {
        public static Tile Convert(char symbol)
            => symbol switch
            {
                '|' => new Pipe(symbol, Direction.North, Direction.South),
                '-' => new Pipe(symbol, Direction.East, Direction.West),
                'L' => new Pipe(symbol, Direction.North, Direction.East),
                'J' => new Pipe(symbol, Direction.North, Direction.West),
                '7' => new Pipe(symbol, Direction.South, Direction.West),
                'F' => new Pipe(symbol, Direction.South, Direction.East),
                '.' => new Ground(symbol),
                'S' => new Start(symbol),
                _ => throw new Exception("Symbol is unrecognize.")
            };

        public override string ToString() => Symbol.ToString();
    }
    private record Start(char Symbol) : Tile(Symbol);
    private record Ground(char Symbol) : Tile(Symbol);
    private record Pipe(char Symbol, Direction FirstSide, Direction SecondSide) : Tile(Symbol)
    {
        public Position? MoveThrough(Tile[][] field, Position position)
        {
            var direction = directions[FirstSide];
            var oppositeDirection = GetOppositeDirection(direction.Direction);
            var np = position.Move(direction);

            if (np.Validate(field) && field[np.Y][np.X] is Pipe firstSidePipe)
            {
                if (firstSidePipe.FirstSide == oppositeDirection || firstSidePipe.SecondSide == oppositeDirection)
                    return np;
            }

            direction = directions[SecondSide];
            oppositeDirection = GetOppositeDirection(direction.Direction);
            np = position.Move(direction);

            if (np.Validate(field) && field[np.Y][np.X] is Pipe secondSidePipe)
            {
                if (secondSidePipe.FirstSide == oppositeDirection || secondSidePipe.SecondSide == oppositeDirection)
                    return np;
            }

            return null;
        }
    }
    private record Loop(char Symbol) : Tile(Symbol)
    {
        public static Loop Create(Tile tile)
        {
            if(tile is Start)
                return new('7');
            return new(tile.Symbol);
        }

        public override string ToString() => base.ToString();
    }
    private record Nest() : Tile('I')
    {
        public override string ToString() => base.ToString();
    }

    private record Empty() : Tile('O')
    {
        public override string ToString() => base.ToString();
    }

    private enum Direction { West, North, East, South };
    private record Move(int X, int Y, Direction Direction);
    private record Position(int X, int Y)
    {
        public Position Move(Move move) => new(X + move.X, Y + move.Y);
        public bool Validate(Tile[][] field)
        {
            if (X < 0 || Y < 0)
                return false;

            if (field[Y][X] is Ground)
                return false;

            return true;
        }
    }
}