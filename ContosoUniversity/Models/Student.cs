using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Фамилия - это обязательное поле")]
        [StringLength(50,ErrorMessage="Первышено максимальное количество символов")]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$",ErrorMessage ="Фамилия может включать в себя только символы русского и латинского алфавита")]
        [Display(Name="Фамилия")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Имя - это обязательное поле")]
        [StringLength(50,ErrorMessage = "Первышено максимальное количество символов")]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Имя может включать в себя только символы русского и латинского алфавита")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата поступления")]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = "Студент")]
        public string FullName 
        { 
            get => $"{LastName} {FirstName}";
        }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
