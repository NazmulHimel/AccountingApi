using Acc.Context;
using Acc.Domain.Entities.DataModel;
using Acc.Domain.Entities.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Acc.HelpersAndUtilities.JwtSecurity
{
    public class JwtSecurityService : IJwtSecurityService
    {
        private readonly JwtTokenManagement _jwtTokenManagement;
        private readonly SqlServerContext _sqlServerContext;
        public JwtSecurityService(IOptions<JwtTokenManagement> appSettings, SqlServerContext sqlServerContext)
        {
            _jwtTokenManagement = appSettings.Value;
            _sqlServerContext = sqlServerContext;
        }

        public User GenerateJwtAdmin(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenManagement.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim("ApplicationName","pblaccounting")
                }),
                Issuer = _jwtTokenManagement.Issuer,
                Audience = _jwtTokenManagement.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtTokenManagement.AccessExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.AccessToken = tokenHandler.WriteToken(token);
            user.RefreshToken = Guid.NewGuid().ToString();
            // remove password before returning
            user.Password = null;

            // update Refreshtoken
            var entity = _sqlServerContext.User.FirstOrDefault(item => item.UserId == user.UserId);
            entity.RefreshToken = user.RefreshToken;
            entity.AccessToken = user.AccessToken;
            _sqlServerContext.User.Update(entity);
            _sqlServerContext.SaveChanges();
            return user;
        }
    }
}
