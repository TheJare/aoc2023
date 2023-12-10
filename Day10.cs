using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public partial class DayUI
{
    const int Up = 1 << 0;
    const int Down = 1 << 1;
    const int Left = 1 << 2;
    const int Right = 1 << 3;
    IEnumerator Day10(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int w = lines[0].Length;
        int h = lines.Length;
        // Edges that this cell type connects to. Up Down Left Right
        int[] celltype = new int[128];
        celltype['|'] = Up | Down;
        celltype['-'] = Left | Right;
        celltype['F'] = Down | Right;
        celltype['7'] = Down | Left;
        celltype['J'] = Up | Left;
        celltype['L'] = Up | Right;
        celltype['S'] = Up | Down | Left | Right;
        // Edges from an adjacent cell that connect to this cell type
        int[] icelltype = new int[128];
        icelltype['|'] = Up | Down;
        icelltype['-'] = Left | Right;
        icelltype['F'] = Up | Left;
        icelltype['7'] = Up | Right;
        icelltype['J'] = Down | Right;
        icelltype['L'] = Down | Left;
        icelltype['S'] = Up | Down | Left | Right;
        (int dx, int dy, int mask)[] directions = { (0, -1, Up), (0, 1, Down), (-1, 0, Left), (1, 0, Right) };

        int cellAt(int x, int y)
        {
            return (x >= 0 && x < w && y >= 0 && y < h) ? celltype[lines[y][x]] : 0;
        }
        int icellAt(int x, int y)
        {
            return (x >= 0 && x < w && y >= 0 && y < h) ? icelltype[lines[y][x]] : 0;
        }

        int r1 = 0;
        int r2 = 0;
        // Find the starting point with some Linq for practice
        var (sx, sy) = Enumerable.Range(0, w).SelectMany(x => Enumerable.Range(0, h),
            (x, y) => (x, y)).First(p => lines[p.y][p.x] == 'S');

        // We use a map with each original cell blown up to 3x3 and fill pipe route
        // only between the centers of the cells. This ensures 1) (0,0) is free, and
        // 2) there's empty room between adjacent parallel walls.
        int[,] map3 = new int[h * 3, w * 3];
        void fillRect3(int x0, int x1, int y0, int y1)
        {
            int x = Math.Min(x0, x1) * 3 + 1;
            int y = Math.Min(y0, y1) * 3 + 1;
            int tx = Math.Max(x0, x1) * 3 + 1;
            int ty = Math.Max(y0, y1) * 3 + 1;
            for (int fy = y; fy <= ty; ++fy)
                for (int fx = x; fx <= tx; ++fx)
                    map3[fy, fx] = 1;
        }

        // Find the pipe loop & mark it in the 3x map
        int len = 0;
        var (x, y) = (sx, sy);
        var (px, py) = (sx, sy);
        do
        {
            foreach (var (dx, dy, mask) in directions)
            {
                var (nx, ny) = (x + dx, y + dy);
                bool cellsConnect = (mask & cellAt(x, y) & icellAt(nx, ny)) != 0;
                if ((nx != px || ny != py) && cellsConnect)
                {
                    ++len;
                    (px, py) = (x, y);
                    fillRect3(x, nx, y, ny); // Fill the thin pipe to prepare for part 2
                    (x, y) = (nx, ny);
                    break;
                }
            }
        } while (x != sx || y != sy);
        r1 = len / 2;

        // typical. Return the number of cell centers (which refer to original map's cells) covered
        // Those correspond to cells in the original map that are outside the loop. Thanks to the 3x,
        // we can reach them from the map outside
        int floodFill3(int x, int y, int v)
        {
            if (x < 0 || x >= w * 3 || y < 0 || y >= h * 3 || map3[y, x] != 0) return 0;
            map3[y, x] = v;
            int r = ((x % 3) == 1 && (y % 3) == 1) ? 1 : 0;
            foreach (var (dx, dy, mask) in directions) r += floodFill3(x + dx, y + dy, v);
            return r;
        }
        // The stuff in the original map that is not pipe and not covered by the floodfill, is inside
        // the pipe loop (unreachable from outside)
        int filled = floodFill3(0, 0, 2);
        r2 = w * h - filled - len;

        yield return null;
        Result(r1, r2);
    }
}
