using Acc.Domain.Entities.DataModel;
using Acc.Domain.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.HelpersAndUtilities.JwtSecurity
{
    public interface IJwtSecurityService
    {
        User GenerateJwtAdmin(User user);
    }
}
