using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WinformAM.Tool
{
    class Tools
    {

        /*************************************************************************/
        /*描    述：从给定的一串字符中提取出位于开始标识符和结束标识符之间的所有字符
         *输入参数：[string str:待处理的字符串]
         *          [string startFlag:信息起始标识串]
         *          [char endFlag:信息结束标识字符]
         *返 回 值：【string】
         *开发日期：2012-11-20
         */
        public static string StringExtractor(string str, string startFlag, char endFlag)
        {
            try
            {
                if (str == null) return null;
                StringBuilder sb = new StringBuilder();
                int startPos = str.IndexOf(startFlag);
                if (startPos == -1) return null;
                startPos += startFlag.Length;
                while (startPos < str.Length && str[startPos] != endFlag)
                {
                    sb.Append(str[startPos]);
                    ++startPos;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        public static string StringExtractor(string str, string startFlag, char endFlag, int startIndex)
        {
            try
            {
                if (str == null) return null;
                StringBuilder sb = new StringBuilder();
                int startPos = str.IndexOf(startFlag, startIndex);
                if (startPos == -1) return null;
                startPos += startFlag.Length;
                while (startPos < str.Length && str[startPos] != endFlag)
                {
                    sb.Append(str[startPos]);
                    ++startPos;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// 从str中获得从startIndex开始的startFlag和endFlag之前的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startFlag">开始标识</param>
        /// <param name="endFlag">接受标识</param>
        /// <param name="startIndex">开始偏移量</param>
        /// <returns></returns>
        public static string StringExtractor(string str, string startFlag, string endFlag, int startIndex)
        {
            try
            {
                if (str == null) return null;
                StringBuilder sb = new StringBuilder();
                int startPos = str.IndexOf(startFlag, startIndex);
                if (startPos == -1) return null;
                startPos += startFlag.Length;
                while (startPos + endFlag.Length < str.Length)
                {
                    int cnt = 0;
                    for (int i = 0; i < endFlag.Length; i++)
                    {
                        if (str[startPos + i] == endFlag[i])
                            ++cnt;
                    }
                    if (cnt == endFlag.Length) break;
                    sb.Append(str[startPos]);
                    ++startPos;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        public static string StringBackExtractor(string str, string endFlag, string startFlag, int endIndex)
        {
            try
            {
                if (str == null || endIndex == -1) return null;
                StringBuilder sb = new StringBuilder();
                int endPos = str.IndexOf(endFlag, endIndex);
                if (endPos == -1) return null;

                while (endPos - startFlag.Length >= 0)
                {
                    int cnt = 0;
                    int j = endPos - startFlag.Length;
                    for (int i = 0; i < startFlag.Length; i++)
                    {
                        if (str[j + i] == startFlag[i])
                            ++cnt;
                        else
                            break;
                    }
                    if (cnt == startFlag.Length)
                    {
                        return str.Substring(endPos, endIndex - endPos);
                    }
                    else
                        --endPos;
                }
                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        /*************************************************************************/
        /*描    述：从给定的一串字符中提取出位于开始标识符和结束标识符之间的所有字符
         *输入参数：[string str:待处理的字符串]
         *          [string startFlag:信息起始标识串]
         *          [char endFlag:信息结束标识字符]
         *返 回 值：【string】
         *开发日期：2012-11-20
         */
        public static string PageDownloader(string url)
        {
            try
            {
                string result = "";
                string err = "";
                HttpClient httpClient = new HttpClient();
                result = httpClient.GetSrc(url, "UTF-8", out err);
                return result;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }

        public static string HttpPageDownloadToFile(string url, string filePath)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2)";
                WebResponse res = req.GetResponse();
                System.IO.Stream stream = res.GetResponseStream();
                byte[] buffer = new byte[32 * 1024];
                int bytesProcessed = 0;
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                int bytesRead;
                do
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, bytesRead);
                    bytesProcessed += bytesRead;
                }
                while (bytesRead > 0);
                fs.Flush();
                fs.Close();
                res.Close();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "success";
        }

        /*************************************************************************/
        /*描    述：将指定内容写到指定文件中
         *输入参数：[string filePath:文件路径]
         *          [string fileText:待写的文件内容]
         *返 回 值：【bool:写文件成功返回True,反之则返回false】
         *开发日期：2012-11-20
         */
        public static bool FileWirter(string filePath, string fileText)
        {
            try
            {
                if (File.Exists(filePath) == false || File.ReadAllText(filePath).Length == 0)
                {
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(fileText);
                    sw.Close();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        /*************************************************************************/
        /*描    述：判断文件是否存在并不为空文件
         *输入参数：[string filePath:文件路径]
         *返 回 值：【bool:文件存在返回True,反之则返回false】
         *开发日期：2012-11-20
         */
        public static bool FileExsits(string filePath)
        {
            try
            {
                if (File.Exists(filePath) == true && File.ReadAllText(filePath).Length > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        /*************************************************************************/
        /*描    述：去除字符串str中非法的字符，只保留英文字母、数字、空格、下划线
         *输入参数：[string str:待处理字符]
         *返 回 值：【string】
         *开发日期：2012-11-20
         */
        public static string RemoveInvalidChar(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= '0' && str[i] <= '9') || str[i] == ' ' || str[i] == '_')
                {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }

        /*************************************************************************/
        /*描    述：阻止线程，时长milliseconds
         *输入参数：[int milliseconds]
         *返 回 值：
         *开发日期：2012-11-20
         */
        public static void Sleep(int milliseconds)
        {
            if (milliseconds < 0) milliseconds = 0;
            System.Threading.Thread.Sleep(milliseconds);
        }

        /*************************************************************************/
        /*描    述：删去括号中的字符
         *输入参数：[string str]待处理字符串
         *          [char leftParent]左括号
         *          [char rightParent]右括号
         *返 回 值：string
         *开发日期：2012-11-20
         */
        public static string DeleteStringInParent(string str, char leftParent, char rightParent)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string text = str;

                int leftPos = text.IndexOf(leftParent);
                int rightPos = 0;
                while (leftPos != -1)
                {
                    rightPos = text.IndexOf(rightParent, leftPos);
                    if (rightPos == -1) break;
                    sb.Remove(0, sb.Length);
                    sb.Append(text.Substring(0, leftPos));
                    sb.Append(text.Substring(rightPos + 1, text.Length - rightPos - 1));

                    text = sb.ToString();
                    leftPos = text.IndexOf(leftParent);
                }

                return text;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return str;
            }
        }

        public static string ReadFile(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string text = sr.ReadToEnd();
                sr.Close();

                return text;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        public static bool PDFDownload(string url,string pdfFilePath,out string message)
        {
            bool result = false;
            message = "";
            try
            {
                WebClient pdfClient = new WebClient();
                string localPath = pdfFilePath;
                pdfClient.DownloadFile(url, localPath);
                result=true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = false;
            }
            return result;
        }

        public static string PDFToText(string pdfPath)
        {
            using (FileStream fs = new FileStream(pdfPath, FileMode.Open))
            {

                StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("UTF-8"));

                string text = sr.ReadToEnd();
                sr.Close();
                return text;
            }
        }

        /// <summary>
        /// 从指定的字符串中获得用空格隔开的两个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IList<string> GetISSNAndEISSN(string str)
        {
            IList<string> strList = new List<string>(); 
            int j = 0;
            for(int i = 0; i < 2; i++)
            {
                StringBuilder sb = new StringBuilder();
                while (j < str.Length && !IsLegalChar(str[j]))
                    j++;
                while (j < str.Length && IsLegalChar(str[j]))
                {
                    sb.Append(str[j]);
                    j++;
                }
                if (sb.Length > 0)
                    strList.Add(sb.ToString());        
            }

            return strList;
        }

        public static bool IsLegalChar(char s)
        {
            if (s == ' ' || s == '\n' || s == '\r')
                return false;
            return true;
        }

        public static string DeleteBeginAndEndIllegalChar(string str)
        {
            int i = 0;
            while (i < str.Length && !IsLegalChar(str[i]))
                i++;
            int j = str.Length - 1;
            while (j >= 0 && !IsLegalChar(str[j]))
                j--;
            if (j >= i)
            {
                return str.Substring(i, j - i + 1);
            }
            else
                return null;
        }

        public static string GetFolderName(string url)
        {
            int i;
            for(i = 0; i < url.Length; i++)
            {
                if(url[i] == '?')
                    break;
            }
            return url.Substring(i+1);
        }
    }
}
