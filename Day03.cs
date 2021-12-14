using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2021;

public class Day03
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day03(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Day03_Example.txt", 22, 9)]
    [InlineData("Day03.txt", 217, 3878)]
    public async Task Part1(string inputPath, int expectedGamma, int expectedEpsilon)
    {
        var inputs = (await ReadInput(inputPath)).ToArray();
        var inputLength = inputs[0].Length;

        var modal = new BitArray(inputLength);
        for (var bitIndex = 0; bitIndex < inputLength; ++bitIndex)
        {
            var trueCount = inputs.Count(array => array[bitIndex]);
            modal[bitIndex] = trueCount >= inputs.Length - trueCount;
        }

        var gamma = modal.ToInt();
        var epsilon = modal.Not().ToInt();

        (gamma, epsilon).Should().Be((expectedGamma, expectedEpsilon));
        _testOutputHelper.WriteLine((gamma * epsilon).ToString());
    }

    [Theory]
    [InlineData("Day03_Example.txt", 23, 10)]
    [InlineData("Day03.txt", 1177, 4070)]
    public async Task Part2(string inputPath, int expectedOxygen, int expectedScrubber)
    {
        var inputs = (await ReadInput(inputPath)).ToArray();
        var oxygen = FilterInputsUsingCondition(inputs,
            (totalCount, trueCount) => trueCount >= totalCount - trueCount);
        
        var scrubber = FilterInputsUsingCondition(inputs,
            (totalCount, trueCount) => trueCount < totalCount - trueCount);
        
        (oxygen, scrubber).Should().Be((expectedOxygen, expectedScrubber));
        _testOutputHelper.WriteLine((oxygen * scrubber).ToString());
    }

    private delegate bool FilterCondition(int totalCount, int trueCount);

    private static int FilterInputsUsingCondition(BitArray[] inputs, FilterCondition filterCondition)
    {
        var inputLength = inputs[0].Length;
        for (var bitIndex = 0; bitIndex < inputLength; ++bitIndex)
        {
            if (inputs.Length == 1)
                break;

            var reverseBitIndex = inputLength - bitIndex - 1;
            var trueCount = inputs.Count(array => array[reverseBitIndex]);
            var filter = filterCondition(inputs.Length, trueCount);
            inputs = inputs.Where(array => array[reverseBitIndex] == filter).ToArray();
        }

        return inputs[0].ToInt();
    }

    private static async Task<IEnumerable<BitArray>> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(line =>
        {
            var bitArray = new BitArray(line.Length);
            for (var bitIndex = 0; bitIndex < line.Length; ++bitIndex)
            {
                // Reverse the bit order to match BitArray expectations
                if (line[line.Length - bitIndex - 1] == '1')
                    bitArray[bitIndex] = true;
            }

            return bitArray;
        });
    }
}

public static class BitArrayExtensions
{
    public static int ToInt(this BitArray array)
    {
        var value = new int[1];
        array.CopyTo(value, 0);
        return value[0];
    }
}