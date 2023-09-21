using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.EntityModels
{
    public enum Gender {
        [Description("Мужской")]
        Male,
        [Description("Женский")]
        Female
    };
    public class PatientCard
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Address { get; set; }
        public String PhoneNumber { get; set; }
        public List<Request> Requests { get; set; }
        public int Age
        {
            get
            {
                if (DateOfBirth != null)
                {
                    DateTime currentDate = DateTime.Now;
                    int age = currentDate.Year - DateOfBirth.Year;

                    // Учесть, что возраст может уменьшиться, если день рождения уже был в этом году
                    if (currentDate < DateOfBirth.AddYears(age))
                    {
                        age--;
                    }

                    return age;
                }
                return 0; // В случае отсутствия даты рождения
            }
        }
    }
}
