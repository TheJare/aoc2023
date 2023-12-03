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

        Label3D[] letters = new Label3D[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                char c = lines[y][x];
                if (c != '.')
                {
                    var text = new Label3D
                    {
                        Text = c.ToString(),
                        Modulate = Char.IsDigit(c) ? Colors.White : Colors.Blue,
                        Position = new Vector3(0, 0, 0),
                        FontSize = 36,
                    };
                    root.AddChild(text);
                    letters[y * w + x] = text;
                    text.Hide();
                }
            }
        }
        yield return null;

        char charAt(int x, int y)
        {
            return (x >= 0 && x < w && y >= 0 && y < h) ? lines[y][x] : '.';
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
                Label3D text = letters[y * w + x];
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
                    text.Modulate = Colors.Green;
                }
                else if (val.n == 2)
                {
                    // This never happens, so we could instead delete in the previous else if
                    Debug.WriteLine($"Gear overflow connected to 3+ nodes at {x},{y}");
                    r = -val.v; // Correct previously assigned ratio
                    gears[(x, y)] = (val.n + 1, 0);
                    text.Modulate = Colors.Blue;
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
        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < w; ++x)
            {
                Label3D text = letters[y * w + x];
                if (text != null)
                {
                    text.Show();
                    Tween tw = text.CreateTween();
                    tw.TweenProperty(text, "position", new Vector3(((float)x - (float)w / 2) * 0.110f, 13f - (float)y * 0.19f, text.Position.Z - 8), 3);
                    // tw.TweenCallback(Callable.From(text.Hide));
                }
            }
            for (int x = 0; x < w; ++x)
            {
                int v = 0;
                int sw = 0;
                while (x < w && Char.IsDigit(lines[y][x]))
                {
                    v = v * 10 + lines[y][x] - '0';
                    letters[y * w + x].Modulate = Colors.Red;
                    sw++;
                    x++;
                }
                if (sw > 0 && containsSymbols(x - sw, y, sw))
                {
                    for (int i = sw; i > 0; i--)
                    {
                        letters[y * w + x - i].Modulate = Colors.Yellow;
                    }
                    r1 += v;
                    r2 += scanGears(x - sw, y, sw, v);
                }
            }
            Result(r1, r2);
            yield return null;
        }
        Result(r1, r2);
    }
}