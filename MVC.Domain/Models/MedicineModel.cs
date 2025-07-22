using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Domain.Models
{
    [Table("Medicines")]
    public class MedicineModel
    {
        [Key]
        public long? MedicineId { get; set; }
        public string? MedicineName { get; set; }
        public string? QRCode { get; set; }
    }
}
