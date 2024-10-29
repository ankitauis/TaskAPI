using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskEntitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
        {
            var userId = User.FindFirst("UserID")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authorized to view tasks." });
            }

            return await _context.Tasks
                .Where(task => task.UserID == int.Parse(userId))
                .ToListAsync();
        }

        // GET: api/TaskEntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTaskEntity(int id)
        {
            var userId = User.FindFirst("UserID")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authorized to view this task." });
            }

            var taskEntity = await _context.Tasks
                .Where(task => task.UserID == int.Parse(userId) && task.Id == id)
                .FirstOrDefaultAsync();

            if (taskEntity == null)
            {
                return NotFound(new { Message = "Task not found or you do not have access." });
            }

            return taskEntity;
        }

        // PUT: api/TaskEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskEntity(int id, TaskEntity taskEntity)
        {
            var userId = User.FindFirst("UserID")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authorized to update this task." });
            }

            if (id != taskEntity.Id)
            {
                return BadRequest(new { Message = "Task ID mismatch." });
            }

            // Check if the task exists and belongs to the logged-in user
            var existingTask = await _context.Tasks
                .Where(task => task.UserID == int.Parse(userId) && task.Id == id)
                .FirstOrDefaultAsync();

            if (existingTask == null)
            {
                return NotFound(new { Message = "Task not found or you do not have access to update it." });
            }

            // Update the task
            existingTask.Title = taskEntity.Title;
            existingTask.Description = taskEntity.Description;
            existingTask.Deadline = taskEntity.Deadline;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new { Message = "Task updated successfully." });
        }

        // POST: api/TaskEntities
        [HttpPost]
        public async Task<ActionResult<TaskEntity>> PostTaskEntity(TaskEntity taskEntity)
        {
            var userId = User.FindFirst("UserID")?.Value;

            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authorized to create tasks.", user = userId });
            }

            taskEntity.UserID = int.Parse(userId);

            _context.Tasks.Add(taskEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskEntity", new { id = taskEntity.Id }, taskEntity);
        }

        // DELETE: api/TaskEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskEntity(int id)
        {
            var userId = User.FindFirst("UserID")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "User is not authorized to delete this task." });
            }

            var taskEntity = await _context.Tasks
                .Where(task => task.UserID == int.Parse(userId) && task.Id == id)
                .FirstOrDefaultAsync();

            if (taskEntity == null)
            {
                return NotFound(new { Message = "Task not found or you do not have access to delete it." });
            }

            _context.Tasks.Remove(taskEntity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Task deleted successfully." });
        }

        private bool TaskEntityExists(int id)
        {
            var userId = User.FindFirst("UserID")?.Value;
            return _context.Tasks.Any(e => e.Id == id && e.UserID == int.Parse(userId));
        }
    }
}
