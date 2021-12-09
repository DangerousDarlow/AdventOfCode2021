using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace AdventOfCode2021;

public class Tests
{
    [Theory]
    [InlineData("Puzzle01_Example.txt", 7)]
    [InlineData("Puzzle01.txt", 1121)]
    public async Task Day01_Part1(string inputPath, int result)
    {
        var values = await ReadToInts(inputPath);
        CountGreaterThanPrevious(values).Should().Be(result);
    }

    [Theory]
    [InlineData("Puzzle01_Example.txt", 5)]
    [InlineData("Puzzle01.txt", 1065)]
    public async Task Day01_Part2(string inputPath, int result)
    {
        var values = await ReadToInts(inputPath);
        var windows = values.ToWindows();
        CountGreaterThanPrevious(windows).Should().Be(result);
    }

    private static async Task<int[]> ReadToInts(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(int.Parse).ToArray();
    }

    private static int CountGreaterThanPrevious(int[] values)
    {
        var previous = int.MaxValue;
        return values.Count(value =>
        {
            var previousPrevious = previous;
            previous = value;
            return value > previousPrevious;
        });
    }
}

public static class IntArrayExtensions
{
    public static int[] ToWindows(this int[] values)
    {
        var windows = new int[values.Length - 2];
        for (var index = 0; index < values.Length - 2; ++index)
            windows[index] = values[index] + values[index + 1] + values[index + 2];

        return windows;
    }
}