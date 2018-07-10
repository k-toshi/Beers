using System;
namespace Beers.Models
{
    public class User
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Eメール
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
        /// <summary>
        /// ユーザ名
        /// </summary>
        /// <value>The name.</value>
        public string PUserName { get; set; }
        /// <summary>
        /// Twitter
        /// </summary>
        /// <value>The twitter account.</value>
        public string TwitterAccount { get; set; }
        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public int EventId { get; set; }
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        public string EventName { get; set; }
        /// <summary>
        /// Gets or sets the pub identifier.
        /// </summary>
        /// <value>The pub identifier.</value>
        public int PubId { get; set; }
        /// <summary>
        /// Gets or sets the name of the pub.
        /// </summary>
        /// <value>The name of the pub.</value>
        public string PubName { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public int Role { get; set; }
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

    }
}
