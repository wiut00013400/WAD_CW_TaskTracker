using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAD13400.Data;
using WAD13400.Models;

namespace WAD13400.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectItemsController : ControllerBase
    {
        private readonly TaskTrackerDbContext _context;

        public ProjectItemsController(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectItem>>> GetProjects()
        {
            return await _context.Projects.Include(p => p.Tasks).ToListAsync();
        }

        // GET: api/ProjectItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectItem>> GetProjectItem(int id)
        {
            //var projectItem = await _context.Projects.FindAsync(id);
            var projectItem = await _context.Projects.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == id);
            if (projectItem == null)
            {
                return NotFound();
            }

            return projectItem;
        }

        // PUT: api/ProjectItems
        [HttpPut]
        public async Task<IActionResult> PutProjectItem(ProjectItem projectItem)
        {
            _context.Entry(projectItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ProjectItemExists(projectItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectItems
        [HttpPost]
        public async Task<ActionResult<ProjectItem>> PostProjectItem(ProjectItem projectItem)
        {
            _context.Projects.Add(projectItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectItem", new { id = projectItem.Id }, projectItem);
        }

        // DELETE: api/ProjectItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectItem(int id)
        {
            var projectItem = await _context.Projects.FindAsync(id);
            if (projectItem == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projectItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectItemExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
