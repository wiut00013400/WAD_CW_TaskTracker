using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using WAD13400.Data;
using WAD13400.Models;

namespace WAD13400.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskTrackerDbContext _context;

        public TaskItemsController(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.Tasks.Include(t => t.Project).ToListAsync();
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var taskItem = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        // PUT: api/TaskItems
        [HttpPut]
        public async Task<IActionResult> PutTaskItem(TaskItem taskItem)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == taskItem.ProjectId);
            if (!projectExists)
            {
                return BadRequest("The specified ProjectId does not exist.");
            }
            _context.Entry(taskItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!TaskItemExists(taskItem.Id))
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

        // POST: api/TaskItems
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            var p = await _context.Projects
                .Include(p => p.Tasks) // Make sure Tasks collection is loaded
                .FirstOrDefaultAsync(p => p.Id == taskItem.ProjectId);

            if (p != null)
            {
                p.Tasks.Add(taskItem);
                _context.Entry(p).State = EntityState.Modified;
            }
            else
            {
                return BadRequest("The specified ProjectId does not exist.");
            }

            _context.Tasks.Add(taskItem);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
