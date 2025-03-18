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
        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects.ToList();
        }

        [HttpGet("FunctionalProjects")]
        public IEnumerable<Project> GetFunctionalProjects()
        {
            var projects = _context.Projects.Where(p => p.ProjectFunctionalityStatus == "Functional").ToList();

            return projects;
        }
    }
}
