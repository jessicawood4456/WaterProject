using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterProject.Data;

namespace WaterProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private WaterDbContext _context;
        
        public WaterController(WaterDbContext temp)
        {
            _context = temp;
        }

        [HttpGet("AllProjects")]
        public IActionResult GetProjects(int pageSize = 5, int pageNum = 1, [FromQuery] List<string>? projectTypes = null)
        {
            //string? favProjType = Request.Cookies["FavoriteProjectType"];
            //Console.WriteLine("~~~~Cookie~~~~\n" + favProjType);
            
            HttpContext.Response.Cookies.Append("FavoriteProjectType", "Borehole Well and Hand Pump", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(5),
            });

            var query = _context.Projects.AsQueryable();

            if (projectTypes != null && projectTypes.Any() )
            {
                query = query.Where(p => projectTypes.Contains(p.ProjectType));
            }

            var totalNumProjects = query.Count();

            var projects = query
                .Skip((pageNum -1) * pageSize)
                .Take(pageSize)
                .ToList();

            var returnObject = new
            {
                Projects = projects,
                TotalNumProjects = totalNumProjects
            };

            return Ok(returnObject);
        }

        [HttpGet("GetProjectTypes")]
        public IActionResult GetProjectTypes()
        {
            var projectTypes = _context.Projects
                .Select(p => p.ProjectType)
                .Distinct()
                .ToList();

            return Ok(projectTypes);
        }

        [HttpPost("addProject")]
        public IActionResult AddProject([FromBody] Project newProject)
        {
            _context.Projects.Add(newProject);
            _context.SaveChanges();
            return Ok(newProject);
        }

        [HttpPut("updateProject/{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] Project updatedProject)
        {
            var existingProject = _context.Projects.Find(projectId);
            
            existingProject.ProjectName = updatedProject.ProjectName;
            existingProject.ProjectType = updatedProject.ProjectType;
            existingProject.ProjectRegionalProgram = updatedProject.ProjectRegionalProgram;
            existingProject.ProjectImpact = updatedProject.ProjectImpact;
            existingProject.ProjectPhase = updatedProject.ProjectPhase;
            existingProject.ProjectFunctionalityStatus = updatedProject.ProjectFunctionalityStatus;
            
            _context.Projects.Update(existingProject);
            _context.SaveChanges();

            return Ok(existingProject);
        }

        [HttpDelete("deleteProject/{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            var project = _context.Projects.Find(projectId);

            if (project == null)
            {
                return NotFound(new {message = "Project nout found"});
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
