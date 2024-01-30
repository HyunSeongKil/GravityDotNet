using System;
using System.Collections.Generic;

namespace GravityFs.Models;

public partial class Atchmnfl
{
    public string AtchmnflId { get; set; } = null!;

    public string AtchmnflGroupId { get; set; } = null!;

    public string? OriginalFilename { get; set; }

    public string? SaveFilename { get; set; }

    public string? SaveSubPath { get; set; }

    public int? FileSize { get; set; }

    public string? ContentType { get; set; }

    public string? RegisterId { get; set; }

    public DateTime? RegistDt { get; set; }

    public string? UpdaterId { get; set; }

    public DateTime? UpdateDt { get; set; }
}
