using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BaseMiddleware
{
    class BaseException : Exception
    {
        public string[] Errors { get; set; }

        protected BaseException(params string[] errors)
        {
            Errors = errors;
        }
    }
}