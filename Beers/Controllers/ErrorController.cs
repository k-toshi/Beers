using System;
namespace Beers.Controllers
{
    public static class ErrorController
    {
        public static string IsNull(string target,string targetname)
        {
            if (String.IsNullOrEmpty(target)) return targetname + "が空白です。" + Environment.NewLine;
            else return "";
        }

        public static string IsPassword(string target, string targetname)
        {
            if (String.IsNullOrEmpty(target)) return targetname + "はxxxが必要です。" + Environment.NewLine;
            else return "";
        }

        public static string ConrfirmPassword(string password,string confrimationPassword, string targetname)
        {
            if (password != confrimationPassword) return targetname + "が確認用と一致しません。" + Environment.NewLine;
            else return "";
        }

        public static string IsEmail(string target, string targetname)
        {
            if (String.IsNullOrEmpty(target)) return targetname + "がEmailの形式ではありません。" + Environment.NewLine;
            else return "";
        }
    }
}
