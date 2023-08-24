using AutoMapper;
using datingAppreal.Data;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [ServiceFilter(typeof(LogUserAcivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager , ITokenServices tokenServices, IMapper mapper)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }



        // POST api/<AccountController>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register([FromQuery] RegisterDtO registerDtO)
        {
            if (await UserExist(registerDtO.Username)) return BadRequest("this username been taken");

            var user = _mapper.Map<User>(registerDtO); 

      


            user.UserName = registerDtO.Username.ToLower();


            var result = await _userManager.CreateAsync(user, registerDtO.Password); ;
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);
           
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,  
                Gender = user.Gender
            };

        }


       private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName==username.ToLower());
        }




        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromQuery] LoginDtO loginDtO)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x=>x.UserName==loginDtO.Username);
            
            if (user == null) return Unauthorized("invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDtO.Password);

            if (!result) return Unauthorized("invalid Password");

            return new UserDto
                {
                Username = user.UserName,
                Token = await _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

       
    }
}
