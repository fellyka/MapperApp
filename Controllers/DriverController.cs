using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MapperApp.Models;
using MapperApp.Models.DTOs.Incoming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MapperApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly ILogger<DriverController> _logger;
        private static List<Driver> drivers = new List<Driver>();
        private readonly IMapper _mapper;

        public DriverController(ILogger<DriverController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

       //Get All drivers
       [HttpGet]
       public IActionResult GetDrivers()
       {
           var allDrivers = drivers.Where( x=> x.Status == 1).ToList();
           return Ok(allDrivers);
       }

       //Post A driver
       [HttpPost]
       public IActionResult CreateDriver(DriverForCreationDto driver)
       {
         if(ModelState.IsValid)
         {
            var _driver = _mapper.Map<Driver>(driver);
            
            drivers.Add(_driver);
            return CreatedAtAction("GetDriver",new {_driver.Id}, _driver);
         }
         //In case soething goes wrong
         return new JsonResult("Something went wrong") {StatusCode = 500 };
       }

       [HttpGet("{id}")]
       [Route("GetDriver")]
       public IActionResult GetDriver(Guid id)
       {
         var item = drivers.FirstOrDefault(x=>x.Id == id);
         if(item == null)
            return NotFound();
          return Ok(item);
       }

       [HttpPut("{id}")]
       public IActionResult UpdateDriver(Guid id, Driver driver)
       {
        if(id == driver.Id)
           return BadRequest();

        var existingDriver = drivers.FirstOrDefault(x=>x.Id == driver.Id);
        if(existingDriver == null)
          return NotFound();

        existingDriver.DriverNumber = driver.DriverNumber;
        existingDriver.FirstName = driver.FirstName;
        existingDriver.LastName = driver.LastName;
        existingDriver.WorldChampionship = driver.WorldChampionship;

        return NoContent();
       }

       [HttpDelete("{id}")]
       public IActionResult DeleteDriver(Guid id)
       {
        var existingDriver = drivers.FirstOrDefault(x=>x.Id == id);
        if(existingDriver == null)
          return NotFound();

        existingDriver.Status = 0;
        //return Ok(existingDriver);

        return NoContent();
       }

    }
}