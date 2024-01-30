using System;
using System.Collections.Generic;

namespace GravityFs.Models;

public partial class AtchmnflGroup
{
    public string AtchmnflGroupId { get; set; } = null!;

    public string BizKey { get; set; } = null!;

    public string RegisterId { get; set; } = null!;

    public DateTime RegistDt { get; set; }

    public string? UpdaterId { get; set; }

    public DateTime? UpdateDt { get; set; }
}
