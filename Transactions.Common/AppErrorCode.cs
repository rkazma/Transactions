using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Common
{
    public class APIErrorCode
    {
        public enum ErrorCode : int
        {
            TRANSACTION_CREATION_FAILED = 3001
        }

        public static ErrorResult TRANSACTION_CREATION_FAILED
        {
            get
            {
                return new ErrorResult((int)ErrorCode.TRANSACTION_CREATION_FAILED, "Transaction creation failed.");
            }
        }
    }

    public class DBErrorCode
    {
        public const int SUCCESS = 0;

        public const int TRANSACTION_CREATION_FAILED = -3001;
    }

    public class ErrorResult
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ErrorResult(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
