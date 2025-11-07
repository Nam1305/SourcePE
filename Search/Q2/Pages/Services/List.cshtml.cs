using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Pages.Services
{
    public class ListModel : PageModel
    {
        private readonly Prnsum25B123Context _context;
        public ListModel(Prnsum25B123Context context)
        {
            _context = context;
        }

        [BindProperty]
        public string? RoomTitle { get; set; }

        [BindProperty]
        public string? FeeType { get; set; }

        public List<Service> Results { get; set; } = new();
        public async Task OnGetAsync()
        {
            Results = await _context.Services
                .Include(s => s.RoomTitleNavigation)
                .Include(s => s.EmployeeNavigation)
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        // Khi bấm nút Search (POST)
        public async Task<IActionResult> OnPostAsync()
        {
            var query = _context.Services
                .Include(s => s.RoomTitleNavigation)
                .Include(s => s.EmployeeNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(RoomTitle))
            {
                query = query.Where(r => r.RoomTitle != null && r.RoomTitle.Contains(RoomTitle));
            }

            if (!string.IsNullOrEmpty(FeeType))
            {
                query = query.Where(r => r.FeeType != null && r.FeeType.Contains(FeeType));
            }

            Results = await query.ToListAsync();
            return Page();

        }
    }
}