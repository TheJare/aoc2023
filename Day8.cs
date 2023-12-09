using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day8(string inputfile)
    {
        // var lines = ReadLines("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        long r1 = 0;
        long r2 = 1;
        var steps = lines[0];
        int displayEvery = steps.Length;

        static int nodeNum(string node) { return ((node[0] - 'A') * 30 + (node[1] - 'A')) * 30 + node[2] - 'A'; }

        (int l, int r)[] nodes = new (int, int)[30 * 30 * 30];
        List<int> starterNodes = new();
        foreach (var l in lines.Skip(1))
        {
            var g = l.Split(new char[] { '=', '(', ')', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            nodes[nodeNum(g[0])] = (nodeNum(g[1]), nodeNum(g[2]));
            if (g[0][2] == 'A') starterNodes.Add(nodeNum(g[0]));
        }

        // Part 1
        int distance(int from, int to)
        {
            int d = 0;
            for (int i = 0; from != to; ++d, i = (i + 1) % steps.Length)
            {
                var (l, r) = nodes[from];
                from = steps[i] == 'L' ? l : r;
            }
            return d;
        }
        r1 = distance(nodeNum("AAA"), nodeNum("ZZZ"));

        // Part 2
        int distanceZ(int from)
        {
            int d = 0;
            for (int i = 0; from % 30 != ('Z' - 'A'); ++d, i = (i + 1) % steps.Length)
            {
                var (l, r) = nodes[from];
                from = steps[i] == 'L' ? l : r;
            }
            return d;
        }
        r2 = starterNodes.Aggregate(1L, (acc, n) => LCM(acc, distanceZ(n)));

        yield return null;
        Result(r1, r2);
    }
}