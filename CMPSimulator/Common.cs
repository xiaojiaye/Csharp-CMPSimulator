using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator
{
    public class Common
    {
        public string CreatePackageId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public string CreateSignTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}
