using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinformAM.Model;
using System.IO;
using System.Threading;
using WinformAM.Tool;
using WinformAM.BLL;
using System.Collections;

namespace WinformAM
{
    public partial class MainForm : Form
    {
        #region var
        private static string directoryPath;
        private static string[] webUrl;
        private static List<Journal> journalList;
        private static string newLine;
        private static string twoLine;
        private static int normalInterval_min;
        private static int normalInterval_max;
        private static int abnormalInterval_min;
        private static int abnormalInterval_max;
        private static int yearStart;
        private static int yearEnd;
        private static char firstLetter_min;
        private static char firstLetter_max;
        private Random randObj;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            string[] str =  {    
                                ""
                            };
            webUrl = str;
            directoryPath = @"d:\httpwww.doaj.orgdoaj\";
            newLine = System.Environment.NewLine;
            twoLine = newLine + newLine;
            journalList = new List<Journal>();
            randObj = new Random();
            normalInterval_min = 10;
            normalInterval_max = 60;
            abnormalInterval_max = 3600;
            abnormalInterval_min = 600;
            firstLetter_max = 'Z';
            firstLetter_min = 'A';
            yearStart = 2010;
            yearEnd = DateTime.Now.Year;
            textBoxYearEnd.Text = yearEnd.ToString();

        }

        #region NoNeedReviseRegion

