namespace API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;

public interface ILikesRepository
{
    public Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targerUserId);
    public Task<PagedList<MemberReponse>> GetUserLikesAsync(LikesParams likesParams);
    public Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUSerId);
    public void RemoveLike(UserLike like);
    public void AddLike(UserLike like);
    public Task<bool> SaveChangesAsync();

}