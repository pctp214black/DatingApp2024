namespace API.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllAsync() => await context.Users.Include(u => u.Photos).ToListAsync();
    public async Task<AppUser?> GetByIdAsync(int id) => await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
    public async Task<AppUser?> GetByUsernameAsync(string username) => await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.UserName == username);
    public async Task<IEnumerable<MemberReponse>> GetMembersAsync() => await context.Users.ProjectTo<MemberReponse>(mapper.ConfigurationProvider).ToListAsync();
    public async Task<MemberReponse?> GetMemeberAsync(string username) => await context.Users.Where(u => u.UserName == username).ProjectTo<MemberReponse>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;
    public void Update(AppUser user) => context.Entry(user).State = EntityState.Modified;
}