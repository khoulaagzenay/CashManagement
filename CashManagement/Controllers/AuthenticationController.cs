using CashManagement.Data.UnitOfWork;
using CashManagement.Models.DTOs.Auth;
using CashManagement.Models.Entities.Authentication;
using CashManagement.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AuthenticationController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                return BadRequest("User already exists!");
            }
            AppUser NewUser = new AppUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(NewUser, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest("User creation failed! Please check user details and try again.");
            }
            switch(model.Role)
            {
                case "Admin":
                    await _userManager.AddToRoleAsync(NewUser, "Admin");
                    break;
                case "User":
                    await _userManager.AddToRoleAsync(NewUser, "User");
                    break;
                default:
                    await _userManager.AddToRoleAsync(NewUser, "User");
                    break;
            }
            return Created(nameof(Register), "User Created Succefully");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await GenerateJwtToken(user);
                return Ok(token);
            }
            return Unauthorized();
        }
        //Generate JWT Token
        private async Task<AuthResultDto> GenerateJwtToken(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email),
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, userRole));
            }
            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var RefreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                IsRevoked = false,
                DateCreated = DateTime.UtcNow,
                DateExpire = DateTime.Now.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "" + Guid.NewGuid().ToString()
            };
            await _unitOfWork.RefreshTokens.AddAsync(RefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var response = new AuthResultDto
            {
                Token = JwtToken,
                RefreshToken = RefreshToken.Token,
                ExpiredAt = token.ValidTo
            };
            return response;
        }
    }
}
