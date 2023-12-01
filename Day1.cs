using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Godot;

public partial class DayUI
{
    (int, int) Day1(string inputfile)
    {
        // var lines = ReadLinesSkipEmpty("test.txt");
        var lines = ReadLinesSkipEmpty(inputfile);
        int r1 = 0;
        foreach (var l in lines)
        {
            var v = l.First(c => Char.IsDigit(c)).ToString() + l.Last(c => Char.IsDigit(c)).ToString();
            r1 += v.ToInt();
        }
        string[] digits = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            " zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
        };
        var whichDigit = (string s) =>
        {
            for (int i = 0; i < digits.Length; i++)
            {
                if (s.StartsWith(digits[i])) return i % 10;
            }
            return -1;
        };

        int r2 = 0;
        foreach (var l in lines)
        {
            var v = 0;
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
            Debug.WriteLine($"{v}");
            r2 += v;
        }
        return (r1, r2);
    }
}