using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Genre { get; set; }
        [StringLength(50)]
        public string Author { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public int Copies { get; set; }
        public int BorrowedCopies { get; set; }
        public bool Availability { get; set; }
    }
}