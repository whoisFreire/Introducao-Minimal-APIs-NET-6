using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get([FromServices] AppDbContext context)
            => Ok(context.Todos.AsNoTracking().ToList());
        
        [HttpGet("/{id:int}")]
        public IActionResult GetById([FromServices] AppDbContext context, [FromRoute] int id)
            => Ok(context.Todos.AsNoTracking().FirstOrDefault(x => x.Id == id));
        
        [HttpPost("/")]
        public IActionResult Post([FromServices] AppDbContext context, [FromBody] TodoModel model)
        {
            context.Todos.Add(model);
            context.SaveChanges();

            return Created($"/{model.Id}",model);
        }
        
        [HttpPut("/{id:int}")]
        public IActionResult Put([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] TodoModel model)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            todo.Title = model.Title;
            todo.Done = model.Done;

            context.Todos.Update(todo);
            context.SaveChanges();

            return Ok(todo);
        }

        [HttpDelete("/{id:int}")]
        public IActionResult Delete([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id);
            if(todo == null)
            {
                return NotFound();
            }
            context.Todos.Remove(todo);
            context.SaveChanges();

            return Ok(todo);
        }
    }
}