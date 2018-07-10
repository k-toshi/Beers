using System;
namespace Beers.Models
{
    public static class CommonDef
    {
        public enum PubsViewMode
        {
            UserPub
        }

        public enum UserType
        {
            User = 1,
            Pub = 2,
            Id = 9,
            Admin = 99
        }

        public enum AppAccountCategory
        {
            User = 1,
            Pub = 2,
            Admin = 9
        }

        public enum QrCodeType
        {
            payment
        }

        public enum CuUserPageType
        {
            New,
            Change,
            ChangePassword
        }
    }
}
