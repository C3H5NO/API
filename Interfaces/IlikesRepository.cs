using API.Entities;

namespace API.DTOs;

public interface IlikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

    Task<AppUser> GetUser(int userId);

    Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
}