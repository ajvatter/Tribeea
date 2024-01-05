using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tribeea.Data;
using Tribeea.Data.Entities;

namespace Tribeea.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly Tribeea.Data.ApplicationDbContext _context;

        public IndexModel(Tribeea.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Event> Event { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Event = await _context.Events.ToListAsync();
        }
    }
}
