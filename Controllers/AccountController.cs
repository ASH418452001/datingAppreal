using AutoMapper;
using datingAppreal.Data;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [ServiceFilter(typeof(LogUserAcivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(DataContext context , ITokenServices tokenServices, IMapper mapper)
        {
            _context = context;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }



        // POST api/<AccountController>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register([FromQuery] RegisterDtO registerDtO)
        {
            if (await UserExist(registerDtO.Username)) return BadRequest("this username been taken");

            var user = _mapper.Map<User>(registerDtO); 

            using var hmac = new HMACSHA512();


            user.UserName = registerDtO.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtO.Password));
            user.PasswordSalt = hmac.Key;

           
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,  
                Gender = user.Gender
            };

        }


       private async Task<bool> UserExist(string username)
        {
            return await _context.User.AnyAsync(x => x.UserName==username.ToLower());
        }




        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromQuery] LoginDtO loginDtO)
        {
            var user = await _context.User.SingleOrDefaultAsync(x=>x.UserName==loginDtO.Username);
            if (user == null) return Unauthorized("invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDtO.Password));
            for (int i =0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            return new UserDto
                {
                Username = user.UserName,
                Token = _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
