using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Pages.Employee
{
    public class ListModel : PageModel
    {

        private readonly PePrnSum25B5WaContext _context;
        public ListModel(PePrnSum25B5WaContext context)
        {
            _context = context;
        }

        public List<Q2.Models.Employee> employees = new();
        public List<Q2.Models.Department> departments = new();

        public Q2.Models.Employee? selectedEmployee { get; set; }



        [BindProperty(SupportsGet = true)]
        public string? EmployeeId { get; set; }

        public void OnGet()
        {
            // load all employees + department
            employees = _context.Employees
                .Include(e => e.Department)
                .ToList();
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                int empId = int.Parse(EmployeeId);
                selectedEmployee = _context.Employees.Include(e => e.EmployeeSkills)
                    .ThenInclude(es => es.Skill)    
                    .FirstOrDefault(e => e.EmployeeId == empId);
            }

            departments = _context.Departments.ToList();

        }

        public IActionResult OnPost(string? Department) 
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Department))
            {
                query = query.Where(e => e.Department.DepartmentName == Department);
            }

            employees = query.ToList();

            departments = _context.Departments.ToList();

            return Page();
        }
    }
}
