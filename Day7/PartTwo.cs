namespace Day7;

static class PartTwo
{
    static readonly string test1 = "../../../Day7/test1.txt";
    static readonly string input = "../../../Day7/input.txt";
    public static long Solve()
    {
        var hands = File.ReadAllLines(input)
                        .Select(x => x.Split(" "))
                        .Select(x => new Hand(x[0], x[1]))
                        .ToArray();

        var temp = hands.Order(new HandComparer()).ToArray();

        var totalWinnings = 0;

        for (var i = 0; i < temp.Length; i++)
            totalWinnings += (i + 1) * temp[i].Bid;

        return totalWinnings;
    }

    private enum HandType
    {
        FiveOfKind = 7,
        FourOfKind = 6,
        FullHouse = 5,
        ThreeOfKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }

    private class Hand
    {
        public readonly char[] Cards;
        public readonly HandType HandType;
        public readonly int Bid;

        public Hand(string cards, string bid)
        {
            Cards = cards.ToCharArray();

            HandType = GetHandType();

            Bid = int.Parse(bid);
        }

        private HandType GetHandType()
        {
            var cardCounter = new Dictionary<char, int>();

            foreach (var card in Cards)
            {
                if (cardCounter.TryGetValue(card, out int cardCount))
                    cardCounter[card]++;
                else
                    cardCounter[card] = 1;
            }

            if (cardCounter.Keys.Count > 1 && cardCounter.TryGetValue('J', out int jCount))
            {
                cardCounter.Remove('J');
                var highestCardCount = cardCounter.Values.Max();
                var highestCard = cardCounter.First(x => x.Value == highestCardCount).Key;
                cardCounter[highestCard] += jCount;
            }

            if (cardCounter.Keys.Count == 1)
                return HandType.FiveOfKind;
            if (cardCounter.Keys.Count == 2 && cardCounter.Values.Any(x => x == 4))
                return HandType.FourOfKind;
            if (cardCounter.Keys.Count == 2 && cardCounter.Values.Any(x => x == 3))
                return HandType.FullHouse;
            if (cardCounter.Keys.Count == 3 && cardCounter.Values.Any(x => x == 3))
                return HandType.ThreeOfKind;
            if (cardCounter.Keys.Count == 3 && cardCounter.Values.Where(x => x == 2).Count() == 2)
                return HandType.TwoPair;
            if (cardCounter.Keys.Count == 4 && cardCounter.Values.Any(x => x == 2))
                return HandType.OnePair;
            if (cardCounter.Keys.Count == 5)
                return HandType.HighCard;

            throw new Exception("Could not find hand type");
        }
    }

    private class HandComparer : IComparer<Hand>
    {
        readonly string _cardStrength = "J23456789TQKA";
        public int Compare(Hand? x, Hand? y)
        {
            if (x!.HandType != y!.HandType)
                return x!.HandType.CompareTo(y!.HandType);

            for (var i = 0; i < 5; i++)
            {
                if (_cardStrength.IndexOf(x!.Cards[i]) > _cardStrength.IndexOf(y!.Cards[i]))
                    return 1;
                if (_cardStrength.IndexOf(x!.Cards[i]) < _cardStrength.IndexOf(y!.Cards[i]))
                    return -1;
            }

            return 0;
        }
    }
}