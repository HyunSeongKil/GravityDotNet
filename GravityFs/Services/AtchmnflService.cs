using GravityFs.Domains;
using GravityFs.Models;

namespace GravityFs.Services;

public class AtchmnflService() : IAtchmnflService
{

  string IAtchmnflService.Regist(GravityfsdbContext dbContext, AtchmnflDto dto)
  {
    string id = dto.AtchmnflId ?? IAtchmnflService.CreateId();

    dbContext.Atchmnfls.Add(new Atchmnfl
    {
      AtchmnflId = id,
      AtchmnflGroupId = dto.AtchmnflGroupId,
      ContentType = dto.ContentType,
      FileSize = dto.FileSize,
      OriginalFilename = dto.OriginalFilename,
      SaveFilename = dto.SaveFilename,
      SaveSubPath = dto.SaveSubPath,
      RegisterId = dto.RegisterId,
      RegistDt = DateTime.Now,
    });


    return id;
  }
}