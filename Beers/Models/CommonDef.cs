using System;
namespace Beers.Models
{
    public static class CommonDef
    {
        public enum PubsViewMode
        {
            UserPubForItem,
            UserPubForEvent
        }

		public enum EventsViewMode
        {
			PubParticipatedEvents,
			AllNotStartedEvents
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
            Payment,
            ApplyEvent
        }

        public enum CuUserPageType
        {
            New,
            Change,
            ChangePassword
        }

		public enum EventGetType
        {
			NotParticipating = 0,
			Participating = 1,
            All = 2,
			NotParticipatingFinished = 3,
            UnsettledPrize = 4
        }
    }
}
