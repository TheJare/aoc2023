using System.IO;

public partial class DayUI
{
    (int, int) Day1()
    {
        var input = ReadLinesSkipEmpty("test.txt");
        return (input.Length, 1);
    }
}