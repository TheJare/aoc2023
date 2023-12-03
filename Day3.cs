using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day3(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int w = lines[0].Length;
        int h = lines.Length;
        int displayEvery = lines.Length / 100;

        char charAt(int x, int y)
        {
            if (x >= 0 && x < w && y >= 0 && y < h)
            {
                return lines[y][x];
            }
            return '.';
        }

        bool isSymbol(int x, int y)
        {
            char c = charAt(x, y);
            return !Char.IsDigit(c) && c != '.';
        }

        bool containsSymbols(int x, int y, int n)
        {
            for (int iy = -1; iy < 2; ++iy)
            {
                for (int ix = -1; ix < n + 1; ++ix)
                {
                    if (isSymbol(x + ix, y + iy)) return true;
                }
            }
            return false;
        }

        Dictionary<(int, int), (int, int)> gears = new(); // gearat[(x,y)] = (numneighbors, ratio)
        int checkGear(int x, int y, int v)
        {
            int r = 0;
            if (charAt(x, y) == '*')
            {
                if (!gears.TryGetValue((x, y), out (int n, int v) val))
                {
                    val = (1, v);
                    gears[(x, y)] = val;
                }
                else if (val.n == 1)
                {
                    val = (val.n + 1, val.v * v);
                    gears[(x, y)] = val;
                    r = val.v;
                }
                else if (val.n == 2)
                {
                    // This never happens, so we could instead delete in the previous else if
                    Debug.WriteLine($"Gear overflow connected to 3+ nodes at {x},{y}");
                    r = -val.v; // Correct previously assigned ratio
                    gears[(x, y)] = (val.n + 1, 0);
                }
            }
            return r;
        }
        int scanGears(int x, int y, int n, int v)
        {
            int r = 0;
            for (int iy = -1; iy < 2; ++iy)
            {
                for (int ix = -1; ix < n + 1; ++ix)
                {
                    r += checkGear(x + ix, y + iy, v);
                }
            }
            return r;
        }

        int r1 = 0;
        int r2 = 0;
        int numr = 0;
        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < w; ++x)
            {
                int v = 0;
                int sw = 0;
                while (x < w && Char.IsDigit(lines[y][x]))
                {
                    v = v * 10 + lines[y][x] - '0';
                    sw++;
                    x++;
                }
                if (sw > 0 && containsSymbols(x - sw, y, sw))
                {
                    r1 += v;
                    r2 += scanGears(x - sw, y, sw, v);
                    if (numr++ % (h / 10) == 0)
                    {
                        Result(r1, r2);
                        yield return null;
                    }
                }
            }
        }
        Result(r1, r2);
    }
}