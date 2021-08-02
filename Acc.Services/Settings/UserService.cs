using Acc.Domain.Entities.BodyModel;
using Acc.Domain.Entities.DataModel;
using Acc.Domain.Interfaces.Settings;
using Acc.HelpersAndUtilities.JwtSecurity;
using Acc.Services.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<User> UserLogin(LoginBodyModel user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserPassword) || string.IsNullOrEmpty(user.UserName))
                    return null;
                
                var dataList = (from u in await _userRepository.FindAllAsyn()
                                
                                where u.UserName.Equals(user.UserName)
                                      && u.IsActive.Equals(true)
                                      && u.Password.Equals(user.UserPassword)
                                select new User
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName,
                                    Password = u.Password,
                                    FullName = u.FullName,
                                    AccessToken = u.AccessToken,
                                    RefreshToken = u.RefreshToken
                                }).ToList();

               


                var data = dataList.FirstOrDefault();

                if (data != null)
                {
                    var userViewModel = new User
                    {
                        UserId = data.UserId,
                      
                        UserName = data.UserName,
                        Password = data.Password,
                        FullName = data.FullName,
                        AccessToken = data.AccessToken,
                        RefreshToken = data.RefreshToken
                    };
                    var userToken = _jwtSecurityService.GenerateJwtAdmin(userViewModel);
                    return userToken;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }


        }

        public async Task<IEnumerable<User>> GetUser()
        {
            try
            {
                var dataList = await _userRepository.FindAllAsyn();
                return dataList;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}
