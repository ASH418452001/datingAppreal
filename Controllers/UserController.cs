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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork uow ,IMapper mapper )
        {
            _uow = uow;
            _mapper = mapper;
        }
        

        /*[AllowAnonymous] */ // to allow anyone to get in this endpoint

        
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDtO>>> Get([FromQuery] UserParams userParams)
        {
            var gender = await _uow.UserRepostory.GetUserGender(User.GetUsername());
            userParams.CurrentUsername = User.GetUsername();


            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = gender =="male"?"female" :"male";
            }

           var user =  await _uow.UserRepostory.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(user.CurrentPage,user.PageSize,user.TotalPages,user.TotalCount)); 
          
            return Ok(user);

           
        }


       
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDtO>> Get(string username)
        {
            return await _uow.UserRepostory.GetMemberAsync(username) ; 
        }


      
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromQuery] MemberUpdateDtO memberUpdateDtO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _uow.UserRepostory.GetUserByNameAsync(username);
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDtO, user);
            if (await _uow.Complete()) return NoContent();
            return BadRequest("Failed to update user");
        }


    }
}
