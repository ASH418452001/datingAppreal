
using datingAppreal.Entities;

namespace datingAppreal.InterFace
{
    public interface ITokenServices
    {

       Task<string> CreateToken(User user);
       
    }
}
