using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity
{
    /// <summary>
    /// ActionUrl的实体类,对HTTP加密服务请求的ACTION URL
    /// </summary>
    public class ActionUrl
    {

        public string strPostAddress { get; set; }
        public string strHttpPort { get; set; }
        public string strId { get; set; }
        public string strPackageID { get; set; }
        public string strSendTime { get; set; }
        public string strInterfaceVersion { get; set; }
        public string strCis { get; set; }
        public string strTransCode { get; set; }

        public ActionUrl()
        {

        }
    }
}
