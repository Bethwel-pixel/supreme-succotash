using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JozyKQL.PG.Services.DynamicService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using succotashBE.data.Functions;
using succotashBE.data.Model;
using succotashBE.data.Views;

namespace succotashBE.Controllers
{
    [ApiController]
    [Route("")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? EncKey;
        private readonly string? JwtKey;
        private readonly string? JwtIssuer;
        private readonly string? JwtAudience;

        public LoginController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            EncKey = _configuration["EncKey:Key"];
            JwtKey = _configuration["JwtSettings:Key"];
            JwtIssuer = _configuration["JwtSettings:Issuer"];
            JwtAudience = _configuration["JwtSettings:Audience"];
            _httpContextAccessor = httpContextAccessor;
        }

        private Userinfo GetUserInfo()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
            var roleIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("RoleId");
            var roleGroupIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("CountryId");
            var countryIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("CountyId");
            var subcountuString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("SubCountryId");


            return new Userinfo
            {
                UserId = int.TryParse(userIdString, out int userId) ? userId : null,
                RoleId = int.TryParse(roleIdString, out int roleId) ? roleId : null,
                CountryId = int.TryParse(roleGroupIdString, out int roleGroupId) ? roleGroupId : null,
                CountyId = int.TryParse(countryIdString, out int countyId) ? countyId : null,
                SubcountyId = int.TryParse(subcountuString, out int subcountyId) ? subcountyId : null,
            };
        }

        private DynamicService CreatedDynamicService
        {
            get
            {
                return new DynamicService(_configuration.GetConnectionString("ConnectionString")!);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(String usernameoremail, string? HashedPassword)
        {
            FetchUserbyusername fetchuser = new FetchUserbyusername
            {
                Username = usernameoremail
            };

            UsersView user = new UsersView();
            UsersView? retrievedUser = await CreatedDynamicService.ExecuteFunctionWithInputDynamically(fetchuser, user);

            if (retrievedUser == null)
            {
                // user.HashedPassword = null; 
                string errorMessage = $"User with email/username {usernameoremail} doesnt exist";
                return Unauthorized("Invalid Credentials");
            }


            bool authorized = VerifyPassword(HashedPassword!, retrievedUser.Password!);
            
            if (!authorized)
            {
                string errorMessage = "Wrong password";
                await Task.Delay(3000);

                return Unauthorized("Invalid Credentials");
            }

            else
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(JwtKey!);
                    var issuer = JwtIssuer;
                    var audience = JwtAudience;


                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, "Pypy"),
                        new Claim(ClaimTypes.Name, retrievedUser.Username!),
                        new Claim(ClaimTypes.Email, retrievedUser.Email!),
                        new Claim("Id", retrievedUser.Id?.ToString() ?? ""),
                        new Claim("CountryId", retrievedUser.Countryid.ToString() ?? ""),
                        new Claim("CountyId", retrievedUser.Countyid.ToString() ?? ""),
                        new Claim("SubCountyId", retrievedUser.Subcountyid.ToString() ?? ""),
                        new Claim("Username", retrievedUser.Username ?? ""),
                        new Claim("RoleId", retrievedUser.Roleid?.ToString() ?? ""),

                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = issuer,
                        Audience = audience,
                        Subject = new ClaimsIdentity(claims.ToArray()),
                        Expires = DateTime.UtcNow.AddHours(8),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };


                    var token = tokenHandler.CreateToken(tokenDescriptor);


                    var tokenString = tokenHandler.WriteToken(token);

                    var json = new
                    {
                        token = tokenString,
                        expireDate = "",
                        expiresIn = "28800"
                    };
                    return Ok(json);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                }
            }



        }

        private bool VerifyPassword(String? value1, String? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            string inputPassword = value1.ToString() ?? string.Empty;
            string hashedPassword = value2.ToString() ?? string.Empty;

            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }

        [HttpPost("SignUp")]
        public async Task<FEOutput?> SignUp(Users users)
        {
            try
            {
                if (users.Username == null)
                {
                    return new FEOutput { Error = JsonConvert.SerializeObject("Username cannot be null") };
                }

                users.Password = EncryptUser(users.Password);

                var results = await CreatedDynamicService.CreateDynamically(users);
                return new FEOutput { Results = JsonConvert.SerializeObject(results) };
            }
            catch (Exception ex)
            {
                return new FEOutput { Error = JsonConvert.SerializeObject(ex) };
            }
        }

        // hashing the password
        private string EncryptUser(string? password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
    
    }


}

