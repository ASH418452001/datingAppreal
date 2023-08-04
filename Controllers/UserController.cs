using datingAppreal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datingAppreal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // just authred member could use these endpoints
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext  context)
        {
            _context = context;
        }
        // GET: api/<UserController>
       
        [AllowAnonymous]  // to allow anyone to get in this endpoint
        [HttpGet]
        public async Task< ActionResult<IEnumerable<object>>> Get()
        {
             var ass= await _context.User.ToListAsync();
            return ass;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(int id)
        {
            return await _context.User.FindAsync(id); ; 
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
