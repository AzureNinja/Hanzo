using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanzoProxyAPI.Models
{
    public class VirtualMachineObject
    {
        public string name { get; set; }
        public string resourcegroup { get; set; }
        public string status { get; set; }
    }
}