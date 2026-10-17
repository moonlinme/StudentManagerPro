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

#region 根据班级名称获取班级下的所有学员
    /// <summary>
    /// 根据班级名称获取班级下的所有学员
    /// </summary>
    /// <param name="className">班级名称</param>
    /// <returns>
    /// 所有学员的列表
    /// </returns>
    public List<StudentExt> GetStudentByClassName(string className)
            {
                string sql = "select StudentName,StudentId,Gender,Birthday,ClassName from Students";
                sql += " inner join StudentClass on Students.ClassId = StudentClass.ClassId";
                sql += " where ClassName=N'{0}'";
                sql = string.Format(sql, className);
                SqlDataReader objReader = SQLHelper.GetReader(sql);
                List<StudentExt> list = new List<StudentExt>();
                while(objReader.Read())
                {
                    list.Add(new StudentExt()
                    {
                        StudentId = Convert.ToInt32(objReader["StudentId"]),
                        StudentName = Convert.ToString(objReader["StudentName"]),
                        Gender = Convert.ToString(objReader["Gender"]),
                        Birthday = Convert.ToDateTime(objReader["Birthday"]),
                        ClassName = Convert.ToString(objReader["ClassName"]),
                    });
                }
                objReader.Close();
                return list;
            }
    #endregion

    #region 根据学员Id获取学员信息
    /// <summary>
    /// 根据学员Id获取学员信息
    /// </summary>
    /// <param name="studentId">学员Id</param>
    /// <returns>
    /// 学员信息对象
    /// </returns>
    public StudentExt GetStudentById(int studentId)
    {
        string sql = "select StudentName,StudentId,Gender,Birthday,ClassName from Students";
        sql += " inner join StudentClass on Students.ClassId = StudentClass.ClassId";
        sql += " where StudentId={0}";
        sql = string.Format(sql, studentId);
        SqlDataReader objReader = SQLHelper.GetReader(sql);
        StudentExt objStudent = new StudentExt();
        if(objReader.Read())
        {
                objStudent = new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = Convert.ToString(objReader["StudentName"]),
                    Gender = Convert.ToString(objReader["Gender"]),
                    Birthday = Convert.ToDateTime(objReader["Birthday"]),
                    ClassName = Convert.ToString(objReader["ClassName"]),
                };
        }
        objReader.Close();
        return objStudent;
    }

    #endregion
 #region 修改学员对象
    /// <summary>
    /// 检查学员Id号是否已存在
    /// </summary>
    /// <param name="studentIdNo">学员Id号</param>
    /// <param name="studentId">学员Id</param>
    /// <returns>
    /// 是否已存在
    /// </returns>
 public bool IsIdNoExistd(string studentIdNo,string studentId)
        {
            string sql = "select count(*) from Students where StudentIdNo = '{0}' and StudentId<>{1}";
            sql = string.Format(sql, studentIdNo,studentId);

            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if(result == 1)
            {
                return true;
            }
            else return false;
        }


    /// <summary>
    /// 修改学员对象
    /// </summary>
    /// <param name="objStudent">学员对象</param>
    /// <returns>
    /// 受影响的行数
    /// </returns>
    public int ModifyStudent(Student objStudent)
    {
       StringBuilder sqlBuilder = new StringBuilder();
       sqlBuilder.Append("update Students set StudentName=N'{0}',Gender=N'{1}',Birthday='{2}',StudentIdNo=N'{3}',Age={4},PhoneNumber=N'{5}',StudentAddress=N'{6}',ClassId={7} where StudentId={8}");
       string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender, objStudent.Birthday, objStudent.StudentIdNo, objStudent.Age, objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.ClassId, objStudent.StudentId);
      
      try
      {
        return SQLHelper.Update(sql);
      }
      catch (Exception ex)
      {
        throw new Exception("修改学员失败,数据库操作异常，错误信息：" + ex.Message);
      }

    }
    #endregion

    #region 删除学员对象
    /// <summary>
    /// 删除学员对象
    /// </summary>
    /// <param name="studentId">学员Id</param>
    /// <returns>
    /// 受影响的行数
    /// </returns>
    public int DeleteStudentById(int studentId)
    {
        string sql = "delete from Students where StudentId={0}";
        sql = string.Format(sql, studentId);
        try
        {
            return SQLHelper.Update(sql);
        }
        catch (Exception ex)
        {
            if(ex.Message.Contains(" FOREIGN KEY constraint"))
            {
                throw new Exception("删除学员失败,该学员已被引用，不能删除");
            }
            throw new Exception("删除学员失败,数据库操作异常，错误信息：" + ex.Message);
        }
    }

    #endregion
   
        }
    }
   
