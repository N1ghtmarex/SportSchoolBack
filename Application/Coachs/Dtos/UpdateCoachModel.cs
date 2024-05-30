using Microsoft.AspNetCore.Http;

namespace Application.Coachs.Dtos
{
    public class UpdateCoachModel
    {
        public IFormFile? Image { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public required string Institution { get; set; }
        public required string Faculty { get; set; }
        public required string Speciality { get; set; }
        public required string EducationForm { get; set; }
        public required string Qualification { get; set; }
        public required string Job { get; set; }
        public required string JobTitle { get; set; }
        public required string JobPeriod { get; set; }
    }
}
