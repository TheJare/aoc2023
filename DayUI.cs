using Godot;
using System;
using System.IO;
using System.Linq;

public partial class DayUI : Control
{
    string r1, r2;
    [Export]
    Label _labelR1;
    [Export]
    Label _labelR2;

    private static string[] ReadLinesSkipEmpty(string file)
    {
        var lines = File.ReadAllLines(file);
        return lines.Select(l => l.Trim()).Where(l => l.Length > 0).ToArray();
    }

    public void Result(string s1, string s2)
    {
        r1 = s1;
        r2 = s2;
        _labelR1.Text = r1;
        _labelR2.Text = r2;
    }

    void Result(int i1, int i2)
    {
        Result(i1.ToString(), i2.ToString());
    }
    void Result((int i1, int i2) r)
    {
        Result(r.i1.ToString(), r.i2.ToString());
    }
    void Result((string s1, string s2) r)
    {
        Result(r.s1, r.s2);
    }

    public void RunDay(int day)
    {
        string inputfile = $"inputs/day{day}.txt";
        switch (day)
        {
            case 1: Result(Day1(inputfile)); break;
            case 2: Result(Day2(inputfile)); break;
        }

    }
}
