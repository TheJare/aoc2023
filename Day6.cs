using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day6(string inputfile)
    {
        // var lines = ReadLines("test.txt");
        var lines = ReadLines(inputfile);
        int r1 = 1;
        int r2 = 0;

        int[] times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).Select(e => e.ToInt()).ToArray();
        int[] dists = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Skip(1).Select(e => e.ToInt()).ToArray();

        static int FindAlternatives(long time, long dist)
        {
            // distance(t, totalt) = t(totalt-t) = 0 + totalt*t - t*t
            // Solve the roots t of 0 = distance(t, time) - distance
            // The ways to win are all the integers between the two
            double rootK = Mathf.Sqrt(time * time - 4.0 * dist);
            int t0 = Mathf.FloorToInt((time - rootK) / 2) + 1;
            int t1 = Mathf.CeilToInt((time + rootK) / 2);
            return t1 - t0;
        }

        // Part 1
        for (int i = 0; i < times.Length; ++i)
            r1 *= FindAlternatives(times[i], dists[i]);
        // Part 2
        long gtime = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
        long gdist = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));
        r2 = FindAlternatives(gtime, gdist);

        Result(r1, r2);
        yield return null;
    }
}