using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tribeea.Data.Entities;

namespace Tribeea.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly Tribeea.Data.ApplicationDbContext _context;

        public DetailsModel(Tribeea.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;
        [BindProperty]
        public IList<Team> Teams { get; set; }
        [BindProperty]
        public IList<Scorecard> Scorecards { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventRecord = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);            
            if (eventRecord == null)
            {
                return NotFound();
            }
            else
            {
                Event = eventRecord;
                Scorecards = await _context.Scorecards.Where(x => x.EventId == id).Include(y => y.Team).ToListAsync();
                var scorecardIds = Scorecards.Select(x => x.TeamId).ToList();
                Teams = await _context.Teams.Where(x => !scorecardIds.Contains(x.Id)).ToListAsync();                
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            foreach (var scorecard in Scorecards)
            {
                _context.Attach(scorecard).State = EntityState.Modified;
            }

            //try
            //{
                await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EventExists(Event.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            var eventRecord = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (eventRecord == null)
            {
                return NotFound();
            }
            else
            {
                Event = eventRecord;
                Scorecards = await _context.Scorecards.Where(x => x.EventId == id).Include(y => y.Team).ToListAsync();
                var scorecardIds = Scorecards.Select(x => x.TeamId).ToList();
                Teams = await _context.Teams.Where(x => !scorecardIds.Contains(x.Id)).ToListAsync();
            }

            return Page();
        }      
    }
}
