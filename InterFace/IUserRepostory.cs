using datingAppreal.DTOs;
using datingAppreal.Entities;

namespace datingAppreal.InterFace
{
    public interface IUserRepostory
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync(); 
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<IEnumerable<MemberDtO>> GetMembersAsync();
        Task <MemberDtO> GetMemberAsync(string username);

    }
}
