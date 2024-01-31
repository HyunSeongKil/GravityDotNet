using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GravityCmmn.Misc;
using GravityFs.Domains;
using GravityFs.Misc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace GravityFs.Controllers;

[ApiController]
[Route("defaults/")]
public class DefaultController(IConfiguration config) : ControllerBase
{
  readonly Logger logger = LogManager.GetCurrentClassLogger();

  [HttpPut]
  [Route("login")]
  [AllowAnonymous]
  public GcResultMap Login([FromBody] UserDto userDto)
  {
    logger.Debug(GcUtils.ToJson(userDto));

    // TODO 로그인 처리

    //
    string? secretKey = config.GetValue<string>("App:SecretKey") ?? throw new Exception("App:SecretKey is null");
    string issuer = config.GetValue<string>("App:Issuer") ?? string.Empty;
    string audience = config.GetValue<string>("App:Audience") ?? string.Empty;


    //
    IDictionary<string, object> bodyDic = new Dictionary<string, object>()
    {
      {"userId", "aha1492"},
      {"userName", "김철수"},
    };

    //
    string accessToken = JwtUtil.CreateToken(secretKey, issuer, audience, bodyDic, 20);
    string refreshToken = JwtUtil.CreateToken(secretKey, issuer, audience, bodyDic, 60);
    logger.Debug("token: {}", accessToken);
    logger.Debug("pricipal: {}", JwtUtil.DecodeToken(accessToken, secretKey, issuer, audience));


    //
    return GcResultMap.WithData(new Dictionary<string, string>()
    {
      ["accessToken"] = accessToken,
      ["refreshToken"] = refreshToken,
    });

  }
}