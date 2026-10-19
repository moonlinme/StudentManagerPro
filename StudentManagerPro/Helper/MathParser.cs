using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace StudentManagerPro.Helper
{
    public static class MathParser
    {
        public static decimal Calculate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return 0;
            try
            {
                DataTable dt = new DataTable();
                var result = dt.Compute(expression, "");
                return Convert.ToDecimal(result);
            }
            catch
            {
                return 0;
            }
        }
    }
}