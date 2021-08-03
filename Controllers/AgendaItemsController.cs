using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgendaApi.Models;
using Microsoft.AspNetCore.Cors;


namespace AgendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaItemsController : ControllerBase
    {
        private readonly AgendaContext _context;

        public AgendaItemsController(AgendaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendaItemDTO>>> GetagendaItems()
        {
            return await _context.agendaItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgendaItemDTO>> GetagendaItem(long id)
        {
            var agendaItem = await _context.agendaItems.FindAsync(id);

            if (agendaItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(agendaItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateagendaItem(long id, AgendaItemDTO agendaItemDTO)
        {
            if (id != agendaItemDTO.Id)
            {
                return BadRequest();
            }

            var agendaItem = await _context.agendaItems.FindAsync(id);
            if (agendaItem == null)
            {
                return NotFound();
            }

            agendaItem.Name = agendaItemDTO.Name;
            agendaItem.Number = agendaItemDTO.Number;
            agendaItem.Email = agendaItemDTO.Email;
            agendaItem.Avatar = agendaItemDTO.Avatar;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!agendaItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AgendaItemDTO>> CreateagendaItem(AgendaItemDTO agendaItemDTO)
        {
            var agendaItem = new AgendaItem
            {
                Name = agendaItemDTO.Name,
                Number = agendaItemDTO.Number,
                Email = agendaItemDTO.Email,
                Avatar = agendaItemDTO.Avatar
        };

            _context.agendaItems.Add(agendaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetagendaItem),
                new { id = agendaItem.Id },
                ItemToDTO(agendaItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteagendaItem(long id)
        {
            var agendaItem = await _context.agendaItems.FindAsync(id);

            if (agendaItem == null)
            {
                return NotFound();
            }

            _context.agendaItems.Remove(agendaItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool agendaItemExists(long id) =>
             _context.agendaItems.Any(e => e.Id == id);

        private static AgendaItemDTO ItemToDTO(AgendaItem agendaItem) =>
            new AgendaItemDTO
            {
                Id = agendaItem.Id,
                Name = agendaItem.Name,
                Number = agendaItem.Number,
                Email = agendaItem.Email,
                Avatar = agendaItem.Avatar
            };
    }
}
