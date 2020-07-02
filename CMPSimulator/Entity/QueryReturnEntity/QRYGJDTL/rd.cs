using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QRYGJDTL
{
    // srcXml 中的字段个数可以比rd中的属性多，没有的就不会解析。
    // 属性类型都要是string，都要有[XmlElementAttribute("Drcrf")]说明
    // 其中有一个int或者没有xml注解，就好string 转 classObject 失败
    [XmlRootAttribute("rd", IsNullable = false)]
    public class rd
    {
     
    }
}
