using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Q2.Models;

namespace Q2.Pages.Instructor
{
    public class ListModel : PageModel
    {
        private readonly PePrn25sprB5Context _context;
        public ListModel(PePrn25sprB5Context context)
        {
            _context = context;
        }

        public List<Q2.Models.Instructor> Instructors { get; set; } = new();

        public List<Q2.Models.Department> Departments { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Department { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? IsFulltime { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }



        public void OnGet()
        {
            var query = _context.Instructors.Include(i => i.DepartmentNavigation).AsQueryable();
            if (!Department.IsNullOrEmpty())
            {
                var x = int.Parse(Department);
                query = query.Where(i => i.Department == x);
            }

            if (!IsFulltime.IsNullOrEmpty())
            {
                if (IsFulltime.Equals("Fulltime")) 
                {
                    query =  query.Where(i => i.IsFulltime == true);
                }

                if (IsFulltime.Equals("Parttime"))
                {
                    query =  query.Where(i => i.IsFulltime == false);
                }
            }

            if (!SortBy.IsNullOrEmpty())
            {
                if (SortBy.Equals("instructorName"))
                {
                    query = query.OrderBy(i => i.Fullname);
                }

                if (SortBy.Equals("instructorId"))
                {
                    query = query.OrderBy(i => i.InstructorId);
                }
                if (SortBy.Equals("contractDate"))
                {
                    query = query.OrderBy(i => i.ContractDate);
                }
            }

            Instructors = query.ToList();
            Departments = _context.Departments.ToList();
        }
    }
}
