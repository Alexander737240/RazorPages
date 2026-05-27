using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.ContosoUniversityContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(ContosoUniversity.Data.ContosoUniversityContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }
        public string TitleSort { get; set; }
        public string CreditsSort { get; set; }
        public int CurrentPageSize { get; set; }

        public PaginatedList<Course> Course { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex, int? pageSize)
        {
            CurrentSort = sortOrder;

            TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
            CreditsSort = sortOrder == "Credits" ? "credits_desc" : "Credits";

            if (searchString != null)
            {
                pageIndex = 1;
                CurrentFilter = searchString;
            }
            else
            {
                CurrentFilter = CurrentFilter ?? "";
            }

            CurrentPageSize = pageSize ?? _configuration.GetValue("PageSize", 5);

            var courses = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                courses = courses.Where(c =>
                    c.Title.Contains(CurrentFilter));
            }

            switch (sortOrder)
            {
                case "Title":
                    courses = courses.OrderBy(c => c.Title);
                    break;
                case "title_desc":
                    courses = courses.OrderByDescending(c => c.Title);
                    break;
                case "Credits":
                    courses = courses.OrderBy(c => c.Credits);
                    break;
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                default:
                    courses = courses.OrderBy(c => c.CourseID);
                    break;
            }

            Course = await PaginatedList<Course>.CreateAsync(
                courses.AsNoTracking(),
                pageIndex ?? 1,
                CurrentPageSize
            );
        }
    }
}