using Acc.Domain.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.HelpersAndUtilities.JwtSecurity
{
    public interface IJwtSecurityService
    {
        UserViewModel GenerateJwtAdmin(UserViewModel user);
    }
}
