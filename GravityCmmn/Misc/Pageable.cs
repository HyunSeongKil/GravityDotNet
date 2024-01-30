namespace GravityCmmn.Misc;

public class Pageable
{
  public int Page { get; set; }
  public int Size { get; set; }
  public string? Sort { get; set; }
  public string? Direction { get; set; }
}