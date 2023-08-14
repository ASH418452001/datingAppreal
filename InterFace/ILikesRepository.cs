using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;

namespace datingAppreal.InterFace
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<User> GetUserWithLikes(int userId);

        Task<PagedList<LikesDtO>> GetUserLikes(LikesParams likesParams);
    }

}
