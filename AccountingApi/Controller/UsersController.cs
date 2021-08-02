using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acc.Domain.Entities.BodyModel;
using Acc.Domain.Entities.DataModel;
using Acc.HelpersAndUtilities.Response;
using Acc.Services.Interfaces.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingApi.Controller
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
     
        public UsersController(IUserService userService)
        {
            _userService = userService;
         
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("UserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin([FromBody] LoginBodyModel user)
        {
            var response = new SingleResponseModel<User>();

            try
            {
                var data = await _userService.UserLogin(user);
                response.Model = data;
                if (response.Model != null)
                {
                    response.Message = "Login Successfull!!";
                }
                else
                {
                    response.DidError = true;
                    response.ErrorMessage = "User Name and Password Not Match!!";
                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var response = new ListResponseModel<User>();
            try
            {
                var data = await _userService.GetUser();
                response.Model = data;
                if (response.Model != null)
                {
                    response.Message = "Data Successfull!";
                }
                else
                {
                    response.DidError = true;
                    response.ErrorMessage = "Data Faild!!";
                }
            }
            catch (Exception exception)
            {
                response.DidError = true;
                response.ErrorMessage = exception.Message.ToString();
            }
            return response.ToHttpResponse();
        }
    }
}
