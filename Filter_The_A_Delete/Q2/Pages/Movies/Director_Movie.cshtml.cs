using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Pages.Movies
{
    public class Director_MovieModel : PageModel
    {
        private readonly PePrn222TrialContext _context;
        public Director_MovieModel(PePrn222TrialContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? DirectorId { get; set; }

        public List<Movie> Results { get; set; } = new();
        public List<Director> Directors { get; set; } = new();  

        public async Task OnGetAsync()
        {
            var query = _context.Movies.Include(m => m.Stars).Include(m => m.Director).AsQueryable();
            if (DirectorId.HasValue)
            {
                query = query.Where(m => m.DirectorId == DirectorId);
            }
            Directors = await _context.Directors.ToListAsync();
            Results = await query.ToListAsync();
                
        }


        public IActionResult OnGetDelete(int? MovieId)
        {
            var current = _context.
                Movies.Include(m => m.Genres).Include(m => m.Stars).FirstOrDefault(m => m.Id == MovieId);

            current.Stars.Clear();
            current.Genres.Clear();

            _context.SaveChanges();
            _context.Remove(current);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
