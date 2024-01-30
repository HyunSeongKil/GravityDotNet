using System.ComponentModel.DataAnnotations;

namespace GravityFs.Domains;

public class AtchmnflDto
{
  public string AtchmnflId { get; set; }
  public string AtchmnflGroupId { get; set; }
  public string RegisterId { get; set; }
  public DateTime RegistDt { get; set; }
  public string OriginalFilename { get; set; }
  public string SaveFilename { get; set; }
  public string SaveSubPath { get; set; }
  public string ContentType { get; set; }
  public int FileSize { get; set; }


}