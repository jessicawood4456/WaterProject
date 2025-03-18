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
        public IActionResult GetProjects(int pageSize = 5, int pageNum = 1)
        {
            var projects = _context.Projects
                .Skip((pageNum -1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalNumProjects = _context.Projects.Count();

            var returnObject = new
            {
                Projects = projects,
                TotalNumProjects = totalNumProjects
            };

            return Ok(returnObject);
        }

        [HttpGet("FunctionalProjects")]
        public IEnumerable<Project> GetFunctionalProjects()
        {
            var projects = _context.Projects.Where(p => p.ProjectFunctionalityStatus == "Functional").ToList();

            return projects;
        }
    }
}
