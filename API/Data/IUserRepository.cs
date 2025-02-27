namespace API.Data;

using API.DTOs;
using API.Entities;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetAllAsync();
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<IEnumerable<MemberReponse>> GetMembersAsync();
    Task<MemberReponse?> GetMemeberAsync(string username);
}