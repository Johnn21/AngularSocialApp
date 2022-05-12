using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<MemberDto> GetMemberByUsernameAsync(string username);
        Task<AppUser> GetUserByUsernameAsync(string username);
        void Update(AppUser user);
    }
}