using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tribeea.Data.Entities;

namespace Tribeea.Controllers
{
    public class Scorecards : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        public Scorecards(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Scorecards
        public ActionResult Index()
        {
            return View();
        }

        // GET: Scorecards/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Scorecards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scorecards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Scorecards/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Scorecards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Scorecards/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Scorecards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("Scorecards/CreateScorecard",
            Name = "CreateScorecard")]
        public IActionResult CreateScorecard(int teamId, int eventId)
        {
            var eventRecord = _context.Events.FirstOrDefaultAsync(m => m.Id == eventId);
            if (eventRecord == null)
            {
                return NotFound();
            }

            var teamRecord = _context.Teams.FirstOrDefaultAsync(m => m.Id == teamId);
            if (teamRecord == null)
            {
                return NotFound();
            }

            var scorecard = new Scorecard();
            scorecard.EventId = eventId;
            scorecard.TeamId = teamId;
            _context.Scorecards.Add(scorecard);

            _context.SaveChangesAsync();

            return RedirectToPage("./events", new { id = eventId });
        }
    }
}
