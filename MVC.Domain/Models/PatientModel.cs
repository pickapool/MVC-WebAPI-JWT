using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain.Models
{
    [Table("Patients")]
    public record class PatientModel
    {
        [Key]
        public long? PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? RoomName { get; set; }
        public string? BedNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey(nameof(PatientId))]
        public List<PatientMedicineModel>? Medicines { get; set; }
    }
}
