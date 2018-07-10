using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
    public class CuUserViewModel : BaseViewModel
    {

        public Command CuUserCommand { get; set; }

        private User newUser;
        public User NewUser
        {
            get { return newUser; }
            set { SetProperty(ref newUser, value); }
        }

        private string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set { SetProperty(ref oldPassword, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string confirmationPassword;
        public string ConfirmationPassword
        {
            get { return confirmationPassword; }
            set { SetProperty(ref confirmationPassword, value); }
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

        private CommonDef.CuUserPageType cuUserPageType { get; set; }

        public CuUserViewModel(CommonDef.CuUserPageType cuUserPageType)
        {
            NewUser = new User();
            if (cuUserPageType == CommonDef.CuUserPageType.Change) NewUser = UserController.LoginUser;
            this.cuUserPageType = cuUserPageType;
            CuUserCommand = new Command(async () => await ExecuteCuUserCommand());
        }

        public async Task ExecuteCuUserCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (await CuUser()) IsError = false;
                else IsError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> CuUser()
        {
            string errorString = InputErrorCheck();

            if (String.IsNullOrEmpty(errorString))
            {
                errorString += await ExecuteCuUser();
            }
            if (!String.IsNullOrEmpty(errorString))
            {
                Error = errorString;
                return false;
            }
            else return true;
        }

        private async Task<string> ExecuteCuUser()
        {
            switch (this.cuUserPageType)
            {
                case CommonDef.CuUserPageType.New:
                    return await UserController.CreateUser(NewUser, Password, ConfirmationPassword);
                case CommonDef.CuUserPageType.Change:
                    return await UserController.ChangeUser(NewUser);
                case CommonDef.CuUserPageType.ChangePassword:
                    return await UserController.ChangePassword(OldPassword, Password, confirmationPassword);
                default:
                    return "";
            }
        }

        private string InputErrorCheck()
        {
            string errorString = "";
            switch (this.cuUserPageType)
            {
                case CommonDef.CuUserPageType.New:
                    errorString += ErrorController.IsNull(NewUser.Email, "Email");
                    errorString += ErrorController.IsEmail(NewUser.Email, "Email");
                    errorString += ErrorController.IsNull(NewUser.PUserName, "ユーザ名");
                    errorString += ErrorController.IsNull(Password, "パスワード（新）");
                    errorString += ErrorController.IsNull(ConfirmationPassword, "パスワード（確認用）");
                    errorString += ErrorController.ConrfirmPassword(Password, ConfirmationPassword, "パスワード");
                    break;
                case CommonDef.CuUserPageType.Change:
                    errorString += ErrorController.IsNull(NewUser.PUserName, "ユーザ名");
                    break;
                case CommonDef.CuUserPageType.ChangePassword:
                    errorString += ErrorController.IsNull(OldPassword, "パスワード（旧）");
                    errorString += ErrorController.IsNull(Password, "パスワード（新）");
                    errorString += ErrorController.IsNull(ConfirmationPassword, "パスワード（確認用）");
                    errorString += ErrorController.ConrfirmPassword(Password, ConfirmationPassword, "パスワード");
                    break;
                default:
                    break;
            }

            return errorString;
        }
    }
}
