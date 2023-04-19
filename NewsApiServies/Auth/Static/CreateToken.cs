using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsApiDomin.Models;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Auth.ClassStatic
{
    public class CreateToken
    {

        public static async Task<AuthModel> CreateJwtToken(AuthModel authModel, JWT? _jwt)
        {

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, authModel.Id.ToString()!));
            claims.Add(new Claim(ClaimTypes.Surname, authModel.DisplayName!));
            claims.Add(new Claim(ClaimTypes.Email, authModel.Email!));
            claims.Add(new Claim(ClaimTypes.Role, authModel.Roles!));


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt!.Key!));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt!.Issuer,
                audience: _jwt!.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt!.DurationInDays),
                signingCredentials: signingCredentials);
            return await Task.Run(() =>
                new AuthModel
                {
                    Message = "Success",
                    Id = authModel.Id,
                    Email = authModel.Email,
                    DisplayName = authModel.DisplayName,
                    Roles = authModel.Roles,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),


                }
            );
        }

        public static RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
