using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;

namespace datingAppreal.InterFace
{
    public interface IUserRepostory
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync(); 
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<PagedList<MemberDtO>> GetMembersAsync(UserParams userParams);
        Task <MemberDtO> GetMemberAsync(string username);

    }
}
