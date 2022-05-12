namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IFriendshipRequestRepository FriendshipRequestRepository { get; }
        IFriendshipRepository FriendshipRepository { get; }
        IMessageRepository MessageRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}