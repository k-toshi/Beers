using System;
using System.Net.Http;

namespace Beers.Models
{
    public class HttpServiceResult<Type>
    {
        public bool IsError { get; set; }
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage + Environment.NewLine;
            }
            set
            {
                errorMessage = value;
            }
        }
        public Type ResultData { get; set; }

        public HttpServiceResult()
        {
            IsError = false;
            ErrorMessage = "";
        }
    }
}
