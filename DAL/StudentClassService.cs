using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Models;
using System.Data.SqlClient;
using System.Data;
using DAL.Helper;
namespace DAL
{
    /// <summary>
    /// 班级数据访问类
    /// </summary>
    public class StudentClassService
    {
        /// <summary>
        /// 获取所有班级
        /// </summary>
        /// <returns></returns>
        public List<StudentClass> GetAllClasses() {
            string sql = "select ClassName,ClassId from StudentClass";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentClass> list = new List<StudentClass>();
            while(objReader.Read())
            {
                list.Add(new StudentClass()
                {
                    ClassName = objReader["ClassName"].ToString(),
                    ClassId = Convert.ToInt32(objReader["ClassId"])
                });
            }
            objReader.Close();
            return list;




        }

    }
}
