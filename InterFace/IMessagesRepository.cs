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
    
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);   
        Task <Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);

    }
}
