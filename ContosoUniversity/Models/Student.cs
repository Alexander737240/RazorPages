namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public string FullName 
        { 
            get => $"{LastName} {FirstName}";
        }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
