using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity.QueryReturnEntity
{
    public class QpdRetrun0010
    {
        private QpdPub objQpdPub = new QpdPub() ;

        private QpdOut objQpdOut = new QpdOut();

        private List<rd> listQpdRd = new List<rd>();

        public QpdPub QpdPub { get { return objQpdPub; } set { this.objQpdPub = value; } }

        public QpdOut QpdOut { get { return objQpdOut; } set { this.objQpdOut = value; } }

        public List<rd> ListQpdRd { get { return listQpdRd; } set { this.listQpdRd = value; } }
     
        //public override string ToString()
        //{
        //    StringBuilder sbQpdReturnXml = new StringBuilder();
        //    StringBuilder sbTotalRd = new StringBuilder();
        //    sbQpdReturnXml.Append("<?xml version='1.0' encoding = 'GBK'?><CMS><eb><pub>");
        //    sbQpdReturnXml.Append(objQpdPub.ToString()+ "</pub><out>");
        //    sbQpdReturnXml.Append(objQpdOut.ToString());
        //    foreach (QpdRd objQpdRd in listQpdRd)
        //    {
        //        sbTotalRd.Append("<rd>" + objQpdRd.ToString()+"</rd>");
        //    }
        //    sbQpdReturnXml.Append(sbTotalRd.ToString());
        //    sbQpdReturnXml.Append("</out></eb></CMS>");
        //    return sbQpdReturnXml.ToString();
        //}

    }
}
