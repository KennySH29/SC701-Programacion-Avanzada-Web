using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(ILogger<TaskController> logger, ITaskRepository taskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetTasks")]
        public async Task<IEnumerable<TaskDTO>> GetAll()
        {
            var tasks = await taskRepository.ReadAsync() ?? [];
            return tasks.Select(TaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskDTO>> GetById(int id)
        {
            var task = await taskRepository.FindAsync(id);
            return TaskDTO.ConvertFrom(task);
        }
    }
}
