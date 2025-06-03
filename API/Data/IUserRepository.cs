namespace API.Data;

using API.Entities;
using API.DTOs;
using API.Helpers;

public interface IUserRepository
{
    public Task<IEnumerable<AppUser>> GetAllAsync();
    public Task<AppUser?> GetByIdAsync(int id);
    public Task<AppUser?> GetByUsernameAsync(string username);
    public Task<MemberReponse?> GetMemberAsync(string username);
    public Task<PagedList<MemberReponse>> GetMembersAsync(UserParams userParams);
    public void Update(AppUser user);
}