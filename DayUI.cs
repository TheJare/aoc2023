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

    IEnumerator _coroutine;
    float _coroutineDelay;
    protected float CoDeltaTime = 0;

    private void StartCoroutine(IEnumerator c)
    {
        _coroutine = c;
        _coroutineDelay = 0;
    }

    private static string[] ReadLinesSkipEmpty(string file)
    {
        var lines = File.ReadAllLines(file);
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
                    float.TryParse(_coroutine.Current.ToString(), out _coroutineDelay);
                }
                else
                {
                    _coroutine = null;
                }
            }
        }
    }

    public void RunDay(int day)
    {
        string inputfile = $"inputs/day{day}.txt";
        _coroutine = null;
        _coroutineDelay = 0;
        switch (day)
        {
            case 1: StartCoroutine(Day1(inputfile)); break;
            case 2: StartCoroutine(Day2(inputfile)); break;
        }

    }
}
