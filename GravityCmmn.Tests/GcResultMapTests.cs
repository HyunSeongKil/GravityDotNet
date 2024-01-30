using System.Diagnostics;
using GravityCmmn.Misc;
using Xunit.Abstractions;

namespace GravityCmmn.Tests;

public class GcResultMapTests(ITestOutputHelper outputHelper)
{
  [Fact]
  public void Test1()
  {
    var resultMap = GcResultMap.Empty();
    outputHelper.WriteLine(resultMap.ToString());

    Assert.True(true);
  }

  // test emtpy
  [Fact]
  public void Empty()
  {
    var resultMap = GcResultMap.Empty();
    Assert.True(resultMap.Data.GetType() == new Dictionary<string, object>().GetType());
    Assert.True(((Dictionary<string, object>)resultMap.Data).Count == 0);
  }
}