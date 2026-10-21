using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;


namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserController(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users =
                await _userRepository.ReadAsync() ?? [];

            return users.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user =
                await _userRepository.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(UserDTO.ConvertFrom(user));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create(
            [FromBody] UserDTO userDTO)
        {
            var user =
                UserDTO.ConvertTo(userDTO);

            user.UserId = 0;

            user.CreatedAt =
                DateTime.Now;

            user.LastModified =
                DateTime.Now;

            // HASH DE LA CONTRASEÑA
            if (!string.IsNullOrWhiteSpace(userDTO.Password))
            {
                user.PasswordHash =
                    _passwordHasher.HashPassword(
                        user,
                        userDTO.Password);
            }

            var result =
                await _userRepository.CreateAsync(user);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(
            int id,
            [FromBody] UserDTO userDTO)
        {
            var user =
                await _userRepository.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username =
                userDTO.Username;

            user.Email =
                userDTO.Email;

            user.IsActive =
                userDTO.IsActive;

            user.RoleId =
                userDTO.RoleId;

            user.ModifiedBy =
                userDTO.ModifiedBy;

            user.LastModifiedBy =
                userDTO.LastModifiedBy;

            user.LastModified =
                DateTime.Now;

            /*
             * Si el usuario escribió una nueva contraseña,
             * generar un nuevo hash.
             *
             * Si dejó Password vacío,
             * conservar el PasswordHash existente.
             */
            if (!string.IsNullOrWhiteSpace(userDTO.Password))
            {
                user.PasswordHash =
                    _passwordHasher.HashPassword(
                        user,
                        userDTO.Password);
            }

            var result =
                await _userRepository.UpdateAsync(user);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var user =
                await _userRepository.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result =
                await _userRepository.DeleteAsync(user);

            return Ok(result);
        }
    }
}
