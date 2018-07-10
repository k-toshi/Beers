using System;
namespace Beers.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FromUserType { get; set; }
        public string FromUserId { get; set; }
        public int FromPubId { get; set; }
        public string FromName { get; set; }
        public string ToUserType { get; set; }
        public string ToUserId { get; set; }
        public int ToPubId { get; set; }
        public string ToName { get; set; }
        public long Cash { get; set; }
        public string Category { get; set; }
        public DateTime CreateDateTime { get; set; }
        private string redFlg;
        public string RedFlg 
        {   get{return redFlg;}
            set
            {
                if (value == "1") redFlg = "赤伝票";
                else redFlg = "黒伝票";
            }
        }
        public int CancelTrId { get; set; }
    }
}
