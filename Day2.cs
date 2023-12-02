using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Godot;

public partial class DayUI
{
    (int, int) Day2(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int r1 = 0;
        int r2 = 0;
        foreach (var l in lines)
        {
            var g = l.Split(new char[] { ':', ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int index = g[1].ToInt();
            int maxr = 0, maxg = 0, maxb = 0;
            for (int i = 2; i < g.Length; i += 2)
            {
                int n = g[i].ToInt();
                string t = g[i + 1];
                if (t == "red") maxr = Math.Max(maxr, n);
                else if (t == "green") maxg = Math.Max(maxg, n);
                else if (t == "blue") maxb = Math.Max(maxb, n);
            }
            if (maxr <= 12 && maxg <= 13 && maxb <= 14) r1 += index;
            r2 += maxr * maxg * maxb;
        }
        return (r1, r2);
    }
}