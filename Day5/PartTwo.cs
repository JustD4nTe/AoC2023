using System.Diagnostics.CodeAnalysis;

namespace Day5;

static class PartTwo
{
    static readonly string test1 = "../../../Day5/test1.txt";
    static readonly string input = "../../../Day5/input.txt";

    private record Range(long Start, long End) : IEqualityComparer<Range>
    {
        public bool Equals(Range? x, Range? y)
            => x.Start == y.Start && x.End == y.End;

        public int GetHashCode([DisallowNull] Range obj)
        {
            throw new NotImplementedException();
        }
    }

    public static long Solve()
    {
        var seedToSoilMap = new Dictionary<Range, Range>();
        var soilToFertilizerMap = new Dictionary<Range, Range>();
        var fertilizerToWaterMap = new Dictionary<Range, Range>();
        var waterToLightMap = new Dictionary<Range, Range>();
        var lightToTemperatureMap = new Dictionary<Range, Range>();
        var temperatureToHumidityMap = new Dictionary<Range, Range>();
        var humidityToLocationMap = new Dictionary<Range, Range>();

        var rawInput = File.ReadAllLines(input);
        var index = 0;

        var seedsToPlant = rawInput[index][7..].Split(" ")
                                        .Select(long.Parse)
                                        .ToArray();

        var seeds = new List<Range>();

        for (var i = 0; i < seedsToPlant.Length; i += 2)
        {
            var start = seedsToPlant[i];
            var end = seedsToPlant[i] + seedsToPlant[i + 1] - 1;
            seeds.Add(new(start, end));
        }

        index += 3;

        ParseMapping(ref index, seeds, seedToSoilMap, rawInput);
        ParseMapping(ref index, seedToSoilMap.Values, soilToFertilizerMap, rawInput);
        ParseMapping(ref index, soilToFertilizerMap.Values, fertilizerToWaterMap, rawInput);
        ParseMapping(ref index, fertilizerToWaterMap.Values, waterToLightMap, rawInput);
        ParseMapping(ref index, waterToLightMap.Values, lightToTemperatureMap, rawInput);
        ParseMapping(ref index, lightToTemperatureMap.Values, temperatureToHumidityMap, rawInput);
        ParseMapping(ref index, temperatureToHumidityMap.Values, humidityToLocationMap, rawInput);

        return humidityToLocationMap.Values.Min(x => x.Start);
    }

    private static void ParseMapping(ref int index, IEnumerable<Range> seeds, Dictionary<Range, Range> map, string[] rawInput)
    {
        while (rawInput.Length > index && rawInput[index] != string.Empty)
        {
            var rawMap = rawInput[index].Split(" ")
                                        .Select(long.Parse)
                                        .ToArray();

            var destStart = rawMap[0];
            var sourceStart = rawMap[1];
            var rangeLength = rawMap[2];
            var sourceEnd = sourceStart + rangeLength - 1;

            var rangesToMap = new List<Range>();

            // case [1]: entire mapping is in seed range
            rangesToMap.AddRange(seeds.Where(x => x.Start <= sourceStart && x.End >= sourceEnd && sourceStart <= x.End && sourceEnd >= x.Start));

            // case [2]: only end is in seed range
            rangesToMap.AddRange(seeds.Where(x => x.Start >= sourceStart && x.End >= sourceEnd && sourceStart <= x.End && sourceEnd >= x.Start));

            // case [3]: only start is in seed range
            rangesToMap.AddRange(seeds.Where(x => x.Start <= sourceStart && x.End <= sourceEnd && sourceStart <= x.End && sourceEnd >= x.Start));

            // case [4]: mapping is larger than seed range
            rangesToMap.AddRange(seeds.Where(x => x.Start >= sourceStart && x.End <= sourceEnd && sourceStart <= x.End && sourceEnd >= x.Start));

            rangesToMap = rangesToMap.Distinct().ToList();

            foreach (var rangeToMap in rangesToMap)
            {
                var internalStart = Math.Max(rangeToMap.Start, sourceStart);
                var internalEnd = Math.Min(rangeToMap.End, sourceEnd);
                var diffStart = internalStart - sourceStart;
                var diffLength = internalEnd - internalStart;

                destStart += diffStart;
                var destEnd = destStart + diffLength;

                var source = new Range(internalStart, internalEnd);

                map[source] = new Range(destStart, destEnd);
            }

            index++;
        }

        Fill(seeds, map);

        index += 2;
    }



    private static void Fill(IEnumerable<Range> seeds, Dictionary<Range, Range> map)
    {
        foreach (var seed in seeds)
        {
            var mappedSeeds = map.Keys.Where(x => seed.Start <= x.Start && seed.End >= x.End).OrderBy(x => x.Start).ToArray();

            // when range doesn't contain any mappings
            if (mappedSeeds.Length == 0)
            {
                map[seed] = seed;
                continue;
            }

            var defaultMappings = new List<Range>();

            // create range from seed range start to first mapping
            if (seed.Start != mappedSeeds[0].Start)
                defaultMappings.Add(new(seed.Start, mappedSeeds[0].Start - 1));

            // fill every empty space between mappings
            for (var i = 0; i < mappedSeeds.Length - 1; i++)
            {
                // skip when two mappigns are close enough 
                // (doesn't have any seed left between)
                if (mappedSeeds[i].End + 1 == mappedSeeds[i + 1].Start)
                    continue;

                defaultMappings.Add(new(mappedSeeds[i].End + 1, mappedSeeds[i + 1].Start));
            }

            // crate range from last mapping to end of seed range
            if (mappedSeeds[^1].End != seed.End)
                defaultMappings.Add(new(mappedSeeds[^1].End, seed.End));

            // add default ranges
            foreach (var defaultMapping in defaultMappings)
                map[defaultMapping] = defaultMapping;
        }
    }
}