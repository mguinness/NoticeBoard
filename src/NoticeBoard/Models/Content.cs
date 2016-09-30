using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoticeBoard.Models
{
    public class Content
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Markdown { get; set; }
        [StringLength(50)]
        public string Path { get; set; }
        [Required, StringLength(50)]
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
