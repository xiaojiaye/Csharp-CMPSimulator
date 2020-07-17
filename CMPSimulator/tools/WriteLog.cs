using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CMPSimulator.tools
{
    public static class WriteLog
    {
        /// <summary>
        /// 根据 日志内容、接口类型及版本号、指令包id
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="busType">接口类型及版本号</param>
        /// <param name="packageId">指令包id</param>
        public static void writeLog(string msg,string busType,string packageId)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            try
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("【业务类型】" + busType + "\r\n" + "【指令包序号】" + packageId + "\r\n" + msg);
                    sw.WriteLine("【本次操作记录完毕】" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("【日志写入异常】：" + e.Message);
                    sw.WriteLine("【异常发生时间】：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        /// <summary>
        /// 手动保存一次日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="packageId">指令包ID</param>
        public static void writeLogByOne(string msg,string packageId)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "手动保存.txt";
            try
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("【业务类型】" + "\r\n" + "【指令包序号】" + packageId + "\r\n" + msg);
                    sw.WriteLine("【本次操作记录完毕】" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("【日志写入异常】：" + e.Message);
                    sw.WriteLine("【异常发生时间】：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine("****************************************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

    }
}
