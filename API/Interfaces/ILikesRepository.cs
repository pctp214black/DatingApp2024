using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface ILikesRepository
{
    public Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId);
    public Task<IEnumerable<MemberReponse>> GetUserLikesAsync(string predicate, int userId);
    public Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUserId);
    public void RemoveLike(UserLike like);
    public void AddLike(UserLike like);
    public Task<bool> SaveChangesAsync();
}
