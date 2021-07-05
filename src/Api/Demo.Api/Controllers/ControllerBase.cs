using Common.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Api.Controllers
{
    public class DemoControllerBase : ControllerBase
    {


        public async Task<EFApiActionResult<T>> SafeExecuteAsync<T>(Func<List<EFApiActionMessage>, Task<T>> func)
        {
            EFApiActionResult<T> ret = new EFApiActionResult<T>();
            try
            {
                ret.Data = await func(ret.Messages);
                
            } 
            catch (Exception ex)
            {
                ret.Messages.Add(new EFApiActionMessage() { MessageType = EFMessageType.Error, Message = GetErrorMsg(ex) });
            }
            ret.IsError = ret.Messages.Any(t => t.MessageType == EFMessageType.Error);
            return ret;
        }


        public EFApiActionResult<T> SafeExecute<T>(Func<List<EFApiActionMessage>, T> func)
        {
            EFApiActionResult<T> ret = new EFApiActionResult<T>();
            try
            {
                ret.Data = func(ret.Messages);
                
            }  
            catch (Exception ex)
            {
                ret.Messages.Add(new EFApiActionMessage() { MessageType = EFMessageType.Error, Message = GetErrorMsg(ex) });
            }
            ret.IsError = ret.Messages.Any(t => t.MessageType == EFMessageType.Error);
            return ret;
        }

        private string GetErrorMsg(Exception ex)
        {

            return ex.Message;
        }

    }
}
