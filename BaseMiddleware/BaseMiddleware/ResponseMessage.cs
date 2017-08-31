using System;
using System.Collections.Generic;
using System.Text;

namespace BaseMiddleware
{
    class ResponseMessage
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
        public string[] Errors { get; set; }
    }
}