using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 学员扩展类
    /// </summary>
     public class StudentExt : Student
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}
