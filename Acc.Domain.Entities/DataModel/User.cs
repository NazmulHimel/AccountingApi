using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Acc.Domain.Entities.DataModel
{
    [Table("User", Schema = "Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public int AppId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public int UserLevel { get; set; } = 0;
        public int RoleId { get; set; } = 0;
        public bool LogUser { get; set; } = false;
        public bool LogLocal { get; set; } = false;
        public int? UserLoginTypeId { get; set; } = 0;
        public int? UserIdGlobal { get; set; } = 0;
        public string EmpCode { get; set; }
        public int? EmpId { get; set; } = 0;
        public int? LoginByType { get; set; } = 0;
        public int? LocationId { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsSystem { get; set; } = false;
        public bool IsVisible { get; set; } = false;
        public bool MRestrict { get; set; } = false;
        public string MName1 { get; set; }
        public string MName2 { get; set; }
        public string MName3 { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
