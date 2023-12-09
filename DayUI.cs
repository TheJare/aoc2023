using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Diagnostics;

public partial class DayUI : Control
{
    [Export]
    Label _labelR1;
    [Export]
    Label _labelR2;

    [Export]
    protected Node3D root;

    // Roll-your-own coroutine

    IEnumerator _coroutine;
    float _coroutineDelay;
    protected float CoDeltaTime = 0;

    private void StartCoroutine(IEnumerator c)
    {
        _coroutine = c;
        _coroutineDelay = 0;
    }

    public override void _Process(double ddelta)
    {
        float delta = (float)ddelta;
        if (_coroutine != null)
        {
            _coroutineDelay = Mathf.Max(0, _coroutineDelay - delta);
            if (_coroutineDelay <= 0)
            {
                CoDeltaTime = delta;
                if (_coroutine.MoveNext())
                {
                    float.TryParse(_coroutine.Current?.ToString(), out _coroutineDelay);
                }
                else
                {
                    _coroutine = null;
                }
            }
        }
    }

    // General purpose utilities

    private static string[] ReadLinesSkipEmpty(string filePath)
    {
        using var file = Godot.FileAccess.Open("res://" + filePath, Godot.FileAccess.ModeFlags.Read);
        string[] lines = file.GetAsText(true).Split('\n');
        return lines.Select(l => l.Trim()).Where(l => l.Length > 0).ToArray();
    }

    private static string[] ReadLines(string filePath)
    {
        using var file = Godot.FileAccess.Open("res://" + filePath, Godot.FileAccess.ModeFlags.Read);
        string[] lines = file.GetAsText(true).Split('\n');
        return lines.Select(l => l.Trim()).ToArray();
    }

    static long GCF(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    static long LCM(long a, long b)
    {
        return a / GCF(a, b) * b;
    }

    // Solution handling

    public void Result(string s1, string s2)
    {
        if (s1 != null) _labelR1.Text = s1;
        if (s2 != null) _labelR2.Text = s2;
    }

    void Result(long? i1, long? i2)
    {
        Result(i1?.ToString(), i2?.ToString());
    }

    public void EndDay()
    {
        for (int i = 0; i < root.GetChildCount(); i++)
        {
            root.GetChild(i).QueueFree();
        }
        _coroutine = null;
        _coroutineDelay = 0;
    }

    public void RunDay(int day)
    {
        EndDay();
        string inputfile = $"inputs/day{day}.txt";
        switch (day)
        {
            case 1: StartCoroutine(Day1(inputfile)); break;
            case 2: StartCoroutine(Day2(inputfile)); break;
            case 3: StartCoroutine(Day3(inputfile)); break;
            case 4: StartCoroutine(Day4(inputfile)); break;
            case 5: StartCoroutine(Day5(inputfile)); break;
            case 6: StartCoroutine(Day6(inputfile)); break;
            case 7: StartCoroutine(Day7(inputfile)); break;
            case 8: StartCoroutine(Day8(inputfile)); break;
        }

    }
}
