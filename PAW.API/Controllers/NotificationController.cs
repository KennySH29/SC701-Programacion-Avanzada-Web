using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(
        ILogger<NotificationController> logger,
        INotificationRepository notificationRepository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var notifications =
                await notificationRepository.ReadAsync() ?? [];

            return notifications.Select(
                NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var notification =
                await notificationRepository.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return Ok(
                NotificationDTO.ConvertFrom(notification));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create(
            [FromBody] NotificationDTO notificationDTO)
        {
            var notification =
                NotificationDTO.ConvertTo(notificationDTO);

            notification.Id = 0;

            notification.IsRead =
                notificationDTO.IsRead ?? false;

            notification.CreatedAt =
                notificationDTO.CreatedAt ?? DateTime.Now;

            var result =
                await notificationRepository.CreateAsync(
                    notification);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(
            int id,
            [FromBody] NotificationDTO notificationDTO)
        {
            var notification =
                await notificationRepository.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.UserId =
                notificationDTO.UserId;

            notification.Message =
                notificationDTO.Message;

            notification.IsRead =
                notificationDTO.IsRead ?? false;

            // CreatedAt NO se modifica.

            var result =
                await notificationRepository.UpdateAsync(
                    notification);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var notification =
                await notificationRepository.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            var result =
                await notificationRepository.DeleteAsync(
                    notification);

            return Ok(result);
        }
    }
}
