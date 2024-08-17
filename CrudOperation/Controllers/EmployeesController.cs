using CrudOperation.Data;
using CrudOperation.Models;
using CrudOperation.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperation.Controllers
{
    // localhost:XXXX/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppicationDbContext dbContext;

        public EmployeesController(AppicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployee = dbContext.Employees.ToList();

            return Ok(allEmployee);


        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employeeEntity = dbContext.Employees.Find(id);

            if (employeeEntity is null)
            {
                return NotFound();
            
            }
            return Ok(employeeEntity);

        }

        [HttpPost]
        public IActionResult AddEmployees(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary

            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            var uri = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{employeeEntity.Id}");

            return Created(uri, employeeEntity);


        }



      }
    }
