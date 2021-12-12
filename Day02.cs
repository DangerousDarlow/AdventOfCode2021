using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace AdventOfCode2021;

public class Day02
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day02(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Day02_Example.txt", 15, 10)]
    [InlineData("Day02.txt", 1931, 953)]
    public async Task Day02_Part1(string inputPath, int expectedHorizontal, int expectedDepth)
    {
        var inputs = await ReadInputs(inputPath);

        var horizontal = 0;
        var depth = 0;
        inputs.ToList().ForEach(input =>
        {
            switch (input.Command)
            {
                case "forward":
                    horizontal += input.Value;
                    return;
                case "down":
                    depth += input.Value;
                    return;
                case "up":
                    depth -= input.Value;
                    return;
                default: throw new Exception($"Illegal input command {input.Command}");
            }
        });

        (horizontal, depth).Should().Be((expectedHorizontal, expectedDepth));
        _testOutputHelper.WriteLine((horizontal * depth).ToString());
    }

    [Theory]
    [InlineData("Day02_Example.txt", 15, 60)]
    [InlineData("Day02.txt", 1931, 894762)]
    public async Task Day02_Part2(string inputPath, int expectedHorizontal, int expectedDepth)
    {
        var inputs = await ReadInputs(inputPath);

        var horizontal = 0;
        var depth = 0;
        var aim = 0;
        inputs.ToList().ForEach(input =>
        {
            switch (input.Command)
            {
                case "forward":
                    horizontal += input.Value;
                    depth += aim * input.Value;
                    return;
                case "down":
                    aim += input.Value;
                    return;
                case "up":
                    aim -= input.Value;
                    return;
                default: throw new Exception($"Illegal input command {input.Command}");
            }
        });

        (horizontal, depth).Should().Be((expectedHorizontal, expectedDepth));
        _testOutputHelper.WriteLine((horizontal * depth).ToString());
    }

    private static async Task<IEnumerable<Input>> ReadInputs(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(line =>
        {
            var s = line.Split(' ');
            return new Input {Command = s[0], Value = int.Parse(s[1])};
        });
    }
}

public record Input
{
    public string? Command { get; init; }
    public int Value { get; init; }
}