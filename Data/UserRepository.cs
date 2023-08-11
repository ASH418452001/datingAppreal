using AutoMapper;
using AutoMapper.QueryableExtensions;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;
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
        public async Task<PagedList<MemberDtO>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.User.AsQueryable();
            //query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);
            var MinDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
            var MaxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= MinDob && u.DateOfBirth <= MaxDob);

            query = userParams.OrderBy switch
            {
                "Created" => query.OrderByDescending(u => u.Created),
                 _  => query.OrderByDescending(u => u.LastActive)  //there is under score in the beginning of the  line , notice it please
            };

            return await PagedList<MemberDtO>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDtO>
                (_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
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
