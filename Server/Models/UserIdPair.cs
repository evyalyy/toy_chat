namespace Server.Models;

public class UserIdPair
{
    private readonly long _userId1;

    private readonly long _userId2;

    public UserIdPair(long userId1, long userId2)
    {
        (_userId1, _userId2) = SortIds(userId1, userId2);
    }

    public long UserId1()
    {
        return _userId1;
    }

    public long UserId2()
    {
        return _userId2;
    }
    
    private static (long, long) SortIds(long userId1, long userId2)
    {
        return userId1 < userId2 ? (userId1, userId2) : (userId2, userId1);
    }
}