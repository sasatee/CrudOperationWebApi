using AuthenticationAPI.Data;
using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        [HttpGet]
        public IActionResult GetAllProject()
        {
            var allProject = _dbContext.Projects.ToList();
            return Ok(allProject);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetProject(Guid id)
        {

            var projectEntity = _dbContext.Projects.Find(id);

            if (projectEntity is null)
            {
                return NotFound();



            }
            return Ok(projectEntity);

        }


        [HttpPost]
        public IActionResult CreateProject(ProjectDto projectDto)
        {
            var projectEntity = new Project()
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                EndDate = projectDto.EndDate,
                StartDate = projectDto.StartDate,
                TotalBudgetedHours = projectDto.TotalBudgetedHours,
            };

            _dbContext.Projects.Add(projectEntity);
            _dbContext.SaveChanges();

            var uri = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{projectEntity.Id}");
            return Created(uri, projectEntity);

        }
        //[HttpPut]
        //public IActionResult Update([FromBody] ProjectDto projectDto) 
        //{
        //  _dbContext.Update(projectDto);
        //   _dbContext.SaveChanges();

        //    var patched = _dbContext.Projects.FirstOrDefaultAsync(u => u.Id == projectDto.Id);

        //    var model = new
        //    {
        //        sent = projectDto,
        //        after = patched

        //    };
        //    return Ok(model);
        //}

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<Project>> UpdateProject(Guid id, Project project)
        {
            try
            {
               

                // Find the existing project
                var projectToUpdate = await _dbContext.Projects.FirstOrDefaultAsync(u => u.Id == id);
                if (projectToUpdate is null)
                {
                    return NotFound($"Project with Id = {id} not found");
                }

                // Update the properties of the existing project
                projectToUpdate.Name = project.Name;
                projectToUpdate.Description = project.Description;
                projectToUpdate.TotalBudgetedHours = project.TotalBudgetedHours;
                projectToUpdate.StartDate = project.StartDate;
                projectToUpdate.EndDate = project.EndDate;

                // Save changes 
                await _dbContext.SaveChangesAsync();

                return Ok(projectToUpdate); // updated project
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data: " + ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<ActionResult<Project>> PatchProject(Guid id, JsonPatchDocument<Project> patchDoc)
        {
            try
            {
                // Find the existing project by id 
                var projectToUpdate = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
                if (projectToUpdate is null)
                {
                    return NotFound($"Project with Id = {id} not found");
                }

             
                patchDoc.ApplyTo(projectToUpdate, ModelState);

                // Check if model state is valid after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(projectToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data: " + ex.Message);
            }
        }
        [HttpDelete]
        [Route("${id:guid}")]
        public async Task<ActionResult<Project>> DeleteProject(Guid id)
        {
            try
            {
                //find the existing project by id '
              

                var projectToDelete =  _dbContext.Projects.FirstOrDefault(x => x.Id == id);

                if (projectToDelete is null)
                {
                    return NotFound($"Project with Id = {id} not found");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _dbContext.Projects.RemoveRange();


                await _dbContext.SaveChangesAsync();

                return Ok($"Successful delete with project ID of {id}");


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data: " + ex.Message);

            }
        }


    }


}
