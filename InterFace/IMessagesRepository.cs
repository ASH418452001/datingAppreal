using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;

namespace datingAppreal.InterFace
{
    public interface IMessagesRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);    
        Task<Message> GetMessage(int id);
        Task<PagedList<MessagesDtO>> GetMessageForUser(MessageParams messageParams); 
        Task<IEnumerable<MessagesDtO>> GetMessageThread(string currentUserName, string recipientUserName); 
        Task<bool> SaveAllAsync();
    }
}
