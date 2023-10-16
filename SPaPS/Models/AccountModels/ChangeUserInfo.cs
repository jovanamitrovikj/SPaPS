using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SPaPS.Models.AccountModels
{
    public class ChangeUserInfo
    {
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public int ClientTypeId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string IdNo { get; set; } = null!;
        [Required]
        public int CityId { get; set; }
        [Required]
        public int? CountryId { get; set; }

        public int? NoOfEmployees { get; set; }
        public DateTime? DateOfEstablishment { get; set; }

        public int? ServiceId { get; set; }

    }
}
