using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly ILogger<ComponentController>
            _logger;

        private readonly IComponentRepository
            _componentRepository;

        public ComponentController(
            ILogger<ComponentController> logger,
            IComponentRepository componentRepository)
        {
            _logger = logger;
            _componentRepository =
                componentRepository;
        }


        // GET: /Component
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>>
            GetAll()
        {
            var components =
                await _componentRepository
                    .ReadAsync() ?? [];

            return components.Select(
                ComponentDTO.ConvertFrom);
        }


        // GET: /Component/1
        [HttpGet("{id:decimal}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>>GetById(decimal id)
        {
            var component =
                await _componentRepository.FindByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return Ok(
                ComponentDTO.ConvertFrom(component));
        }


        // POST: /Component
        [HttpPost]
        public async Task<ActionResult<bool>> Create(
            [FromBody] ComponentDTO componentDTO)
        {
            var component =
                ComponentDTO.ConvertTo(
                    componentDTO);

            // ID generado por SQL
            component.Id = 0;

            var result =
                await _componentRepository
                    .CreateAsync(component);

            return Ok(result);
        }


        // PUT: /Component/1
        [HttpPut("{id:decimal}")]
        public async Task<ActionResult<bool>> Update(
            decimal id,
            [FromBody] ComponentDTO componentDTO)
        {
            var component =
                await _componentRepository
                    .FindByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            component.Name =
                componentDTO.Name;

            component.Content =
                componentDTO.Content;

            var result =
                await _componentRepository
                    .UpdateAsync(component);

            return Ok(result);
        }


        // DELETE: /Component/1
        [HttpDelete("{id:decimal}")]
        public async Task<ActionResult<bool>> Delete(
            decimal id)
        {
            var component =
                await _componentRepository
                    .FindByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            var result =
                await _componentRepository
                    .DeleteAsync(component);

            return Ok(result);
        }
    }
}
