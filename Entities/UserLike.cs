namespace API.Entities;

public class UserLike
{
  public AppUser? SourceUser { get; set; }
  public int SourceUserId { get; set; }
  public AppUser? LikedUser { get; set; }
  public int LikedUserId { get; set; }
  public List<UserLike>? LikedByUsers { get; set; }
  public List<UserLike>? LikedUsers { get; set; }
}