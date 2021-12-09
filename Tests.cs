using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2021;

public class Tests
{
    [Theory]
    [InlineData("Puzzle01_Example.txt", 7)]
    [InlineData("Puzzle01.txt", 1121)]
    public async Task Day01_Part1(string inputPath, int expectedValue)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);

        var lastDepth = int.MinValue;
        var deeperThanLast = lines.Select(int.Parse).Count(depth =>
        {
            var greaterThanLastDepth = depth > lastDepth;
            lastDepth = depth;
            return greaterThanLastDepth;
        });

        // -1 because the first depth doesn't count
        (deeperThanLast - 1).Should().Be(expectedValue);
    }
}