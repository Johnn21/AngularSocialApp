namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IFriendshipRequestRepository FriendshipRequestRepository { get; }
        IFriendshipRepository FriendshipRepository { get; }
        IMessageRepository MessageRepository { get; }
        IPostRepository PostRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}