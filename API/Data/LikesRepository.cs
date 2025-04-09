namespace API.Data;

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like) => context.Likes.Add(like);
    public void RemoveLike(UserLike like) => context.Likes.Remove(like);
    
    public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUSerId)
        => await context.Likes
            .Where(l => l.SourceUserId == currentUSerId)
            .Select(l => l.TargetUserId)
            .ToListAsync();

    public async Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targerUserId)
        => await context.Likes.FindAsync(sourceUserId, targerUserId);

     public async Task<PagedList<MemberReponse>> GetUserLikesAsync(LikesParams likesParams)
    {
        var likes = context.Likes.AsQueryable();
        IQueryable<MemberReponse> query;

        switch (likesParams.Predicate.ToLower(CultureInfo.InvariantCulture))
        {
            case "liked":
                query = likes
                    .Where(l => l.SourceUserId == likesParams.UserId)
                    .Select(l => l.TargetUser)
                    .ProjectTo<MemberReponse>(mapper.ConfigurationProvider);
                break;
            case "likedby":
                query = likes
                    .Where(l => l.TargetUserId == likesParams.UserId)
                    .Select(l => l.SourceUser)
                    .ProjectTo<MemberReponse>(mapper.ConfigurationProvider);
                break;
            default:
                var likeIds = await GetCurrentUserLikeIdsAsync(likesParams.UserId);
                query = likes
                    .Where(l => l.TargetUserId == likesParams.UserId && likeIds.Contains(l.SourceUserId))
                    .Select(l => l.SourceUser)
                    .ProjectTo<MemberReponse>(mapper.ConfigurationProvider);
                break;
        }

        return await PagedList<MemberReponse>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
}