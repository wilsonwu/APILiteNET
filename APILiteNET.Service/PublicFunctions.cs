using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace APILiteNET.Service
{
    public static class PublicFunctions
    {
        private static string DESKey = "APILiteNET";
        private static char[] charSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static bool IsEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public static string URLDecode(string strEncoded)
        {
            return strEncoded == null ? null : HttpUtility.UrlDecode(strEncoded);
        }

        public static string URLEncode(string strOriginal)
        {
            return strOriginal == null ? null : HttpUtility.UrlEncode(strOriginal);
        }

        public static string Base64Encode(string strOriginal)
        {
            string strModified = null;
            if (!string.IsNullOrEmpty(strOriginal))
            {
                byte[] byt = System.Text.Encoding.UTF8.GetBytes(strOriginal);
                strModified = Convert.ToBase64String(byt);
            }
            return strModified;
        }

        public static string Base64Decode(string strModified)
        {
            string strOriginal = null;
            if (!string.IsNullOrEmpty(strModified))
            {
                byte[] b = Convert.FromBase64String(strModified);
                strOriginal = UTF8Encoding.UTF8.GetString(b, 0, b.Length);
            }
            return strOriginal;
        }

        public static string DecimalToBinary(int decimalNumber)
        {
            string binaryNumber = Convert.ToString(decimalNumber, 2);
            return binaryNumber;
        }

        public static int BinaryToDecimal(string binaryNumber)
        {
            int decimalNumber = Convert.ToInt32(binaryNumber, 2);
            return decimalNumber;
        }

        public static string DESEncode(string pToEncrypt)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(DESKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(DESKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string DESDecode(string pToDecrypt)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(DESKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(DESKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }

        public static string Base62Encode(long value)
        {
            string sixtyNum = string.Empty;
            if (value < 62)
            {
                sixtyNum = charSet[value].ToString();
            }
            else
            {
                long result = value;
                while (result > 0)
                {
                    long val = result % 62;
                    sixtyNum = charSet[val] + sixtyNum;
                    result = result / 62;
                }
            }
            return sixtyNum;
        }

        public static string MD5(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            var md5 = System.Security.Cryptography.MD5.Create();
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(data);

            var buffer = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                buffer.AppendFormat("{0:X2}", hash[i]);
            }

            return buffer.ToString();
        }

        private static int GetCurrentGreenwichHourIn24()
        {
            int thisHour = Convert.ToInt32((DateTimeOffset.Now + DateTimeOffset.Now.Offset).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 2));
            return thisHour;
        }

        public static string GetClassName(Type dataClass)
        {
            string className = null;
            string codeClassName = dataClass.Name;
            if (codeClassName.IndexOf("_") > 0)
            {
                className = codeClassName.Substring(codeClassName.IndexOf("_") + 1);
            }
            else
            {
                className = codeClassName;
            }
            return className;
        }

        public static long[] GetPermissionsByPermissionCode(int permissionCode)
        {
            string bPermissionCode = PublicFunctions.DecimalToBinary(permissionCode);
            long[] permissions = new long[bPermissionCode.Length];
            for (int i = 0; i < bPermissionCode.Length; i++)
            {
                long currentBinaryPermission = Convert.ToInt32(bPermissionCode.Substring(bPermissionCode.Length - i - 1));
                for (int j = 0; j < i; j++)
                {
                    currentBinaryPermission = currentBinaryPermission - permissions[j];
                }
                permissions[i] = currentBinaryPermission;
            }
            for (int k = 0; k < bPermissionCode.Length; k++)
            {
                permissions[k] = PublicFunctions.BinaryToDecimal(permissions[k].ToString());
            }

            return permissions;
        }

        public static string GetFixedDigitNumber(long number, int digit)
        {
            string result = null;
            if (number > 0)
            {
                result = number.ToString().PadLeft(digit, '0');
            }
            return result;
        }

        public static string GetUploadFolder(string baseFolder)
        {
            string yearFolder = DateTime.Now.Year.ToString();
            string monthFolder = DateTime.Now.Month.ToString();
            string folder = "/" + baseFolder + "/" + yearFolder + "/" + monthFolder;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(folder)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folder));
            }
            return folder;
        }

        public static string GetAppSetting(string appSettingKey)
        {
            return System.Configuration.ConfigurationManager.AppSettings[appSettingKey].ToString();
        }

        public static string HttpPost(string postURL, NameValueCollection headerData, NameValueCollection formData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (headerData != null)
            {
                foreach (string key in headerData.Keys)
                {
                    request.Headers.Add(key, headerData[key]);
                }
            }
            List<string> formDataList = new List<string>();

            if (formData != null)
            {
                foreach (string key in formData.Keys)
                {
                    string tempData = key + "=" + formData[key].ToString();
                    formDataList.Add(tempData);
                }
            }

            string postData = string.Join("&", formDataList);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return result;
        }

        public static void HttpUploadFile(string url, string file, Stream fileStream, string paramName, string contentType, NameValueCollection headerData, NameValueCollection formData)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            if (headerData != null)
            {
                foreach (string key in headerData.Keys)
                {
                    wr.Headers.Add(key, headerData[key]);
                }
            }

            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (formData != null)
            {
                foreach (string key in formData.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, formData[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            wresp = wr.GetResponse();
            Stream stream2 = wresp.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);
        }

        public static string GetContentType(string filePath)
        {
            Bitmap bm = (Bitmap)Image.FromFile(HttpContext.Current.Server.MapPath(filePath));
            Bitmap tmp = new Bitmap(bm.Width, bm.Height);
            Graphics gInput = Graphics.FromImage(tmp);
            ImageFormat thisFormat = tmp.RawFormat;
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(filePath).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        public static Size GetThumbnailSize(Image original, int thumbnailSize)
        {
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)thumbnailSize / originalWidth;
            }
            else
            {
                factor = (double)thumbnailSize / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }
}