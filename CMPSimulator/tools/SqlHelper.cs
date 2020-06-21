using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace CMPSimulator.tools
{
    /// <summary>
    ///  通用数据访问类
    /// </summary>
    class SqlHelper
    {

            // private static readonly string connString = "Server=.;DataBase=StudentManageDB;Uid=sa;Pwd=password01!";
            public static readonly string connString =
                ConfigurationManager.ConnectionStrings["connString"].ToString();

            //private static readonly string connString =
            //    Common.StringSecurity.DESDecrypt(ConfigurationManager.ConnectionStrings["connString"].ToString());

            /// <summary>
            /// 执行增、删、改（insert/update/delete）
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static int Update(string sql)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            /// <summary>
            /// 执行增、删、改操作（带参数的SQL语句）
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public static int Update(string sql, SqlParameter[] param)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    cmd.Parameters.AddRange(param);  // 添加参数数组
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            public static int UpdateByProcedure(string procedureName, SqlParameter[] param)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand();
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;// 设置当前的操作是执行存储过程
                    cmd.CommandText = procedureName; // 设置存储过程参数
                    cmd.Parameters.AddRange(param);  // 添加参数数组
                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            /// <summary>
            /// 执行单一结果查询（select）
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static object GetSingleResult(string sql)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            /// <summary>
            /// 执行多结果查询（select）
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static SqlDataReader GetReader(string sql)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    SqlDataReader objReader =
                        cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return objReader;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    throw ex;
                }
            }
            /// <summary>
            /// 执行返回数据集的查询
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static DataSet GetDataSet(string sql)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd); //创建数据适配器对象
                DataSet ds = new DataSet();//创建一个内存数据集
                try
                {
                    conn.Open();
                    da.Fill(ds);  //使用数据适配器填充数据集
                    return ds;  //返回数据集
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            /// <summary>
            /// 获取服务器的时间
            /// </summary>
            /// <returns></returns>
            public static DateTime GetServerTime()
            {
                return Convert.ToDateTime(GetSingleResult("select getdate()"));
            }

            /// <summary>
            /// 启用事务同时提交多条SQL语句
            /// </summary>
            /// <param name="sqlList">sql语句集合</param>
            /// <returns>返回bool类型，代表事务执行是否成功</returns>
            public static bool UpdateByTran(List<string> sqlList)
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();

                    cmd.Transaction = conn.BeginTransaction();//开启事务
                                                              //循环执行Sql语句
                    foreach (var itemSql in sqlList)
                    {
                        cmd.CommandText = itemSql;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Transaction.Commit();//提交事务（真正的从数据库中修改了数据）
                    return true;
                }
                catch (Exception ex)
                {
                    if (cmd.Transaction != null)
                        cmd.Transaction.Rollback();//回滚事务(返回撤消“正常的任务”)
                    throw new Exception("调用事务方法出现错误：" + ex.Message);
                    throw;
                }
                finally
                {
                    if (cmd.Transaction != null)
                    {
                        cmd.Transaction = null;//清空事务
                    }
                    conn.Close();
                }
            }
       
    }
}
