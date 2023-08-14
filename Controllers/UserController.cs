using AutoMapper;
using datingAppreal.Data;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [ServiceFilter(typeof(LogUserAcivity))]

    [Route("api/[controller]")]
    [ApiController]
    /*[Authorize]*/ // just authred member could use these endpoints
    public class UserController : ControllerBase
    {
        private readonly IUserRepostory _userRepostory;
        private readonly IMapper _mapper;

        public UserController(IUserRepostory userRepostory ,IMapper mapper )
        {
            _userRepostory = userRepostory;
            _mapper = mapper;
        }
        // GET: api/<UserController>
       
        /*[AllowAnonymous] */ // to allow anyone to get in this endpoint
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDtO>>> Get([FromQuery] UserParams userParams)
        {
            var currentUser = await _userRepostory.GetUserByNameAsync(User.GetUsername());
            userParams.CurrentUsername = currentUser.UserName;


            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender =="male"?"female" :"male";
            }

           var user =  await _userRepostory.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(user.CurrentPage,user.PageSize,user.TotalPages,user.TotalCount)); 
          
            return Ok(user);

           
        }


        // GET api/<UserController>/5
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDtO>> Get(string username)
        {
            return await _userRepostory.GetMemberAsync(username) ; //How to  make return just to what i need , like username ?
            
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromQuery] MemberUpdateDtO memberUpdateDtO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepostory.GetUserByNameAsync(username);
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDtO, user);
            if (await _userRepostory.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
