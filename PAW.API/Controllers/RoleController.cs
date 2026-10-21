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
            if (role == null)
                return NotFound();
            return Ok(RoleDTO.ConvertFrom(role));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] RoleDTO roleDTO)
        {
            var role = RoleDTO.ConvertTo(roleDTO);
            role.RoleId = 0; // Ensure the RoleId is set to 0 for new roles
            var result = await roleRepository.CreateAsync(role);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var role = await roleRepository.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = await roleRepository.DeleteAsync(role);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] RoleDTO roleDTO)
        {
            var role = await roleRepository.FindAsync(id);

            if (role == null)
                return NotFound();

            role.RoleName = roleDTO.RoleName;

            var result = await roleRepository.UpdateAsync(role);

            return Ok(result);
        }
    }
}
