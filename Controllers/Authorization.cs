//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IdentityModel.Tokens.Jwt;

//namespace Graduation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Authorization : ControllerBase
//    {
//        public void Authorize(String jwt)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var token = handler.ReadJwtToken(jwt);

//            var claims = token.Claims;
//            //return claims;
//        }
//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Graduation.Controllers
//{
//    public class ApiController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public ApiController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var token = context.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

//            if (string.IsNullOrEmpty(token))
//            {
//                context.Result = Unauthorized();
//                return;
//            }

//            try
//            {
//                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
//                var tokenHandler = new JwtSecurityTokenHandler();
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
//                var accountType = jwtToken.Claims.First(x => x.Type == "AccountType").Value;

//                // You can access userId and accountType in the authorized API handler
//                context.HttpContext.Items["UserId"] = userId;
//                context.HttpContext.Items["AccountType"] = accountType;
//            }
//            catch (Exception)
//            {
//                context.Result = Unauthorized();
//                return;
//            }

//            base.OnActionExecuting(context);
//        }
//    }
//}
