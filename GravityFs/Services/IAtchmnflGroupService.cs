using GravityCmmn.Misc;
using GravityFs.Domains;
using GravityFs.Models;

namespace GravityFs.Services;

public interface IAtchmnflGroupService
{
  static string CreateId()
  {
    return "fgrp" + GcUtils.Uuid8();
  }

  string Regist(GravityfsdbContext dbContext, AtchmnflGroupDto dto);
}