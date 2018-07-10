using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Beers.Controllers;
using Beers.Models;
           
namespace Beers.ViewModels
{
    public class LoginUserInfoViewModel : BaseViewModel
    {
        public User LoginUser { get; set; }
        public Account LoginUserAccount { get; set; }

        public LoginUserInfoViewModel()
        {
        }

        public async Task GetUserInfo()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await UserController.GetUserInfos();
                await UserController.GetUserAccounts();
                LoginUser = UserController.LoginUser;
                LoginUserAccount = UserController.LoginUserAccount;
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
