using System.Text.Json;

namespace GravityCmmn.Misc;

public class GcResultMap : Dictionary<string, object>
{
  public object Data
  {
    get => this["data"];
    set => this["data"] = value;
  }

  public string? Code
  {
    get => this["code"] != null ? this["code"].ToString() : string.Empty;
    set => this["code"] = value!;
  }

  public string? Message
  {
    get => this["message"] != null ? this["message"].ToString() : string.Empty;
    set => this["message"] = value!;
  }

  public int Page
  {
    get => this["page"] != null ? (int)this["page"] : 0;
    set => this["page"] = value;
  }

  public int Size
  {
    get => this["size"] != null ? (int)this["size"] : 0;
    set => this["size"] = value;
  }

  public long TotalCount
  {
    get => this["totalCount"] != null ? (long)this["totalCount"] : 0;
    set => this["totalCount"] = value;
  }

  public string? Sort
  {
    get => this["sort"] != null ? this["sort"].ToString() : string.Empty;
    set => this["sort"] = value!;
  }

  public GcResultMap()
  {
    Data = new Dictionary<string, object>();
    Code = string.Empty;
    Message = string.Empty;
    Page = 0;
    Size = 10;
    TotalCount = 0;
  }


  public static GcResultMap WithData(object data)
  {
    GcResultMap result = new()
    {
      Data = data ?? new Dictionary<string, object>(),
    };

    return result;
  }

  public static GcResultMap WithCode(string code, string? message)
  {
    GcResultMap result = new()
    {
      Data = new Dictionary<string, object>(),
      Code = code,
      Message = message
    };


    return result;
  }

  public static GcResultMap Empty()
  {
    return new();
  }

  public override string ToString()
  {
    return JsonSerializer.Serialize(this);
  }
}