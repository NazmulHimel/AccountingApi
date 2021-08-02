using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.Domain.Entities.BodyModel
{
    public class LoginBodyModel
    {
        public int AppId { get; set; }
        public int LocationId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
