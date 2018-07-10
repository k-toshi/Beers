using System;
using System.Threading.Tasks;
using Beers.Services;
using Beers.Models;
using Beers.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
           
namespace Beers.Controllers
{
    public static class UserController
    {
        /// <summary>
        /// ログインユーザ
        /// </summary>
        /// <value>The login user.</value>
        public static User LoginUser { get; private set; }
        /// <summary>
        /// Gets or sets the login token.
        /// </summary>
        /// <value>The login token.</value>
        private static string LoginToken { get; set; }
        /// <summary>
        /// Gets or sets the login user account.
        /// </summary>
        /// <value>The login user account.</value>
        public static Account LoginUserAccount { get; private set; }
        /// <summary>
        /// The step manger.
        /// </summary>
        public static IStepManager stepManger { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Beers.Controllers.UserController"/> is login.
        /// </summary>
        /// <value><c>true</c> if is login; otherwise, <c>false</c>.</value>
        public static bool IsLogin { get; set; }

        /// <summary>
        /// ログイン用のEmailの設定
        /// </summary>
        public static void SetLoginUserEmail(string Email)
        {
            LoginUser = new User();
            LoginUser.Email = Email;
        }
        /// <summary>
        /// ログインユーザのToken取得
        /// </summary>
        /// <returns>The login user token.</returns>
        public static string GetLoginUserToken() { return LoginToken; }
        /// <summary>
        /// ログイン
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="password">Password.</param>
        public static async Task<string> Login(string password)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"grant_type","password"},
                {"username",LoginUser.Email},
                {"password",password}
            };

            HttpServiceResult<dynamic> hsr = await HttpService.PostDataFromService<dynamic>("token", contents);

            if (!hsr.IsError)
            {
                LoginToken = hsr.ResultData["access_token"];
                IsLogin = true;
                return "";
            }
            else return hsr.ErrorMessage;

        }

        /// <summary>
        /// ユーザ情報の取得
        /// </summary>
        /// <returns>The user infos.</returns>
        public static async Task<string> GetUserInfos()
        {
            HttpServiceResult<User> hsr = await HttpService.GetDataFromServiceWithToken<User>("api/account/userinfoall");              

            if (!hsr.IsError)
            {
                LoginUser = hsr.ResultData;
                return "";
            }
            else return hsr.ErrorMessage;

        }
        /// <summary>
        /// Gets the user accounts.
        /// </summary>
        /// <returns>The user accounts.</returns>
        public static async Task<string> GetUserAccounts()
        {
            HttpServiceResult<Account> hsr = await HttpService.GetDataFromServiceWithToken<Account>("api/accounts/getaccount");

            if (!hsr.IsError)
            {
                LoginUserAccount = hsr.ResultData;
                return "";
            }
            else return hsr.ErrorMessage;

        }

        /// <summary>
        /// ユーザのトランザクションの履歴を取得する
        /// </summary>
        /// <returns>The app accounts.</returns>
        public static async Task<ObservableCollection<Transaction>> GetTransactionHistory()
        {
            HttpServiceResult<ObservableCollection<Transaction>> hsr = await HttpService.GetDataFromServiceWithToken<ObservableCollection<Transaction>>("api/appaccounts/getappaccounts");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;

        }

        public static async Task<ObservableCollection<PubView>> GetUserPubs()
        {
            HttpServiceResult<ObservableCollection<PubView>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<PubView>>
                                 ("api/userpubs/getuserpub?userid=" + LoginUser.Id + "&apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

        public static async Task<string> PayToPub(int toPubId,long toCash)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"ToUserType",((int)CommonDef.UserType.Pub).ToString()},
                {"ToUserId","-1"},
                {"ToPubId",toPubId.ToString()},
                {"ToAccountId","-1"},
                {"Cash",toCash.ToString()},
                {"Category",((int)CommonDef.AppAccountCategory.Pub).ToString()},
                {"FromPubId","-1"}
            };

            HttpServiceResult<dynamic> hsr = await HttpService.PostDataFromServiceWithToken<dynamic>("api/appaccounts/posttransfer", contents);

            if (!hsr.IsError) return "";
            else return hsr.ErrorMessage;

        }

        public static async Task<string> GetStepPoint()
        {
            HttpServiceResult<string> hsr =
                await HttpService.GetDataFromServiceWithToken<string>
                                 ("api/steppoint/getstepareapoint?point=" + stepManger.GetStepPoints());

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

        public static async Task<string> CreateUser(User newUser,string password,string confirmationPassword)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"PUserName",newUser.PUserName},
                {"Email",newUser.Email},
                {"Password",password},
                {"ConfirmPassword",confirmationPassword},
                {"TwitterAccount",newUser.TwitterAccount}
            };

            HttpServiceResult<dynamic> hsr = await HttpService.PostDataFromServiceWithToken<dynamic>("api/account/register", contents);

            if (!hsr.IsError)
            {
                SetLoginUserEmail(newUser.Email);
                await Login(password);
                IsLogin = true;
                return "";
            }
            else return hsr.ErrorMessage;
        }

        public static async Task<string> ChangeUser(User targetUser)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"Id",targetUser.Id},
                {"PUserName",targetUser.PUserName},
                {"Email",targetUser.Email},
                {"TwitterAccount",targetUser.TwitterAccount}
            };

            HttpServiceResult<dynamic> hsr = await HttpService.PutDataFromServiceWithToken<dynamic>("api/aspnetusers/" + targetUser.Id, contents);

            if (!hsr.IsError) return "";
            else return hsr.ErrorMessage;
        }

        public static async Task<string> ChangePassword(string oldPassword,string newPassword, string confirmationPassword)
        {
            Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"OldPassword",oldPassword},
                {"NewPassword",newPassword},
                {"ConfirmPassword",confirmationPassword},
            };

            HttpServiceResult<dynamic> hsr = await HttpService.PostDataFromServiceWithToken<dynamic>("api/account/ChangePassword", contents);

            if (!hsr.IsError) return "";
            else return hsr.ErrorMessage;
        }
    }
}
