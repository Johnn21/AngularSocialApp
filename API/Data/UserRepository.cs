using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public UserRepository(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberByUsernameAsync(string username)
        {
           return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           return await _context.Users
                .Where(x => x.UserName == username)
                .Include(p => p.Photos)
                .SingleOrDefaultAsync();
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}