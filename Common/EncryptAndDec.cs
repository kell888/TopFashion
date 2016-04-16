using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace TopFashion
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public sealed class EncryptAndDec
    {
        #region  MD5加密
        /// <summary>
        /// MD5加密算法[.NET类库中自带的算法 MD5是个不可逆的算法 没有解密的算法]
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5EncryptLGBB(string text)
        {
            return Encrypt1(text, "LGBBCRMW");
        }
        public static string MD5Encrypt(string text, string key)
        {
            return Encrypt1(text, key);
        }

        //MD5具体加密方法
        private static string Encrypt1(string text, string skey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(skey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(skey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        #endregion

        #region DES加密/解密
        /// <summary>
        /// DES数据加密标准，速度较快，适用于加密大量数据的场合
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <returns></returns>
        public static string DESCEncrypt(string text)
        {
            return Encrypt2(text, "LGBBCRMW");
        }
        public static string DESCEncrypt(string text, string key)
        {
            return Encrypt2(text, key);
        }
        //DES加密 加密密钥必须为8位
        private static string Encrypt3(string text, string key)
        {
            string content = "";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            content = ret.ToString();
            return content;
        }

        /// <summary>
        /// DES加密，支持中文
        /// </summary>
        /// <param name="pToEncrypt">加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Encrypt2(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);  

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static string DESDencrypt(string text)
        {
            return Decrypt2(text, "LGBBCRMW");
        }
        public static string DESDencrypt(string text, string key)
        {
            return Decrypt2(text, key);
        }

        /// <summary>
        /// DES解密具体实现，不支持中文
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns> 
        private static string Decrypt3(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] outputByteArray = new byte[text.Length / 2];
            for (int i = 0; i < text.Length / 2; i++)
            {
                int x = Convert.ToInt32(text.Substring(i * 2, 2), 16);
                outputByteArray[i] = (byte)x;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(outputByteArray, 0, outputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.ASCII.GetString(ms.ToArray());
        }

        /// <summary>
        /// DES解密方法，支持中文
        /// </summary>
        /// <param name="pToDecrypt">加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string Decrypt2(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get  the  decrypted  data  back  from  the  memory  stream  
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region  DES文件加密/解密
        private static string iv = "CRMWLGBB";
        private static string key = "LGBBCRMW";
        /// <summary>  
        /// DES加密偏移量，必须是>=8位长的字符串  
        /// </summary>  
        public static string IV
        {
            get { return iv; }
            set { iv = value; }
        }
        /// <summary>  
        /// DES加密的私钥，必须是8位长的字符串  
        /// </summary>  
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }
        /// <summary>  
        /// 对文件内容进行DES加密  
        /// /// </summary>  
        /// <param name="sourceFile">待加密的文件绝对路径</param>  
        /// <param name="destFile">加密后的文件保存的绝对路径</param>  
        public static void EncryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIV = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
        /// <summary>  
        /// 对文件内容进行DES加密，加密后覆盖掉原来的文件 
        /// </summary>  
        /// <param name="sourceFile">待加密的文件的绝对路径</param>  
        public static void EncryptFile(string sourceFile)
        {

            EncryptFile(sourceFile, sourceFile);

        }
        /// <summary>  
        /// 对文件内容进行DES解密
        /// </summary>  
        /// <param name="sourceFile">待解密的文件绝对路径</param>  
        /// <param name="destFile">解密后的文件保存的绝对路径</param>  
        public static void DecryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIV = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }

                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>   
        /// 对文件内容进行DES解密，加密后覆盖掉原来的文件   
        /// </summary>   
        /// <param name="sourceFile">待解密的文件的绝对路径</param> 
        public static void DecryptFile(string sourceFile)
        {
            DecryptFile(sourceFile, sourceFile);
        }

        #endregion
    }
}
