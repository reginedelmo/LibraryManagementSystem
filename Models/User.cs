using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [StringLength(20)]
        public string Username { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public string UserType { get; set; }
    }

    public enum UserType
    {
        Librarian,
        Member
    }
}