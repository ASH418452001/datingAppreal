namespace datingAppreal.InterFace
{
    public interface IUnitOfWork
    {
        IUserRepostory UserRepostory { get; }
        IMessagesRepository MessagesRepository { get; } 
        ILikesRepository LikesRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
