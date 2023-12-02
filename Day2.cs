using System;
using System.Collections;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day2(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int displayEvery = lines.Length / 100;
        int r1 = 0;
        int r2 = 0;
        foreach (var (l, lineIndex) in lines.Select((l, i) => (l, i)))
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

            void createText(int val, int max, Color color, float x)
            {
                bool ok = val <= max;
                var text = new Label3D
                {
                    Text = val.ToString(),
                    Modulate = ok ? color : Colors.White,
                    Position = new Vector3(x, 0, 2.5f),
                    FontSize = 100
                };
                root.AddChild(text);
                Tween tw = text.CreateTween();
                tw.TweenProperty(text, "position", new Vector3(text.Position.X, text.Position.Y, text.Position.Z - 8f), 0.5);
                Color destColor = new(color.R, color.G, color.B, 0);
                tw.Parallel().TweenProperty(text, "modulate", destColor, 0.5);
                tw.Parallel().TweenProperty(text, "outline_modulate", destColor, 0.3);
                tw.TweenCallback(Callable.From(text.QueueFree));
            }
            if (lineIndex % displayEvery == 0)
            {
                createText(maxr, 12, Colors.Red, -1f);
                createText(maxg, 13, Colors.Green, 0);
                createText(maxb, 14, Colors.Blue, 1f);
                Result(r1, r2);
                yield return 0.05f;
            }
        }
        Result(r1, r2);
    }
}