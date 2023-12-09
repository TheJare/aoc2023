using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day9(string inputfile)
    {
        // var lines = ReadLines("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        long r1 = 0;
        long r2 = 0;

        foreach (var (l, i) in lines.Select((l, i) => (l, i)))
        {
            var items = l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToInt()).ToArray();

            for (int n = items.Length - 1; n > 0; --n)
            {
                r1 += items[n];
                int rn = items.Length - n;
                r2 += items[0] * (rn % 2 == 0 ? -1 : 1);
                bool allZeros = true;
                for (int j = 0; j < n; ++j)
                {
                    items[j] = items[j + 1] - items[j];
                    allZeros = allZeros && items[j] == 0;
                }
                if (allZeros) break;
            }
            Result(r1, r2);
            yield return null;
        }
        Result(r1, r2);
    }
}