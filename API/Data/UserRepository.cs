using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users 
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async  Task<PagedList<MemberDto>> GetMembersAsync(UsersParams usersParams)
    {
        var query = context.Users.AsQueryable();

        // Filter out current user
        query = query.Where(x => x.UserName != usersParams.CurrentUserName);

        // Filter by age if specified
        if(usersParams.Gender != null)
        {
            query = query.Where(x => x.Gender == usersParams.Gender);
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-usersParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-usersParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
        query = usersParams.OrderBy switch 
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)   
        };
            
        return await PagedList<MemberDto>
            .CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), 
            usersParams.PageNumber, usersParams.PageSize);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
        .Include(x => x.Photos)
        .ToListAsync();   
    }

    public async Task<AppUser?> GetUsersByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUsersByUsernameAsync(string username)
    {
        return await context.Users
        .Include(x => x.Photos) 
        .SingleOrDefaultAsync(item => item.UserName == username); 
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;  
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
