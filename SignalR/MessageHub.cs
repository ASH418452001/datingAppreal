﻿using AutoMapper;
using datingAppreal.Data;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.SignalR;

namespace datingAppreal.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;

        public MessageHub(IUnitOfWork uow,IMapper mapper, IHubContext<PresenceHub> presenceHub)
            
        {
            _uow = uow;
            _mapper = mapper;
            _presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            
            var otherUser = httpContext.Request.Query["user"];

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _uow.MessagesRepository
                .GetMessageThread(Context.User.GetUsername(), otherUser);

            if (_uow.HasChanges()) await _uow.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
           var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }



        public async Task SendMessage(CreatemessageDto createmessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createmessageDto.RecipientUsername.ToLower())
               throw new HubException("you can not send message to yourself");
            var sender = await _uow.UserRepostory.GetUserByNameAsync(username);

            var recipient = await _uow.UserRepostory.GetUserByNameAsync(createmessageDto.RecipientUsername);

            if (recipient == null) throw new HubException("not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createmessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _uow.MessagesRepository.GetMessageGroup(groupName);

            if(group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DAteRead = DateTime.UtcNow;
            }

            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if(connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("New Message Received",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }
            
            _uow.MessagesRepository.AddMessage(message);

            if (await _uow.Complete())
            {
                

                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessagesDtO>(message));
            }
                
                
                
        }


        private string GetGroupName(string caller,string other)
        {
            var stringCompare = string.CompareOrdinal(caller,other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _uow.MessagesRepository.GetMessageGroup(groupName);

            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());
            
            if(group == null)
            {
                group = new Group(groupName);
                _uow.MessagesRepository.AddGroup(group);      
            }
            group.Connections.Add(connection);

            if(await _uow.Complete()) return group;

            throw new HubException("Failed to add to group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _uow.MessagesRepository.GetGroupForConnection(Context.ConnectionId);
            
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            
            _uow.MessagesRepository.RemoveConnection(connection);
            
            if(await _uow.Complete()) return group;

            throw new HubException("Failed to remove from group");
        }
    }
}