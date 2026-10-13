// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;

// namespace StudentManagerPro.Common
// {
//     public class StringSecurity
//     {
//         /// <summary>
//         /// 使用SHA1加密字符串
//         /// </summary>
//         /// <param name="input">要加密的字符串</param>
//         /// <returns>加密后的字符串</returns>
//         public static string EncryptSHA1(string input)
//         {
//             using (SHA1 sha1 = SHA1.Create())
//             {
//                 byte[] inputBytes = Encoding.UTF8.GetBytes(input);
//                 byte[] hashBytes = sha1.ComputeHash(inputBytes);
//                 return BitConverter.ToString(hashBytes).Replace("-", "");
//             }
//         }

//         /// <summary>
//         /// 使用DES加密字符串
//         /// </summary>
//         /// <param name="input">要加密的字符串</param>
//         /// <returns>加密后的字符串</returns>
//         public static string EncryptDES(string input)
//         {
//             return input;
//         }
//     }
// }
