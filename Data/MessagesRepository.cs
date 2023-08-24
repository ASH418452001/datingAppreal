using AutoMapper;
using AutoMapper.QueryableExtensions;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.EntityFrameworkCore;

namespace datingAppreal.Data
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessagesRepository(DataContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
           _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
          _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
          _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();

        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessagesDtO>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();
            query = messageParams.Continer switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.RecipientDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted && u.DAteRead==null)
            };
            var messages = query.ProjectTo<MessagesDtO>(_mapper.ConfigurationProvider);
            return await PagedList<MessagesDtO>
                .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessagesDtO>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var query = _context.Messages
                
                .Where(
                    m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false &&
                    m.SenderUsername == recipientUserName ||
                    m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
                    m.SenderUsername == currentUserName
                ).OrderByDescending(m => m.MessageSent).AsQueryable();

            var unreadMessages = query.Where(m => m.DAteRead == null
            && m.RecipientUsername == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DAteRead = DateTime.UtcNow;
                }
                
            }
            return await query.ProjectTo<MessagesDtO>(_mapper.ConfigurationProvider).ToListAsync();

        }

        public void RemoveConnection(Connection connection)
        {
           _context.Connections.Remove(connection);
        }

      
    }
}
