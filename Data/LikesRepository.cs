﻿using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.EntityFrameworkCore;

namespace datingAppreal.Data
{
    public class LikesRepository : ILikesRepository

    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync( sourceUserId,  targetUserId);
        }
        public async Task<PagedList<LikesDtO>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.User.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }

            if (likesParams.predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }
            var likedUsers = users.Select(user => new LikesDtO

            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });
            return await PagedList<LikesDtO>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        } 
            

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await _context.User.Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
