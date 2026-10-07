using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(ILogger<UserRoleController> logger, IUserRoleRepository userRoleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserRoles")]
        public async Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var userRoles = await userRoleRepository.ReadAsync() ?? [];
            return userRoles.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var userRole = await userRoleRepository.FindAsync(id);
            return UserRoleDTO.ConvertFrom(userRole);
        }
    }
}
