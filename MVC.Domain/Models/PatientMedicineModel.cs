using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain.Models
{
    [Table("PatientMedicines")]
    public class PatientMedicineModel
    {
        [Key]
        public long? PatientMedicineId { get; set; }
        public long? PatientId { get; set; }
        public long? MedicineId { get; set; }
        [ForeignKey(nameof(MedicineId))]
        public MedicineModel? Medicine { get; set; }
        public string? Description { get; set; }
    }
}
