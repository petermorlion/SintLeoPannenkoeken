﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Users;
using System.Security.Cryptography;
using System.Text;

namespace SintLeoPannenkoeken.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Dictionary<string, string> RoleIds = new Dictionary<string, string>
        {
            { "admin", "Admin" },
            { "financieploeg", "FinanciePloeg" },
            { "bestuurder", "Bestuurder" }
        };

        public UsersController(
            ILogger<UsersController> logger, 
            ApplicationDbContext dbContext, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var users = _dbContext.Users.ToList();
            
            
            var userViewModels = users.Select(async user =>
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return new UserViewModel(user, roles);
                }).Select(t => t.Result).ToList();

            return Ok(userViewModels);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel createUserViewModel)
        {
            var user = new IdentityUser(createUserViewModel.Email);
            user.Email = createUserViewModel.Email;
            user.EmailConfirmed = true;

            var randomPassword = GetRandomPassword();
            // TODO: check for result?
            var userResult = await _userManager.CreateAsync(user, randomPassword);
            if (!userResult.Succeeded)
            {
                // TODO: handle error
                return BadRequest(userResult.Errors);
            }

            foreach (var role in createUserViewModel.Roles)
            {
                var roleId = RoleIds[role];
                var roleExists = await _roleManager.RoleExistsAsync(roleId);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleId));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, roleId);
                if (!roleResult.Succeeded)
                {
                    // TODO: handle error
                    return BadRequest(roleResult.Errors);
                }
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var passwordResetLink = Url.Action("ResetPassword", "Account", new { code = encodedCode, email = user.Email, Area = "Identity" }, Request.Scheme);

            var userCreatedViewModel = new UserCreatedViewModel(user, passwordResetLink);

            return Created($"/api/users/{user.Id}", userCreatedViewModel);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        private string GetRandomPassword()
        {
            var length = 32;
            var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890&é'(§è!çà)";
            var secret = new StringBuilder();
            while (length-- > 0)
            {
                secret.Append(alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)]);
            }

            return secret.ToString();
        }
    }
}
