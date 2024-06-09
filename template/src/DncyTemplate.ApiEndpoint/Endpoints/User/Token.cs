using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DncyTemplate.ApiEndpoint.Constants;
using DncyTemplate.Application.Models;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace DncyTemplate.ApiEndpoint.Endpoints.User
{

    /// <summary>
    /// token 端点
    /// </summary>
    public class Token : Endpoint<TokenRequest, ResultDto>, IResponseWraps
    {
        public override void Configure()
        {
            Post("/token");
            AllowAnonymous();
            Summary(s => s.Description = "获取token");
        }

        public override async Task<ResultDto> ExecuteAsync(TokenRequest req, CancellationToken ct)
        {
            await Task.Delay(111, cancellationToken: ct);

            var u = users.FirstOrDefault(x => x.UserName == req.UserName && x.Password == req.Password);
            if (u == null)
            {
                return ResultDto.Error("user not exist!!!");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, u.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, u.Id.ToString()),
                new Claim(ClaimTypes.Role, u.Role),
                new Claim(JwtRegisteredClaimNames.Aud,"a"),
                new Claim(JwtRegisteredClaimNames.Aud,"b"),
                new Claim(JwtRegisteredClaimNames.Aud,"c"),
#if Tenant
                new Claim(AppConstant.TENANT_KEY, u.TenantId)
#endif
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"));

            var token = new JwtSecurityToken(
                issuer: "pluto",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(120),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );


            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return this.Success(new
            {
                access_token = jwtToken,
            });
        }

        #region virtual user

        private static readonly List<dynamic> users =
        [
            new
            {
                Id = 1,
                Mobile = "18530064433",
                UserName = "admin3",
                Password = "admin",
                Role = "admin",
#if Tenant
                TenantId = "T20210602000003"
#endif
            },
            new
            {
                Id = 2,
                Mobile = "18530064432",
                UserName = "admin2",
                Password = "admin",
                Role = "admin",
#if Tenant
                TenantId = "T20210602000002"
#endif
            },
            new
            {
                Id = 3,
                Mobile = "18530064431",
                UserName = "sa",
                Password = "admin",
                Role = "SystemAdmin",
#if Tenant
                TenantId = "T20210602000001"
#endif
            }
        ];

        #endregion
    }



    public record TokenRequest(string UserName, string Password);


    public class MyValidator : Validator<TokenRequest>
    {
        public MyValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName is required!")
                .MinimumLength(5)
                .WithMessage("UserName is too short!");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required!")
                .MinimumLength(5)
                .WithMessage("Password is too short!")
                .MaximumLength(12)
                .WithMessage("Password is too long!");
        }
    }
}
