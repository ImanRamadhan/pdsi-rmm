using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMM.API.Models
{
    public class ResponseMessage
    {
        private string _message;
        private bool _hasAnError;

        public string Message { get { return _message; } set { _message = value; } }
        public bool HasAnError { get { return _hasAnError; } set { _hasAnError = value; } }
    }
}