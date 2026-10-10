using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;

namespace DAL.Helper
{
    /// <summary>
    /// 通用数据访问类
    /// </summary>
   public class SQLHelper
    {
        // ⚠️ 注意：这里的数据库名要和你实际建的一致（你之前建的是 StudentManageDB，不是 StudentManagerDB）
        // private static string connString = "Server=.\\TEW_SQLEXPRESS;Database=StudentManageDB;Uid=sa;Pwd=password01!;";
        public static string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

        /// <summary>
        /// 执行非查询操作，如插入、更新、删除
        /// </summary>
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // 记录日志
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行查询操作，返回单个整数结果
        /// </summary>
        public static int GetSingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                // 修正：将 object 转换为 int，且防止 null 报错
                object result = cmd.ExecuteScalar();
                return result == null ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // 记录日志
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        } // 👈 这里结束了 GetSingleResult 方法，原来的多余括号已删除

        /// <summary>
        /// 执行查询操作，返回数据读取器
        /// </summary>
        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                // CommandBehavior.CloseConnection 会在 DataReader 关闭时自动关闭连接
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                // 记录日志
                throw ex;
            }
        }
    }
}