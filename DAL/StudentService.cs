using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DAL.Helper;
using Models;
using DAL;

namespace DAL
{
    /// <summary>
    /// 学生信息数据访问类
    /// </summary>
    public class StudentService
    {
        #region 添加学员对象
        /// <summary>
        /// 检查学员身份证号是否存在
        /// </summary>
        /// <param name="studentIdNo">学员身份证号</param>
        /// <returns>
        /// true：存在；false：不存在
        /// </returns>
        public bool IsIdNoExistd(string studentIdNo)
        {
            string sql = "select count(*) from Students where StudentIdNo = '{0}'";
            sql = string.Format(sql, studentIdNo);

            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if(result == 1)
            {
                return true;
            }
            else return false;
        }

        public int AddStudent(Student objStudent)
        {
                        // 1 编写sql
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("insert into Students (StudentName,Gender,Birthday,StudentIdNo,Age,PhoneNumber,StudentAddress,ClassId) ");
            
            // ⚠️ 关键修改：给 {0} {1} {6} 前面都加上 N
            sqlBuilder.Append("values(N'{0}', N'{1}', '{2}', '{3}', {4}, '{5}', N'{6}', {7})");
            // 2 解析对象
            string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender, objStudent.Birthday, objStudent.StudentIdNo, objStudent.Age, objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.ClassId);
            // 3 提交到数据库
            try
            {
               return SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
               throw new Exception("添加学员失败,数据库操作异常，错误信息：" + ex.Message);
            }
        }
        #endregion

    }
}
