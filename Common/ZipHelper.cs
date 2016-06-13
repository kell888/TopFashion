using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Data;
using System.IO.Compression;

namespace TopFashion
{
    /// <summary>
    /// 数据压缩解压缩辅助类
    /// </summary>
    public class ZipHelper
    {  
        /// <summary>
        /// 将DataTable格式化成字节数组byte[]
        /// </summary>
        /// <param name="dsOriginal">DataTable对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatData(DataTable dsOriginal)
        {
            byte[] binaryDataResult = null;
            if (dsOriginal != null && dsOriginal.Rows.Count > 0)
            {
                MemoryStream memStream = new MemoryStream();
                IFormatter brFormatter = new BinaryFormatter();
                dsOriginal.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dsOriginal);
                binaryDataResult = memStream.ToArray();
                memStream.Close();
                memStream.Dispose();
            }
            return binaryDataResult;
        }

        /// <summary>
        /// 将DataTable格式化成字节数组byte[]，并且已经经过压缩
        /// </summary>
        /// <param name="dsOriginal">DataTable对象</param>
        /// <returns>经过压缩的字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(DataTable dsOriginal)
        {
            byte[] binaryDataResult = null;
            if (dsOriginal != null && dsOriginal.Rows.Count > 0)
            {
                MemoryStream memStream = new MemoryStream();
                IFormatter brFormatter = new BinaryFormatter();
                dsOriginal.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dsOriginal);
                binaryDataResult = memStream.ToArray();
                memStream.Close();
                memStream.Dispose();
            }
            return Compress(binaryDataResult);
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            if (data != null)
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                GZipStream stream = new GZipStream(ms, CompressionMode.Decompress, true);
                byte[] buffer = new byte[1024];
                MemoryStream temp = new MemoryStream();
                int read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    temp.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                //必须把stream流关闭才能返回ms流数据,不然数据会不完整
                stream.Close();
                stream.Dispose();
                ms.Close();
                ms.Dispose();
                bData = temp.ToArray();
                temp.Close();
                temp.Dispose();
                return bData;
            }
            else
                return null;
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            if (data != null)
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
                stream.Dispose();
                //必须把stream流关闭才能返回ms流数据,不然数据会不完整
                //并且解压缩方法stream.Read(buffer, 0, buffer.Length)时会返回0
                bData = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return bData;
            }
            else
                return null;
        }

        /// <summary>
        /// 将字节数组反序列化成DataTable对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>DataTable对象</returns>
        public static DataTable RetrieveDataSet(byte[] binaryData)
        {
            DataTable dsOriginal = null;
            if (binaryData != null)
            {
                MemoryStream memStream = new MemoryStream(binaryData);
                IFormatter brFormatter = new BinaryFormatter();
                Object obj = brFormatter.Deserialize(memStream);
                dsOriginal = (DataTable)obj;
            }
            return dsOriginal;
        }

        /// <summary>
        /// 将字节数组反解压后序列化成DataTable对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>DataTable对象</returns>
        public static DataTable RetrieveDataSetDecompress(byte[] binaryData)
        {
            DataTable dsOriginal = null;
            if (binaryData != null)
            {
                MemoryStream memStream = new MemoryStream(Decompress(binaryData));
                IFormatter brFormatter = new BinaryFormatter();
                Object obj = brFormatter.Deserialize(memStream);
                dsOriginal = (DataTable)obj;
            }
            return dsOriginal;
        }

        /// <summary>
        /// 将object格式化成字节数组byte[]
        /// </summary>
        /// <param name="dsOriginal">object对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatData(object dsOriginal)
        {
            byte[] binaryDataResult = null;
            if (dsOriginal != null)
            {
                MemoryStream memStream = new MemoryStream();
                IFormatter brFormatter = new BinaryFormatter();
                brFormatter.Serialize(memStream, dsOriginal);
                binaryDataResult = memStream.ToArray();
                memStream.Close();
                memStream.Dispose();
            }
            return binaryDataResult;
        }

        /// <summary>
        /// 将objec格式化成字节数组byte[]，并压缩
        /// </summary>
        /// <param name="dsOriginal">object对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(object dsOriginal)
        {
            byte[] binaryDataResult = null;
            if (dsOriginal != null)
            {
                MemoryStream memStream = new MemoryStream();
                IFormatter brFormatter = new BinaryFormatter();
                brFormatter.Serialize(memStream, dsOriginal);
                binaryDataResult = memStream.ToArray();
                memStream.Close();
                memStream.Dispose();
            }
            return Compress(binaryDataResult);
        }

        /// <summary>
        /// 将字节数组反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static object RetrieveObject(byte[] binaryData)
        {
            if (binaryData != null)
            {
                MemoryStream memStream = new MemoryStream(binaryData);
                IFormatter brFormatter = new BinaryFormatter();
                Object obj = brFormatter.Deserialize(memStream);
                return obj;
            }
            else
                return null;
        }

        /// <summary>
        /// 将字节数组解压后反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static object RetrieveObjectDecompress(byte[] binaryData)
        {
            if (binaryData != null)
            {
                MemoryStream memStream = new MemoryStream(Decompress(binaryData));
                IFormatter brFormatter = new BinaryFormatter();
                Object obj = brFormatter.Deserialize(memStream);
                return obj;
            }
            else
                return null;
        }

        /// <summary>
        /// 解密配置文件并读入到xmldoc中
        /// </summary>
        public static XmlNode DecryptConfigFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            XmlDocument m_XmlDoc = new XmlDocument();
            BinaryFormatter formatter = null;
            try
            {
                formatter = new BinaryFormatter();
                m_XmlDoc.LoadXml(EncryptAndDec.Decrypt2((string)formatter.Deserialize(fs), "TIANSHUN"));
                return m_XmlDoc.DocumentElement;
            }
            catch (SerializationException e)
            {
                WriteLog.CreateLog("内部操作", AppDomain.CurrentDomain.BaseDirectory, "ZipHelper类>DecryptConfigFile方法", "反序列化失败，原因:" + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                fs = null;
            }
        }

        /// <summary>
        /// 加密密钥后再对文件字符进行加密
        /// </summary>
        public static void EncryptConfigFile(string filePath, string str)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(fs, EncryptAndDec.Encrypt2(str, "TIANSHUN"));
            }
            catch (SerializationException e)
            {
                WriteLog.CreateLog("内部操作", AppDomain.CurrentDomain.BaseDirectory, "ZipHelper类>EncryptConfigFile方法", "序列化失败，原因: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                fs = null;
            }
        }
    }
}
