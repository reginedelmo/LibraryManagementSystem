using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Borrow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int BookID { get; set; }
        public virtual Book Book { get; set; }
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
    }
}