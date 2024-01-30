using System.Diagnostics;
using GravityCmmn.Misc;
using Xunit.Abstractions;

namespace GravityCmmn.Tests;

public class GcUtilsTest(ITestOutputHelper outputHelper)
{
    // readonly ITestOutputHelper outputHelper = outputHelper;

    // test uuid8 결과가 8자리인지 검사    
    [Fact]
    public void Uuid8()
    {
        string str = GcUtils.Uuid8();
        Assert.True(str.Length == 8);
    }

    // test uuid12 결과가 12자리인지 검사
    [Fact]
    public void Uuid12()
    {
        string str = GcUtils.Uuid12();
        Assert.True(str.Length == 12);
    }

    // test uuid12 start with letter
    [Fact]
    public void Uuid12StartWithLetter()
    {
        string str = GcUtils.Uuid12();
        Assert.True(char.IsLetter(str[0]));
    }

    // test camel string to kebab string
    [Fact]
    public void CamelToKebab()
    {
        string str = GcUtils.CamelToKebab("CamelToKebab");
        Assert.True(str == "camel-to-kebab");
    }

    // test camel string to snake string
    [Fact]
    public void CamelToSnake()
    {
        string str = GcUtils.CamelToSnake("CamelToSnake");
        Assert.True(str == "camel_to_snake");
    }

    // test kebab string to camel string
    [Fact]
    public void KebabToCamel()
    {
        string str = GcUtils.KebabToCamel("kebab-to-camel");
        Assert.True(str == "kebabToCamel");
    }

    // test snake string to camel string
    [Fact]
    public void SnakeToCamel()
    {
        string str = GcUtils.SnakeToCamel("snake_to_camel");
        Assert.True(str == "snakeToCamel");
    }

    // test GetTotalMemorySize
    [Fact]
    public void GetTotalMemorySize()
    {
        long size = GcUtils.GetTotalMemorySize();
        outputHelper.WriteLine($"size: {size} byte(s)");
        Assert.True(size > 0);
    }

    // test GetCpuCoreCount
    [Fact]
    public void GetCpuCoreCount()
    {
        int count = GcUtils.GetCpuCoreCount();
        outputHelper.WriteLine($"count: {count}");
        Assert.True(count > 0);
    }

    // test GetDiskFreeSpace
    [Fact]
    public void GetDiskFreeSpace()
    {
        string path = "c:\\";
        long size = GcUtils.GetDiskFreeSpace(path);
        outputHelper.WriteLine($"path: {path}\tfree size: {size / 1024 / 1024} MByte(s)");
        Assert.True(size > 0);
    }

    // test GetOsInfo
    [Fact]
    public void GetOsInfo()
    {
        IDictionary<string, string> dic = GcUtils.GetOsInfo();
        outputHelper.WriteLine($"info: {dic}");
        Assert.True(dic.ContainsKey("OSName"));
        Assert.True(dic.ContainsKey("OSArch"));
        Assert.True(dic.ContainsKey("Framework"));
    }

    // test ToJson
    [Fact]
    public void ToJson()
    {
        Dictionary<string, string> dic = new(){
            {"OSName", "windows"},
        };
        string json = GcUtils.ToJson(dic);
        outputHelper.WriteLine($"json: {json}");
        Assert.Contains("OSName", json);
        Assert.Contains("windows", json);
    }

}