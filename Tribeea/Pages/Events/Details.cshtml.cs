using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tribeea.Data.Entities;
using Tribeea.Viewmodel;

namespace Tribeea.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public DetailsModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;
        [BindProperty]
        public IList<Team> Teams { get; set; }
        [BindProperty]
        public IList<Scorecard> Scorecards { get; set; }
        public List<ScorecardViewmodel> ScorecardViewmodels { get; set; }
        public List<ScorecardViewmodel> MonthlyScorecardViewmodels { get; set; }

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
                Teams = await _context.Teams.Where(x => !scorecardIds.Contains(x.Id)).OrderBy(y => y.Name).ToListAsync();
                ScorecardViewmodels = new List<ScorecardViewmodel>();
                ScorecardViewmodels = GetScorecard(id, "usp_GetEventScorecard");
                MonthlyScorecardViewmodels = GetScorecard(id, "usp_GetMonthlyEventScorecard");
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

            await _context.SaveChangesAsync();


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
                Teams = await _context.Teams.Where(x => !scorecardIds.Contains(x.Id)).OrderBy(y => y.Name).ToListAsync();
                ScorecardViewmodels = GetScorecard(id, "usp_GetEventScorecard");
                MonthlyScorecardViewmodels = GetScorecard(id, "usp_GetMonthlyEventScorecard");
            }

            return Page();
        }

        private List<ScorecardViewmodel> GetScorecard(int? id, string spName)
        {
            List<ScorecardViewmodel>  scorecardsView = new List<ScorecardViewmodel>();
            using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@eventId", id));
                    cmd.CommandText = spName;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            foreach (DataRow row in dt.Rows)
                            {
                                scorecardsView.Add(new ScorecardViewmodel()
                                {
                                    Ranking = row[0].ToString(),
                                    TeamName = row[1].ToString(),
                                    Score = (int)row[2]
                                });
                            }
                        }
                    }
                }
            }
            return scorecardsView;
        }
    }
}
