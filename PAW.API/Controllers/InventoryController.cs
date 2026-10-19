using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository inventoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetInventories")]
        public async Task<IEnumerable<InventoryDTO>> GetAll()
        {
            var inventories = await inventoryRepository.ReadAsync() ?? [];
            return inventories.Select(InventoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory == null)
                return NotFound();
            return Ok(InventoryDTO.ConvertFrom(inventory));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] InventoryDTO inventoryDTO)
        {
            var inventory = InventoryDTO.ConvertTo(inventoryDTO);
            inventory.InventoryId = 0; // Ensure the InventoryId is set to 0 for new inventory items
            if (inventory.DateAdded == null || inventory.DateAdded == default(DateTime))
            {
                inventory.DateAdded = DateTime.Now;
            }
            inventory.LastUpdated = DateTime.Now;
            var result = await inventoryRepository.CreateAsync(inventory);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            var result = await inventoryRepository.DeleteAsync(inventory);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] InventoryDTO inventoryDTO)
        {
            var inventory = await inventoryRepository.FindAsync(id);

            if (inventory == null)
                return NotFound();

            inventory.UnitPrice = inventoryDTO.UnitPrice;
            inventory.UnitsInStock = inventoryDTO.UnitsInStock;
            inventory.ProductId = inventoryDTO.ProductId;
            inventory.ModifiedBy = inventoryDTO.ModifiedBy;
            inventory.LastUpdated = DateTime.Now;

            var result = await inventoryRepository.UpdateAsync(inventory);

            return Ok(result);
        }
    }
}
