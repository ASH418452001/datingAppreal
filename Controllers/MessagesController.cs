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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MessagesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<MessagesDtO>> CreateMessage(CreatemessageDto createmessageDto)
        {
            var username = User.GetUsername();

            if (username == createmessageDto.RecipientUsername.ToLower())
                return BadRequest("you can not send message to yourself");
            var sender = await _uow.UserRepostory.GetUserByNameAsync(username);

            var recipient = await _uow.UserRepostory.GetUserByNameAsync(createmessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createmessageDto.Content
            };
            _uow.MessagesRepository.AddMessage(message);

            if (await _uow.Complete()) return Ok(_mapper.Map<MessagesDtO>(message));
            return BadRequest("Failed to send the message");
        }

        // PUT api/<MessagesController>/5
        [HttpGet]
        public async Task<ActionResult<PagedList<MessagesDtO>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _uow.MessagesRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalPages, messages.TotalCount));
            return messages;
        }

     
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            
            var message = await _uow.MessagesRepository.GetMessage(id);
            
            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            if (message.SenderUsername == username) message.SenderDeleted = true;
            
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message.SenderDeleted & message.RecipientDeleted)
            {
                _uow.MessagesRepository.DeleteMessage(message);
            }

            if (await _uow.Complete()) return Ok();
            return BadRequest("problem to saving the message"); ;
        }
    }
}
