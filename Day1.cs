using System;
using System.Collections;
using System.Linq;
using Godot;

public partial class DayUI
{
    IEnumerator Day1(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int displayEvery = lines.Length / 100;

        string[] digits = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            " zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
        };
        int whichDigit(string s)
        {
            for (int i = 0; i < digits.Length; i++)
            {
                if (s.StartsWith(digits[i])) return i % 10;
            }
            return -1;
        }

        int r1 = 0;
        int r2 = 0;
        foreach (var (l, lineIndex) in lines.Select((l, i) => (l, i)))
        {
            var v = (l.First(c => Char.IsDigit(c)) - '0') * 10 + l.Last(c => Char.IsDigit(c)) - '0';
            r1 += v;

            v = 0;
            for (int j = 0; j < l.Length; j++)
            {
                var d = whichDigit(l[j..]);
                if (d >= 0) { v = d * 10; break; }
            }
            for (int j = l.Length - 1; j >= 0; j--)
            {
                var d = whichDigit(l[j..]);
                if (d >= 0) { v += d; break; }
            }
            r2 += v;
            if (lineIndex % displayEvery == 0)
            {
                Result(r1, r2);
                yield return null;
            }
        }
        Result(r1, r2);
    }
}