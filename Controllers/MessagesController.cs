using AutoMapper;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepostory _userRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepostory userRepository, IMessagesRepository messagesRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MessagesDtO>> CreateMessage(CreatemessageDto createmessageDto)
        {
            var username = User.GetUsername();

            if (username == createmessageDto.RecipientUsername.ToLower())
                return BadRequest("you can not send message to yourself");
            var sender = await _userRepository.GetUserByNameAsync(username);

            var recipient = await _userRepository.GetUserByNameAsync(createmessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createmessageDto.Content
            };
            _messagesRepository.AddMessage(message);

            if (await _messagesRepository.SaveAllAsync()) return Ok(_mapper.Map<MessagesDtO>(message));
            return BadRequest("Failed to send the message");
        }

        // PUT api/<MessagesController>/5
        [HttpGet]
        public async Task<ActionResult<PagedList<MessagesDtO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messagesRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalPages, messages.TotalCount));
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessagesDtO>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messagesRepository.GetMessageThread(currentUsername, username));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            
            var message = await _messagesRepository.GetMessage(id);
            
            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            if (message.SenderUsername == username) message.SenderDeleted = true;
            
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message.SenderDeleted & message.RecipientDeleted)
            {
                _messagesRepository.DeleteMessage(message);
            }

            if (await _messagesRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem to saving the message"); ;
        }
    }
}
