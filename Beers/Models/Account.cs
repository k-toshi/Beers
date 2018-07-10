using System;
namespace Beers.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PubId { get; set; }
        public long Cash { get; set; }
        public string Category { get; set; }
    }
}
