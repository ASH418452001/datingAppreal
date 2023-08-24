using datingAppreal.Data;
using datingAppreal.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }
        // GET: api/<BuggyController>
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }
        [HttpGet("not-found")]
        public ActionResult<User> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return thing;
        }

        [HttpGet("Server-Error")]
        public ActionResult<string> GetServererror()
        {
          
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
            
       
           
        }

        [HttpGet("Bad-Request")]
        public ActionResult<string> GetBadRequest()
        {
           return  BadRequest("This was invalid REQUEST");
        }


    }
}
