using System;
using System.Globalization;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like) => context.Likes.Add(like);
    public void RemoveLike(UserLike like) => context.Likes.Remove(like);
    public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUserId) => await context.Likes.Where(l => l.SourceUserId == currentUserId).Select(l => l.TargetUserId).ToListAsync();
    public async Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId) => await context.Likes.FindAsync(sourceUserId, targetUserId);
    public async Task<IEnumerable<MemberReponse>> GetUserLikesAsync(string predicate, int userId)
    {
        var likes = context.Likes.AsQueryable();
        switch (predicate.ToLower(CultureInfo.InvariantCulture))
        {
            case "liked":
                return await likes.Where(l => l.SourceUserId == userId).Select(l => l.TargetUserId).ProjectTo<MemberReponse>(mapper.ConfigurationProvider).ToListAsync();
            case "likedby":
                return await likes.Where(l => l.TargetUserId == userId).Select(l => l.SourceUserId).ProjectTo<MemberReponse>(mapper.ConfigurationProvider).ToListAsync();
            default:
                var likeIds = await GetCurrentUserLikeIdsAsync(userId);
                return await likes
                                .Where(l => l.TargetUserId == userId && likeIds.Contains(l.SourceUserId))
                                .Select(l => l.SourceUser).ProjectTo<MemberReponse>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
}
