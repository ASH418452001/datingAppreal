using AutoMapper;
using AutoMapper.QueryableExtensions;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.InterFace;
using Microsoft.EntityFrameworkCore;

namespace datingAppreal.Data
{
    public class UserRepository : IUserRepostory
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context , IMapper mapper)
        {
            _context = context;
           _mapper = mapper;
        }

        public async Task<MemberDtO> GetMemberAsync(string username)
        {
            return await _context.User.Where(x => x.UserName == username)
                .ProjectTo<MemberDtO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<MemberDtO>> GetMembersAsync()
        {
            return await _context.User.
                ProjectTo<MemberDtO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
           return await _context.User.FindAsync(id);
        }

        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _context.User.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.UserName==username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.User.Include(p=>p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0 ;
        }

        public void Update(User user)
        {
           _context.Entry(user).State=EntityState.Modified;
        }
    }
}
