using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPaPS.Models
{
    public partial class ReferenceType
    {
        public ReferenceType()
        {
            References = new HashSet<Reference>();
        }

        [Required]
        public long ReferenceTypeId { get; set; }
       [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
      //  [Required]
        public DateTime? CreatedOn { get; set; }
       // [Required]
        public int?   CreatedBy { get; set; }
       // [Required]
        public DateTime? UpdatedOn { get; set; }
       //[Required]
       
        public int? UpdatedBy { get; set; }
       [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Reference> References { get; set; }
    }
}
