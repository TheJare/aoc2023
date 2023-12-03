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

    IEnumerator _coroutine;
    float _coroutineDelay;
    protected float CoDeltaTime = 0;

    private void StartCoroutine(IEnumerator c)
    {
        _coroutine = c;
        _coroutineDelay = 0;
    }

    private static string[] ReadLinesSkipEmpty(string filePath)
    {
        using var file = Godot.FileAccess.Open("res://" + filePath, Godot.FileAccess.ModeFlags.Read);
        string[] lines = file.GetAsText(true).Split('\n');
        return lines.Select(l => l.Trim()).Where(l => l.Length > 0).ToArray();
    }

    public void Result(string s1, string s2)
    {
        if (s1 != null) _labelR1.Text = s1;
        if (s2 != null) _labelR2.Text = s2;
    }


    void Result(int? i1, int? i2)
    {
        Result(i1?.ToString(), i2?.ToString());
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
        }

    }
}
