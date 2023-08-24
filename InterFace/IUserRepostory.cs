using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;

namespace datingAppreal.InterFace
{
    public interface IUserRepostory
    {
        void Update(User user);
       
        Task<IEnumerable<User>> GetUsersAsync(); 
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<PagedList<MemberDtO>> GetMembersAsync(UserParams userParams);
        Task <MemberDtO> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);

    }
}
