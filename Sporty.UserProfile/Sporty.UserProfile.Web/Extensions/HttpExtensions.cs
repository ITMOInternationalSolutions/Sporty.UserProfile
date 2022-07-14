namespace Sporty.UserProfile.Web.Extensions;

public static class HttpExtensions
{
    public static Guid GetIdFromItems(HttpContext httpContext)
    {
        var userIdItem = httpContext.Items["SenderUserId"];

        if (userIdItem is null)
        {
            throw new Exception("Incorrect user id item");
        }

        return (Guid)userIdItem;
    }
}