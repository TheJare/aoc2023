using System;
using System.Collections;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day4(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int displayEvery = lines.Length / 100;
        int r1 = 0;
        int r2 = 0;
        int[] multiplier = new int[lines.Length];
        foreach (var (l, lineIndex) in lines.Select((l, i) => (l, i)))
        {
            var g = l.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool[] cards = new bool[100];
            int i = 2;
            for (; i < g.Length && g[i] != "|"; ++i)
            {
                cards[g[i].ToInt()] = true;
            }
            int v = 0;
            for (i++; i < g.Length; ++i)
            {
                if (cards[g[i].ToInt()]) v++;
            }
            r1 += (1 << v) / 2;
            for (int j = 0; j < v; j++)
            {
                multiplier[lineIndex + j + 1] += multiplier[lineIndex] + 1;
            }
            r2 += multiplier[lineIndex] + 1;
            if (lineIndex % displayEvery == 0)
            {
                Result(r1, r2);
                yield return null;
            }
        }
        Result(r1, r2);
    }
}