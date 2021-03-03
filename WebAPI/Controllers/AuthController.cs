using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyToken(String token)
        {
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;

            try
            {
                var response = await auth.GetUserAsync(token);
                if (response != null)
                    return Accepted();
            }
            catch (FirebaseException ex)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("updateRole")]
        public async Task<IActionResult> UpdateRole(String uid)
        {
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            Dictionary<String, Object> claims = new Dictionary<string, object>();
            claims.Add("admin", true);
            await auth.SetCustomUserClaimsAsync(uid, claims);
            return Ok();
        }

        [HttpPost("getRole")]
        public async Task<IActionResult> GetRole(String uid)
        {
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            var user = await auth.GetUserAsync(uid);
            return Ok(user);
        }

        [HttpGet("secrets")]
        [Authorize]
        public async Task<IEnumerable<string>> GetSecrets(String token)
        {
            var User = HttpContext.User;
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            try
            {
                var response = await auth.VerifyIdTokenAsync(token);
                if (response != null)
                    return null;
            }
            catch (FirebaseException ex)
            {
                return null;
            }
            return new List<string>()
            {
                "This is from the secret controller",
                "Seeing this means you are authenticated",
                "You have logged in using your google account from firebase",
                "Have a nice day!!"
            };
        }
    }
}