using GravityFs.Domains;
using GravityFs.Models;


namespace GravityFs.Services;

public class AtchmnflGroupService(ILogger<AtchmnflGroupService> logger) : IAtchmnflGroupService
{


  public string Regist(GravityfsdbContext dbContext, AtchmnflGroupDto dto)
  {
    string id = dto.AtchmnflGroupId ?? IAtchmnflGroupService.CreateId();
    var atchmnflGroup = new AtchmnflGroup
    {
      AtchmnflGroupId = id,
      BizKey = dto.BizKey,
      RegisterId = dto.RegisterId,
      RegistDt = DateTime.Now,
    };

    dbContext.AtchmnflGroups.Add(atchmnflGroup);



    return id;
  }
}