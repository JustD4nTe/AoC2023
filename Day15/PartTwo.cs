namespace Day15;

static class PartTwo
{
    static readonly string test1 = "../../../Day15/test1.txt";
    static readonly string input = "../../../Day15/input.txt";

    public static int Solve()
    {
        var initSeq = File.ReadAllLines(input)[0]
                          .Split(",")
                          .Select(ParseInput)
                          .ToArray();

        var boxes = InitBoxes();
        InitLens(initSeq, boxes);

        return CalculateFocusingPower(boxes);
    }

    private static List<List<Lens>> InitBoxes()
    {
        var boxes = new List<List<Lens>>();
        for (var i = 0; i < 256; i++)
            boxes.Add([]);
        return boxes;
    }

    private static void InitLens(Lens[] initSeq, List<List<Lens>> boxes)
    {
        foreach (var step in initSeq)
        {
            var lens = boxes[step.BoxId].SingleOrDefault(x => x.Label == step.Label);

            if (step.Operation == Operation.Remove && lens is not null)
                boxes[step.BoxId].Remove(lens);
            else if (step.Operation == Operation.AddOrUpdate && lens is null)
                boxes[step.BoxId].Add(step);
            else if (step.Operation == Operation.AddOrUpdate && lens is not null)
                lens.ChangeFocalLength(step.FocalLength);
        }
    }

    private static int CalculateFocusingPower(List<List<Lens>> boxes)
    {
        var result = 0;
        for (var i = 0; i < 256; i++)
        {
            for (var j = 0; j < boxes[i].Count; j++)
                result += (i + 1) * (j + 1) * boxes[i][j].FocalLength;
        }

        return result;
    }

    private static Lens ParseInput(string input)
    {
        string label;
        int boxId;
        Operation operation;
        int? focalLength;
        if (input.Contains('-'))
        {
            var buff = input.Split('-');
            label = buff[0];
            boxId = HashAlgorithm(buff[0]);
            operation = Operation.Remove;
            focalLength = null;
        }
        else
        {
            var buff = input.Split("=");
            label = buff[0];
            boxId = HashAlgorithm(buff[0]);
            operation = Operation.AddOrUpdate;
            focalLength = int.Parse(buff[1]);
        }

        return new(label, boxId, operation, focalLength);
    }

    private static int HashAlgorithm(string str)
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

    private enum Operation { Remove, AddOrUpdate }
    private class Lens(string Label, int BoxId, Operation Operation, int? FocalLength)
    {
        private int? _focalLength { get; set; } = FocalLength;

        public string Label { get; } = Label;
        public int BoxId { get; } = BoxId;
        public Operation Operation { get; } = Operation;
        public int FocalLength => _focalLength ?? throw new Exception("FocalLength is null");

        public void ChangeFocalLength(int newFocalLength) => _focalLength = newFocalLength;
    }
}