namespace API.Helpers;

public class CreateMessageDto : PaginationParams
{
  public string RecipientUsername { get; set; }
  public string Content { get; set; } = "Unread";
}