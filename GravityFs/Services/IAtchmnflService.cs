using GravityCmmn.Misc;
using GravityFs.Domains;
using GravityFs.Models;

namespace GravityFs.Services;

public interface IAtchmnflService
{
  static string CreateId()
  {
    return "file" + GcUtils.Uuid8();
  }

  string Regist(GravityfsdbContext dbContext, AtchmnflDto dto);
}