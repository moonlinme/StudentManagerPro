using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using Models;
using DAL.Helper;

namespace DAL
{
    /// <summary>
    /// 系统管理员服务类
    /// </summary>
    public class SysAdminService
    {
        /// <summary>
        /// 系统管理员登录
        /// </summary>
        /// <param name="objAdmin">系统管理员对象</param>
        /// <returns>登录成功返回系统管理员对象，否则返回 null</returns>
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            //组合SQL语句
            string sql = "select AdminName from Admins where LoginId = {0} and LoginPwd = '{1}'";
            sql = string.Format(sql, objAdmin.LoginId, objAdmin.LoginPwd);
            // 从数据库查询
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            if (objReader.Read())
            {
                objAdmin.AdminName = objReader["AdminName"].ToString();
            }
            else
            {
                objAdmin = null;
            }   

            objReader.Close();
            return objAdmin;
        }
    }
}
