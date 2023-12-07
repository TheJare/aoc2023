using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    struct Play
    {
        public string hand;
        public int bid;
        public int type; // higher is better. 0 = high card; 1 = pair, 2 = two pairs, 4 = three, 5 = house, 8 = 4 of a kind, 16 = 5 of a kind
        public int value; // if types are equal, higher value wins
        public override readonly string ToString() { return $"{hand} {bid} {type} {value}"; }
    };

    const string Cards = "23456789TJQKA";
    const string Cards2 = "J23456789TQKA";
    static readonly int MaxHandValue = (int)Math.Pow(Cards.Length, 5);

    IEnumerator Day7(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);

        static int processPlays(string[] lines, string cardRanks, char joker)
        {
            Play[] plays = new Play[lines.Length];
            foreach (var (l, lineIndex) in lines.Select((l, i) => (l, i)))
            {
                var play = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Play p = new()
                {
                    hand = play[0],
                    bid = play[1].ToInt()
                };
                char[] cards = p.hand.ToCharArray();
                p.value = 0;
                p.type = 0;
                int numJ = 0;
                for (int i = 0; i < 5; ++i)
                {
                    p.value = p.value * cardRanks.Length + cardRanks.IndexOf(p.hand[i]);
                    if (cards[i] == joker) numJ++;
                    else if (cards[i] != '.')
                    {
                        int numRepeats = 0;
                        for (int j = i + 1; j < 5; j++)
                        {
                            if (cards[i] == cards[j])
                            {
                                numRepeats++;
                                cards[j] = '.';
                            }
                        }
                        p.type += numRepeats >= 2 ? (1 << numRepeats) : numRepeats;
                    }
                }
                if (p.type == 8) p.type <<= numJ;
                else if (p.type == 4) p.type <<= numJ;
                else if (p.type == 2 && numJ == 1) p.type = 5;
                else if (p.type == 1 && numJ > 0) p.type <<= (numJ + 1);
                else if (p.type == 0) p.type = numJ >= 2 ? (1 << Math.Min(numJ, 4)) : numJ;
                plays[lineIndex] = p;
            }
            Array.Sort(plays, (a, b) => Math.Sign(a.type * MaxHandValue + a.value - (b.type * MaxHandValue + b.value)));
            return plays.Select((p, i) => (i + 1) * p.bid).Sum();
        }
        int r1 = processPlays(lines, Cards, ' ');
        int r2 = processPlays(lines, Cards2, 'J');

        Result(r1, r2);
        yield return null;
    }
}