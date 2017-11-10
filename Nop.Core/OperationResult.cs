using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public OperationResult()
        {

        }

        public OperationResult(bool isSuccess,string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }


}
