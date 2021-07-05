using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AspNetCore
{
    public class EFApiActionResult<T>
    {
        public EFApiActionResult()
        {
            this.Messages = new List<EFApiActionMessage>();
        }
        public bool IsError { get; set; }


        public T Data { get; set; }

        public List<EFApiActionMessage> Messages { get; set; }
    }

    public class EFApiActionMessage
    {
        public EFMessageType MessageType { get; set; }

        public string Code { get; set; }
        public string Message { get; set; }
    }
    public enum EFMessageType
    {
        Error = 0,
        Warning = 1,
        Info = 2,
    }

}
