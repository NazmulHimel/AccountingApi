using Acc.Domain.Interfaces.Settings;
using Acc.HelpersAndUtilities.JwtSecurity;
using Acc.Services.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.Services.Settings
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtSecurityService _jwtSecurityService;
        public UserService(IUserRepository userRepository, IJwtSecurityService jwtSecurityService)
        {
            _userRepository = userRepository;
            _jwtSecurityService = jwtSecurityService;
        }
    }
}
