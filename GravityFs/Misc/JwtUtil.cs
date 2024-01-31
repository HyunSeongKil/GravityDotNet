using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GravityCmmn.Misc;
using Microsoft.IdentityModel.Tokens;

namespace GravityFs.Misc;

public class JwtUtil
{
  public static string CreateToken(string secretKey, string issuer, string audience, IDictionary<string, object> bodyDic, int expireMinutes = 60)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = bodyDic.ToList().Select(x =>
    {
      return new Claim(x.Key, x.Value.ToString());
    });


    var token = new JwtSecurityToken(
        issuer,
        audience,
        claims,
        expires: DateTime.Now.AddMinutes(expireMinutes),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public static IDictionary<string, string> DecodeToken(string token, string secretKey, string issuer, string audience)
  {
    var bodyDic = new Dictionary<string, string>();


    var pricipal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters()
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = issuer,
      ValidAudience = audience,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    }, out SecurityToken validatedToken);

    if (pricipal == null)
    {
      return bodyDic;
    }

    //
    pricipal.Claims.ToList().ForEach(x =>
    {
      bodyDic.Add(x.Type, x.Value);
    });

    //
    return bodyDic;
  }
}