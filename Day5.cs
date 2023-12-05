using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day5(string inputfile)
    {
        // var lines = ReadLines("test.txt");
        var lines = ReadLines(inputfile);
        int displayEvery = lines.Length / 100;
        long r1 = 0;
        long r2 = 0;
        var seeds = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(s => long.Parse(s)).ToArray();
        List<(long i, long n)> seedRanges = seeds.Chunk(2).Select(chunk => (chunk[0], chunk[1])).ToList();

        // Split an input range (i, n) by a conversion (to, from, len)
        // Yields three (i, n) subranges: left, middle (converted) and right
        // Each subrange is only valid if its n is > 0, so the usable result is
        // between 1 and 3 usable subranges. The middle one, if valid, is converted
        static (long, long)[] SplitRange(long i, long n, long to, long from, long len)
        {
            (long a, long b)[] r = new (long, long)[3];
            r[0] = (i, Math.Min(i + n, from));
            r[1] = (Math.Max(i, from), Math.Min(i + n, from + len));
            r[2] = (Math.Max(i, from + len), i + n);
            r[0] = (r[0].a, r[0].b - r[0].a);
            r[1] = (r[1].a + to - from, r[1].b - r[1].a);
            r[2] = (r[2].a, r[2].b - r[2].a);
            return r;
        }

        List<(long, long, long)> conversions = new();
        foreach (var (l, lineIndex) in lines.Skip(2).Select((l, i) => (l, i)))
        {
            if (l.Length == 0)
            {
                // Empty line, let's apply the conversions
                // very nice of them to include an empty line at the end of the input
                // Part 1
                for (int j = 0; j < seeds.Length; j++)
                {
                    foreach (var (to, from, len) in conversions)
                    {
                        if (seeds[j] >= from && seeds[j] < (from + len))
                        {
                            seeds[j] += to - from;
                            break;
                        }
                    }
                }

                // Part 2. Ok
                List<(long i, long n)> newRanges = new(seedRanges.Count);
                for (int j = 0; j < seedRanges.Count; j++)
                {
                    var (i, n) = seedRanges[j];
                    bool intact = true;
                    for (int k = 0; k < conversions.Count; ++k)
                    {
                        var (to, from, len) = conversions[k];
                        (long i, long n)[] split = SplitRange(i, n, to, from, len);
                        if (split[0].n > 0 && split[0].n < n)
                        {
                            // Subrange to the left of the conversion when original
                            // range does get split by the conversion: add to the tail
                            // of list of ranges to check so it goes through later conversions
                            seedRanges.Add(split[0]);
                        }
                        if (split[2].n > 0 && split[2].n < n)
                        {
                            // Subrange to the right of the conversion after a split
                            seedRanges.Add(split[2]);
                        }
                        if (split[1].n > 0)
                        {
                            // Converted subrange gets transferred to the list of ranges
                            // in the destination space
                            newRanges.Add(split[1]);
                            intact = false;
                            break;
                        }
                    }
                    // If range didn't suffer any conversion, let it pass intact
                    if (intact) newRanges.Add((i, n));
                }
                // Sort and coalesce converted ranges
                // Turns out this doesn't help much, ends with 48 ranges vs 116
                // Hopefully will be nice for visualization.
                newRanges.Sort((a, b) => Math.Sign(a.i - b.i));
                for (int j = newRanges.Count - 2; j >= 0; --j)
                {
                    if (newRanges[j].i + newRanges[j].n >= newRanges[j + 1].i)
                    {
                        newRanges[j] = (newRanges[j].i, newRanges[j + 1].i + newRanges[j + 1].n - newRanges[j].i);
                        newRanges.RemoveAt(j + 1);
                    }
                }
                seedRanges = newRanges;

                conversions.Clear();
                r1 = seeds.Min();
                r2 = seedRanges.Select(e => e.i).Min();
                Result(r1.ToString(), r2.ToString());
                yield return null;
            }
            else
            {
                var ls = l.Split(' ');
                if (ls.Length == 3)
                {
                    var range = ls.Select(s => long.Parse(s)).ToArray();
                    conversions.Add((range[0], range[1], range[2]));
                }
            }
        }
    }
}