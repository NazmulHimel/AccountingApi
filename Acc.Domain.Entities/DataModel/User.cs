using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Acc.Domain.Entities.DataModel
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
    }
}
