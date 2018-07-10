using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; set; }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string error;
        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set { SetProperty(ref isError, value); }
        }

        public LoginViewModel()
        {
            IsError = false;
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        public async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (await Login())
                {
                    IsError = false;
                }
                else IsError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> Login()
        {
            string errorString = "";
            errorString += ErrorController.IsNull(Email,"Email");
            errorString += ErrorController.IsEmail(Email, "Email");
            errorString += ErrorController.IsNull(Password, "Password");

            if(String.IsNullOrEmpty(errorString))
            {
                UserController.SetLoginUserEmail(Email);
                errorString += await UserController.Login(Password);
            }
            if (!String.IsNullOrEmpty(errorString))
            {
                Error = errorString;
                return false;
            }
            else
            {
                await UserController.GetUserInfos();
                await UserController.GetUserAccounts();
                return true;
            }
        }
    }
}
