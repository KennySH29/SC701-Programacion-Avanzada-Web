using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;
using Tasks = System.Threading.Tasks;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(
        ILogger<TaskController> logger,
        ITaskRepository taskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetTasks")]
        public async Tasks.Task<IEnumerable<TaskDTO>> GetAll()
        {
            var tasks = await taskRepository.ReadAsync() ?? [];

            return tasks.Select(TaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Tasks.Task<ActionResult<TaskDTO>> GetById(int id)
        {
            var task = await taskRepository.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(TaskDTO.ConvertFrom(task));
        }

        [HttpPost]
        public async Tasks.Task<ActionResult<bool>> Create(
            [FromBody] TaskDTO taskDTO)
        {
            var task = TaskDTO.ConvertTo(taskDTO);

            task.Id = 0;

            if (task.CreatedAt == null ||
                task.CreatedAt == default(DateTime))
            {
                task.CreatedAt = DateTime.Now;
            }

            task.LastModified = DateTime.Now;

            var result =
                await taskRepository.CreateAsync(task);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Tasks.Task<ActionResult<bool>> Update(
            int id,
            [FromBody] TaskDTO taskDTO)
        {
            var task =
                await taskRepository.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Name = taskDTO.Name;
            task.Description = taskDTO.Description;
            task.Status = taskDTO.Status;
            task.DueDate = taskDTO.DueDate;
            task.ModifiedBy = taskDTO.ModifiedBy;

            task.LastModified = DateTime.Now;

            var result =
                await taskRepository.UpdateAsync(task);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Tasks.Task<ActionResult<bool>> Delete(int id)
        {
            var task =
                await taskRepository.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var result =
                await taskRepository.DeleteAsync(task);

            return Ok(result);
        }
    }
}