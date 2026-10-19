using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var suppliers = await supplierRepository.ReadAsync() ?? [];
            return suppliers.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier == null)
                return NotFound();
            return Ok(SupplierDTO.ConvertFrom(supplier));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] SupplierDTO supplierDTO)
        {
            var supplier = SupplierDTO.ConvertTo(supplierDTO);
            supplier.SupplierId = 0; // Ensure the SupplierId is set to 0 for new suppliers
            supplier.LastModified = DateTime.Now;
            var result = await supplierRepository.CreateAsync(supplier);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            var result = await supplierRepository.DeleteAsync(supplier);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] SupplierDTO supplierDTO)
        {
            var supplier = await supplierRepository.FindAsync(id);

            if (supplier == null)
                return NotFound();

            supplier.SupplierName = supplierDTO.SupplierName;
            supplier.ContactName = supplierDTO.ContactName;
            supplier.ContactTitle = supplierDTO.ContactTitle;
            supplier.Phone = supplierDTO.Phone;
            supplier.Address = supplierDTO.Address;
            supplier.City = supplierDTO.City;
            supplier.Country = supplierDTO.Country;
            supplier.ModifiedBy = supplierDTO.ModifiedBy;
            supplier.LastModified = DateTime.Now;

            var result = await supplierRepository.UpdateAsync(supplier);

            return Ok(result);
        }
    }
}
