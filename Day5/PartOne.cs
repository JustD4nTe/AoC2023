
namespace Day5;

static class PartOne
{
    static readonly string test1 = "../../../Day5/test1.txt";
    static readonly string input = "../../../Day5/input.txt";

    public static long Solve()
    {
        var seedToSoilMap = new Dictionary<long, long>();
        var soilToFertilizerMap = new Dictionary<long, long>();
        var fertilizerToWaterMap = new Dictionary<long, long>();
        var waterToLightMap = new Dictionary<long, long>();
        var lightToTemperatureMap = new Dictionary<long, long>();
        var temperatureToHumidityMap = new Dictionary<long, long>();
        var humidityToLocationMap = new Dictionary<long, long>();

        var rawInput = File.ReadAllLines(input);
        var index = 0;

        var seeds = rawInput[index][7..].Split(" ")
                                        .Select(long.Parse);
        index += 3;

        ParseMapping(ref index, seeds, seedToSoilMap, rawInput);
        ParseMapping(ref index, seedToSoilMap.Values, soilToFertilizerMap, rawInput);
        ParseMapping(ref index, soilToFertilizerMap.Values, fertilizerToWaterMap, rawInput);
        ParseMapping(ref index, fertilizerToWaterMap.Values, waterToLightMap, rawInput);
        ParseMapping(ref index, waterToLightMap.Values, lightToTemperatureMap, rawInput);
        ParseMapping(ref index, lightToTemperatureMap.Values, temperatureToHumidityMap, rawInput);
        ParseMapping(ref index, temperatureToHumidityMap.Values, humidityToLocationMap, rawInput);
        
        return humidityToLocationMap.Values.Min();
    }

    private static void ParseMapping(ref int index, IEnumerable<long> seeds, Dictionary<long, long> map, string[] rawInput)
    {
        while (rawInput.Length > index && rawInput[index] != string.Empty)
        {
            var rawMap = rawInput[index].Split(" ")
                                        .Select(long.Parse)
                                        .ToArray();
            var destStart = rawMap[0];
            var sourceStart = rawMap[1];
            var rangeLength = rawMap[2];

            var seedsToMap = seeds.Where(x => x >= sourceStart && x < sourceStart + rangeLength);

            foreach (var seedToMap in seedsToMap)
            {
                map[seedToMap] = (seedToMap - sourceStart) + destStart;
            }

            index++;
        }

        Fill(seeds, map);

        index += 2;
    }

    private static void Fill(IEnumerable<long> seeds, Dictionary<long, long> map)
    {
        var missingKeys = seeds.Except(map.Keys);

        foreach (var missingKey in missingKeys)
        {
            map[missingKey] = missingKey;
        }
    }
}