using datingAppreal.Data;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [ServiceFilter(typeof(LogUserAcivity))]

    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public LikesController(IUnitOfWork uow )
        {
            _uow = uow;
        }


     

      
        [HttpPost ]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            
            var likedUser = await _uow.UserRepostory.GetUserByNameAsync(username);
            
            var sourceUser = await _uow.LikesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            
            if (sourceUser.UserName == username) return BadRequest("You can not like yourself");
            
            var userLike = await _uow.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);
            
            if (userLike != null) return BadRequest("you  already likke this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };
            
            sourceUser.LikedUsers.Add(userLike);

            if (await _uow.Complete()) return Ok();
            return BadRequest("Failed to like User");
        }

        // PUT api/<LikesController>/5
        [HttpGet]
        public async Task<ActionResult<PagedList<LikesDtO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
           
            var users = await _uow.LikesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage
                , users.PageSize, users.TotalCount, users.TotalPages));
            
            return Ok(users);
        }

       
    }
}
