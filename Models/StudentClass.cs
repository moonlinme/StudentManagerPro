using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class StudentClass
    {
        // 对应数据库里的 ClassId 列
        public int ClassId { get; set; }

        // 对应数据库里的 ClassName 列
        public string ClassName { get; set; }
    }

}
