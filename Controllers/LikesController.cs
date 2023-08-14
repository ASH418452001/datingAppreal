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
       
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepostory _userRepostory;

        public LikesController( ILikesRepository likesRepository, IUserRepostory userRepostory)
        {
            
            _likesRepository = likesRepository;
            _userRepostory = userRepostory;
        }


     

      
        [HttpPost ]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            
            var likedUser = await _userRepostory.GetUserByNameAsync(username);
            
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();
            
            if (sourceUser.UserName == username) return BadRequest("You can not like yourself");
            
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            
            if (userLike != null) return BadRequest("you  already likke this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };
            
            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepostory.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like User");
        }

        // PUT api/<LikesController>/5
        [HttpGet]
        public async Task<ActionResult<PagedList<LikesDtO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
           
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage
                , users.PageSize, users.TotalCount, users.TotalPages));
            
            return Ok(users);
        }

        // DELETE api/<LikesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
