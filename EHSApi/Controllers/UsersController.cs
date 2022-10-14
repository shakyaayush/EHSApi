using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EHSApi.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EHSApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace EHSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EHSDbContext _context;
        private readonly IConfiguration config;

        public UsersController(EHSDbContext context, 
            IConfiguration config)
        {
            _context = context;
            this.config = config;

         }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        // POST: api/Users/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check if username and password are matching or not
            var result = await _context.Users.FirstOrDefaultAsync(
                             u => u.UserName == user.UserName
                             && u.Password == user.Password);
            if (result == null) // login failed
            {
                return NotFound();
            }
            //login is success and generate the JWT

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, result.Id.ToString()),
                    new Claim(ClaimTypes.Role, result.Role),
                };
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Secret"]));
            var issuer = config["Issuer"];
            var audience = config["Audience"];
            var expiryDays = Convert.ToInt32(config["ExpiryDays"]);

            var token = new JwtSecurityToken(issuer, audience, authClaims,
                expires: DateTime.Now.AddDays(expiryDays),
                signingCredentials: new SigningCredentials(authSigningKey,
                SecurityAlgorithms.HmacSha256));
            //username, role and token
            var dto = new LoginDto
            {
                UserName = result.UserName,
                Role = result.Role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            return Ok(dto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
