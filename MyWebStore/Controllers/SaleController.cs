using Microsoft.AspNetCore.Mvc;
using MyWebStore.DTOs;
using MyWebStore.Services;

namespace MyWebStore.Controllers
{
    public class SaleController(SaleServices _service) : Controller
    {
        public IActionResult AddEdit()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody]SaleDTO request)
        {
            try
            {
                _service.Create(request);
                return Ok(new { message = "Sale saved successfully!" });
            }
            catch (Exception ex) { 
                return StatusCode(500,new {message = ex.Message});
            }
            
        }

        [HttpPut]
        public IActionResult Edit([FromBody] SaleDTO request)
        {
            try
            {
                _service.Edit(request);
                return Ok(new { message = "Sale modified successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int SaleId)
        {
            try
            {
                _service.Delete(SaleId);
                return Ok(new { message = "Sale removed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult Get([FromQuery]int SaleId)
        {
            try
            {
                return Ok(_service.Get(SaleId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
    }
}
