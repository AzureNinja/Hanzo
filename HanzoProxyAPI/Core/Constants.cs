using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanzoProxyAPI.Core
{
    static class Constants
    {
        public const string ApiStatOK = "ok";
        public const string ApiStatFailure = "fail";
        public const int ApiErrorCodeInvalidParameter = 100;
        public const int ApiErrorCodeUnauthorized = 101;
        public const int ApiErrorCodeUnknownExecutionError = 120;
        public const int ApiErrorCodeNotImplementedYet = 130;

    }
}