        /*************************************************************************/
        //加载主界面
        //2013-3-17
        /*************************************************************************/
        private void MainForm_Load(object sender, EventArgs e)
        {
            textBoxDisplay.AppendText("Please select a path for files saved!" + newLine);
            textBoxDisplay.AppendText("Default path:" + directoryPath + newLine);
            try
            {
                if (Directory.Exists(directoryPath) == false)
                    Directory.CreateDirectory(directoryPath);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            buttonSettings.Enabled = true;
            buttonDownloadJournal.Enabled = false;
            buttonSaveToDatabase.Enabled = true;
        }

        /*************************************************************************/
        //下载所给路径下的页面到指定路径
        //2013-3-17
        /*************************************************************************/
        private bool DownloadJournalList(string directoryPath, string[] webUrl, int count)
        {
            try
            {
                if (webUrl.Length == 1)
                {
                    string folderName = Tools.GetFolderName(webUrl[0]);
                    if (Directory.Exists(directoryPath) == false)
                        Directory.CreateDirectory(directoryPath);
                    string filePath = directoryPath + folderName;
                    int num = count / 10;
                    if (count % 10 > 0)
                        num++;
                    for (int i = 1; i < num + 1; i++)
                    {
                        string url = webUrl[0] + "&page=" + i;
                        if (Tools.FileExsits(filePath + i + ".html") == false)
                        {
                            textBoxDisplay.AppendText(url + " is downloading..." + newLine);
                            try
                            {
                                //InsertNormalInterval();
                                Tools.FileWirter(filePath + i + ".html", Tools.PageDownloader(url));
                            }
                            catch(Exception e)
                            {

                            }
                            textBoxDisplay.AppendText(url + " Downloaded" + newLine);
                        } 
                    }                    
                }
                else if (webUrl.Length > 1)
                {
                    if (Directory.Exists(directoryPath) == false)
                        Directory.CreateDirectory(directoryPath);
                    string filePath = directoryPath;
                    if (Tools.FileExsits(filePath) == false)
                    {
                        FileStream fs = new FileStream(filePath, FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        for (int i = 0; i < webUrl.Length; i++)
                        {
                            sw.Write(Tools.PageDownloader(webUrl[i]));
                        }
                        sw.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 下载页面
        /// </summary>
        /// TODO 需要从新指定路径webUrl
        private void ProcessForButtonDownloadJournal()
        {
            CategoryWebsiteBLL categoryWebsiteBLL = new CategoryWebsiteBLL();
            IList<CategoryWebsite> categoryWebsiteList = categoryWebsiteBLL.GetAllCategoryWebsites();
            foreach (CategoryWebsite cate in categoryWebsiteList)
            {
                webUrl[0] = cate.Website;
                if (DownloadJournalList(directoryPath, webUrl, cate.WebsiteCount) == true)
                {
                    textBoxDisplay.AppendText(webUrl[0] + " Downloaded!" + newLine + newLine);
                }
                else
                {
                    textBoxDisplay.AppendText(webUrl[0] + " is already existed!" + newLine);

                }
            }

            textBoxDisplay.AppendText("All Pages Downloaded!" + newLine + newLine);

            buttonSaveToDatabase.Enabled = true;
            //journalList = ExtractJournals(directoryPath, webUrl);
            buttonDownloadJournal.Enabled = true;
            buttonSaveToDatabase.Enabled = true;
            buttonSettings.Enabled = false;
            buttonDownloadJournal.Text = "Download Journal List";
        }


        private void textBoxDisplay_DoubleClick(object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// 设置时间间隔
        /// </summary>
        private void InsertNormalInterval()
        {
            try
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                normalInterval_min = Convert.ToInt32(textBox1.Text);
                normalInterval_max = Convert.ToInt32(textBox2.Text);
                if (normalInterval_min > normalInterval_max)
                {
                    int tmp = normalInterval_max;
                    normalInterval_max = normalInterval_min;
                    normalInterval_min = tmp;
                }
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                normalInterval_min = 10;
                normalInterval_max = 60;
            }
            finally
            {
                int normalInterval = randObj.Next(normalInterval_min, normalInterval_max);
                textBoxDisplay.AppendText("Wait for " + normalInterval + " seconds!" + newLine);
                Tools.Sleep(normalInterval * 1000);
            }
        }

        /// <summary>
        /// 设置时间间隔
        /// </summary>
        private void InsertAbnormalInterval()
        {
            try
            {
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                abnormalInterval_min = Convert.ToInt32(textBox3.Text);
                abnormalInterval_max = Convert.ToInt32(textBox4.Text);
                if (abnormalInterval_max < abnormalInterval_min)
                {
                    int tmp = abnormalInterval_min;
                    abnormalInterval_min = abnormalInterval_max;
                    abnormalInterval_max = tmp;
                }
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                abnormalInterval_min = 600;
                abnormalInterval_max = 3600;
            }
            finally
            {
                int abnormalInterval = randObj.Next(abnormalInterval_min, abnormalInterval_max);
                textBoxDisplay.AppendText("Wait for " + abnormalInterval + " seconds!" + newLine);
                Tools.Sleep(abnormalInterval * 1000);
            }
        }

        /// <summary>
        /// 读取标题首字母要求
        /// </summary>
        private void ReadFirstLetterScope()
        {
            try
            {
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                firstLetter_min = textBox5.Text[0];
                firstLetter_max = textBox6.Text[0];
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString());
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                firstLetter_max = 'Z';
                firstLetter_min = 'A';
            }
        }

        /// <summary>
        /// 读取文章时间要求
        /// </summary>
        private void ReadYearScope()
        {
            try
            {
                textBoxYearStart.ReadOnly = true;
                textBoxYearEnd.ReadOnly = true;
                yearStart = Convert.ToInt32(textBoxYearStart.Text);
                yearEnd = Convert.ToInt32(textBoxYearEnd.Text);
                if (yearStart > yearEnd)
                {
                    int tmp = yearEnd;
                    yearEnd = yearStart;
                    yearStart = tmp;
                }
                textBoxYearEnd.ReadOnly = false;
                textBoxYearStart.ReadOnly = false;
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
                textBoxYearEnd.ReadOnly = false;
                textBoxYearStart.ReadOnly = false;
                yearStart = 2010;
                yearEnd = DateTime.Now.Year;
            }
        }

        /// <summary>
        /// 判断标题首字母是否在指定的范围内
        /// </summary>
        /// <param name="journalName">文章名字</param>
        /// <param name="min">起始字母</param>
        /// <param name="max">结束字母</param>
        /// <returns>在范围内返回true，不是返回false</returns>
        private bool IsInScope(string journalName, char min, char max)
        {
            char firstLetter = Char.ToUpper(journalName[0]);
            min = Char.ToUpper(min);
            max = Char.ToUpper(max);
            if (min > max)
            {
                char temp = min;
                min = max;
                max = temp;
            }
            if (firstLetter >= min && firstLetter <= max)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// 从下载到本地的期刊页面中读取所有需要的信息
        /// </summary>
        /// <param name="directoryPath">下面下载后所在路径</param>
        /// <param name="webUrl">页面网址（决定了所在的本地文件夹）</param>
        /// <returns>信息列表</returns>
        private List<Journal> ExtractJournals(string directoryPath, string[] webUrl)
        {
            JournalBLL journalBLL = new JournalBLL();
            List<Journal> jnlInfoList = new List<Journal>();
            try
            {
                if (Tools.FileExsits(webUrl[0]) == false)
                    return null;
                string text = Tools.ReadFile(webUrl[0]);

                int curPos = 0;
                string journalFlag = "<div class=\"data\">";
                while (curPos < text.Length)
                {
                    curPos = text.IndexOf(journalFlag, curPos);
                    if (curPos == -1)
                        break;

                    Journal jnlInfo = new Journal();
                    jnlInfo.JournalName = Tools.DeleteStringInParent(Tools.StringExtractor(text, "<b>", "</b>", curPos), '<', '>').Trim();
                    jnlInfo.JournalName = Tools.DeleteBeginAndEndIllegalChar(jnlInfo.JournalName);
                    //ISSN and EISSN
                    curPos = text.IndexOf("<div style=\"color: #585858\">", curPos);
                    string temp = Tools.StringExtractor(text, "</strong>:", "<br>", curPos);
                    IList<string> ISSNAndEISSN = Tools.GetISSNAndEISSN(Tools.DeleteBeginAndEndIllegalChar(temp));
                    if (ISSNAndEISSN.Count == 2)
                    {
                        jnlInfo.ISSN = ISSNAndEISSN[0];
                        jnlInfo.EISSN = ISSNAndEISSN[1];
                    }
                    else if (ISSNAndEISSN.Count == 1)
                    {
                        jnlInfo.ISSN = ISSNAndEISSN[0];
                    }
                    //publisher
                    curPos = text.IndexOf("<br>", curPos);
                    jnlInfo.Publisher = Tools.DeleteBeginAndEndIllegalChar(Tools.StringExtractor(text, "</strong>:", "<br>", curPos));
                    //subject
                    curPos = text.IndexOf("<br><strong>Subject</strong>", curPos);
                    int nextPos = text.IndexOf("<br><b>Country</b>", curPos);
                    string subjectStr = "";
                    while (curPos < nextPos)
                    {
                        string temp2 = Tools.DeleteBeginAndEndIllegalChar(Tools.StringExtractor(text, "\">", "</a>", curPos));
                        if (text.IndexOf(temp2) > nextPos)
                            break;
                        else
                            subjectStr += temp2 + ",";
                        curPos = text.IndexOf("\">", curPos) + 2;
                    }
                    if (subjectStr.Length != 0)
                        subjectStr = subjectStr.Substring(0, subjectStr.Length - 1);
                    jnlInfo.Subject = subjectStr;
                    curPos = nextPos;
                    //country
                    //language
                    curPos = text.IndexOf("<b>Language</b>", curPos);
                    jnlInfo.JournalLanguage = Tools.DeleteBeginAndEndIllegalChar(Tools.StringExtractor(text, "</b>:", "</br>", curPos));
                    //start year
                    curPos = text.IndexOf("<b>Start year</b>", curPos);
                    jnlInfo.StartYear = Tools.DeleteBeginAndEndIllegalChar(Tools.StringExtractor(text, "</b>", "</b>", curPos));
                    //fee
                    curPos = text.IndexOf("<b>Publication fee</b>", curPos);
                    jnlInfo.PublicationFee = Tools.DeleteBeginAndEndIllegalChar(Tools.StringExtractor(text, "\">", "</span>", curPos));

                    jnlInfoList.Add(jnlInfo);
                }

            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
            }
            return jnlInfoList;
        }

        /// <summary>
        /// 将期刊信息保存到数据库
        /// </summary>
        private void ProcessForButtonSavetoDatabase()
        {
            //if (journalList == null) return;
            CategoryWebsiteBLL categoryWebsiteBLL = new CategoryWebsiteBLL();
            IList<CategoryWebsite> categoryWebsiteList = categoryWebsiteBLL.GetAllCategoryWebsites();
            foreach(CategoryWebsite cate in categoryWebsiteList)
            {
                webUrl[0] = cate.Website;
                int count = cate.WebsiteCount;
                int id = cate.CategoryWebsiteID;
                int num = count / 10;
                if (count % 10 > 0)
                    num++;
                string folderName = Tools.GetFolderName(webUrl[0]);
                string filePath = directoryPath + folderName;
                for(int i = 0; i < num; i++)
                {
                    webUrl[0] = filePath + (i + 1) + ".html";
                    journalList = ExtractJournals(directoryPath, webUrl);
                    JournalBLL journalBusiness = new JournalBLL();
                    foreach (Journal jnlInfo in journalList)
                    {
                        try
                        {
                            jnlInfo.CategoryWebsiteID = id;
                            journalBusiness.Add(jnlInfo);
                        }
                        catch (Exception ex_for1)
                        {
                            continue;
                            textBoxDisplay.AppendText(ex_for1.ToString() + newLine);
                        }
                        textBoxDisplay.AppendText(twoLine);
                    }
                    textBoxDisplay.AppendText(twoLine + "All Journals are saved!" + twoLine);
                    buttonSaveToDatabase.Text = "Conserve To Database";
                }
            }

            //paperBusiness.RemoveDuplicate();
            //authorBusiness.RemoveDuplicate();
            buttonSaveToDatabase.Enabled = true;
            buttonDownloadJournal.Enabled = true;
            buttonSettings.Enabled = true;
        }


        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            buttonDownloadJournal.Enabled = false;
            buttonSaveToDatabase.Enabled = false;

            foldBrowserForFilePath.Description = "Please select a folder";
            foldBrowserForFilePath.RootFolder = Environment.SpecialFolder.MyComputer;
            foldBrowserForFilePath.ShowNewFolderButton = true;
            DialogResult dialogResult = foldBrowserForFilePath.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                directoryPath = foldBrowserForFilePath.SelectedPath;
                if (directoryPath[directoryPath.Length - 1] != '\\')
                    directoryPath = directoryPath + "\\";
                textBoxDisplay.AppendText("The folder you selected is :" + directoryPath + System.Environment.NewLine);
            }
            textBoxDisplay.AppendText(newLine + "Please set the interval time !" + newLine);
            buttonDownloadJournal.Enabled = true;
            buttonSaveToDatabase.Enabled = false;
            buttonSettings.Enabled = true;
        }

        private void buttonSaveToDatabase_Click(object sender, EventArgs e)
        {
            buttonDownloadJournal.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            buttonSettings.Enabled = false;
            buttonSaveToDatabase.Text = "Conserving……";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(ProcessForButtonSavetoDatabase));
            thread.Start();
        }

        /*************************************************************************/
        /*描    述：清空显示板
         *输入参数：
         *返 回 值：
         *开发日期：2012-11-20
         */
        private void buttonClearScreen_Click(object sender, EventArgs e)
        {
            textBoxDisplay.Clear();
        }

        /*************************************************************************/
        /*描    述：退出程序
         *输入参数：
         *返 回 值：
         *开发日期：2012-11-20
         */
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            System.Environment.Exit(0);
        }

        //private void buttonDownloadPapers_Click(object sender, EventArgs e)
        //{
        //    buttonDownloadJournal.Enabled = false;
        //    buttonDownloadPapers.Enabled = false;
        //    buttonDownloadPapers.Text = "Paperlist is downloading……";
        //    buttonSettings.Enabled = false;
        //    buttonSaveToDatabase.Enabled = false;
        //    Thread thread = new Thread(new ThreadStart(ProcessForButtonDownloadPapers));
        //    thread.Start();
        //}

        private void buttonDownloadJournal_Click(object sender, EventArgs e)
        {
            buttonSettings.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            buttonDownloadJournal.Enabled = false;
            buttonDownloadJournal.Text = "Journal List downloading……";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread threadDownloadJournal = new Thread(new ThreadStart(ProcessForButtonDownloadJournal));
            threadDownloadJournal.Start();
        }
        private bool IsNum(string str)
        {
            foreach (char ch in str)
            {
                if (ch < '0' || ch > '9')
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 获取两个字符串之间的字符
        /// </summary>
        /// <returns></returns>
        private string Search_string(string s, string s1, string s2)
        {
            try
            {
                int n1, n2;
                n1 = s.IndexOf(s1, 0) + s1.Length;
                n2 = s.IndexOf(s2, n1);
                return s.Substring(n1, n2 - n1);
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(newLine + ex.Message);
                return "";
            }
        }
    }
}
