using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await roleRepository.ReadAsync() ?? [];
            return roles.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            return RoleDTO.ConvertFrom(role);
        }
    }
}
