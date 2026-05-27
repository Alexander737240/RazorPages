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

namespace ContosoUniversity.Pages.Enrollments
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
        public string GradeSort { get; set; }
        public string CourseSort { get; set; }
        public string StudentSort { get; set; }
        public int CurrentPageSize { get; set; }

        public PaginatedList<Enrollment> Enrollment { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex, int? pageSize, string searchStudent, string searchCourse)
        {
            CurrentSort = sortOrder;

            GradeSort = sortOrder == "Grade" ? "grade_desc" : "Grade";
            CourseSort = sortOrder == "Course" ? "course_desc" : "Course";
            StudentSort = sortOrder == "Student" ? "student_desc" : "Student";

            if (!string.IsNullOrEmpty(searchString) || !string.IsNullOrEmpty(searchStudent) || !string.IsNullOrEmpty(searchCourse))
            {
                pageIndex = 1;
                CurrentFilter = searchString ?? searchStudent ?? searchCourse;
            }
            else
            {
                CurrentFilter = CurrentFilter ?? "";
            }

            CurrentPageSize = pageSize ?? _configuration.GetValue("PageSize", 5);

            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .AsQueryable();


            if (!string.IsNullOrEmpty(searchString))
            {
                enrollments = enrollments.Where(e =>
                    e.Student.LastName.Contains(searchString) ||
                    e.Student.FirstName.Contains(searchString) ||
                    e.Course.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(searchStudent))
            {
                enrollments = enrollments.Where(e =>
                    e.Student.LastName.Contains(searchStudent) ||
                    e.Student.FirstName.Contains(searchStudent));
            }

            if (!string.IsNullOrEmpty(searchCourse))
            {
                enrollments = enrollments.Where(e =>
                    e.Course.Title.Contains(searchCourse));
            }

            switch (sortOrder)
            {
                case "Grade":
                    enrollments = enrollments.OrderBy(e => e.Grade);
                    break;
                case "grade_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Grade);
                    break;
                case "Course":
                    enrollments = enrollments.OrderBy(e => e.Course.Title);
                    break;
                case "course_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Course.Title);
                    break;
                case "Student":
                    enrollments = enrollments.OrderBy(e => e.Student.LastName);
                    break;
                case "student_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Student.LastName);
                    break;
                default:
                    enrollments = enrollments.OrderBy(e => e.EnrollmentID);
                    break;
            }

            Enrollment = await PaginatedList<Enrollment>.CreateAsync(
                enrollments.AsNoTracking(),
                pageIndex ?? 1,
                CurrentPageSize
            );
        }
    }
}