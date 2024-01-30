using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GravityCmmn.Misc;

public static class GcUtils
{

  #region uuid
  public static string Uuid8() => Guid.NewGuid().ToString().Replace("-", "")[..8];


  public static string Uuid12(bool startWithLetter = true)
  {
    if (!startWithLetter)
    {
      return Guid.NewGuid().ToString().Replace("-", "")[..12];
    }

    while (true)
    {
      var uuid = Guid.NewGuid().ToString();
      if (!char.IsDigit(uuid[0]))
      {
        return uuid.Replace("-", "")[..12];
      }
    }
  }
  #endregion uuid

  #region  문자열 변환

  // camel string to kebab string
  public static string CamelToKebab(string str) => Regex.Replace(str, "(?<!^)([A-Z])", "-$1").ToLower();

  // camel string to snake string
  public static string CamelToSnake(string str) => Regex.Replace(str, "(?<!^)([A-Z])", "_$1").ToLower();

  // kebab string to camel string
  public static string KebabToCamel(string str) => Regex.Replace(str, "-(.)", m => m.Groups[1].Value.ToUpper());

  // snake string to camel string
  public static string SnakeToCamel(string str) => Regex.Replace(str, "_(.)", m => m.Groups[1].Value.ToUpper());
  #endregion 문자열 변환


  #region 시스템 정보
  // get total memory size
  public static long GetTotalMemorySize() => Process.GetCurrentProcess().PrivateMemorySize64;

  // get cpu core count
  public static int GetCpuCoreCount() => Environment.ProcessorCount;

  // get disk free space
  public static long GetDiskFreeSpace(string path)
  {
    DriveInfo drive = new(path);
    return drive.AvailableFreeSpace;
  }

  // get os info
  public static IDictionary<string, string> GetOsInfo()
  {
    return new Dictionary<string, string>
    {
      {"OSName", RuntimeInformation.OSDescription},
      {"OSArch", RuntimeInformation.OSArchitecture.ToString()},
      {"Framework", RuntimeInformation.FrameworkDescription}
    };
  }
  #endregion 시스템 정보

  #region  기타
  public static string ToJson(object obj)
  {
    return JsonSerializer.Serialize(obj);

  }
  #endregion 기타


}