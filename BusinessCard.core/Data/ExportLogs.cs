using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCard.core.Data
{
    public class ExportLogs
    {
        [Key]
        public int Id { get; set; }

        public DateTime ExportDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string FileType { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
