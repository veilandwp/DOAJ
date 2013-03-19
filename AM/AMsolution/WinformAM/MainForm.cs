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
        private static List<JournalInfo> journalList;
        private static List<PaperInfo> paperList;
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
                                "http://www.dovepress.com/browse_journals.php?sort_by=alpha&search_str=&order_by=alpha&display_style=stack&alpha="
                            };
            webUrl = str;
            directoryPath = @"D:\Journals\httpwww.dovepress.combrowse_journals.php\";
            newLine = System.Environment.NewLine;
            twoLine = newLine + newLine;
            journalList = new List<JournalInfo>();
            paperList = new List<PaperInfo>();
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
            buttonDownloadPapers.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
        }

        /*************************************************************************/
        //下载所给路径下的页面到指定路径
        //2013-3-17
        /*************************************************************************/
        private bool DownloadJournalList(string directoryPath, string[] webUrl)
        {
            try
            {
                if (webUrl.Length == 1)
                {
                    if (Directory.Exists(directoryPath) == false)
                        Directory.CreateDirectory(directoryPath);
                    string filePath = directoryPath;
                    if (Tools.FileExsits(filePath) == false)
                    {
                        Tools.FileWirter(filePath, Tools.PageDownloader(webUrl[0]));
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
        /// 下载二级目录下的所有页面
        /// </summary>
        /// TODO 需要从新指定路径webUrl
        private void ProcessForButtonDownloadJournal()
        {
            textBoxDisplay.AppendText("Journal List is downloading..." + newLine);
            if (DownloadJournalList(directoryPath, webUrl) == true)
            {
                textBoxDisplay.AppendText("Downloaded!" + newLine);
                buttonDownloadPapers.Enabled = true;
            }
            else
            {
                textBoxDisplay.AppendText("The Journal List is already existed!" + newLine);
                buttonDownloadPapers.Enabled = false;
            }
            buttonDownloadJournal.Enabled = true;
            buttonDownloadPapers.Enabled = true;
            buttonSaveToDatabase.Enabled = false;
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
        private List<JournalInfo> ExtractJournals(string directoryPath, string[] webUrl)
        {
            JournalBLL journalBLL = new JournalBLL();
            List<Journal> jnlInfoList = new List<Journal>();
            try
            {
                string journalListFilePath = directoryPath;
                if (Tools.FileExsits(journalListFilePath) == false)
                    return null;
                string text = Tools.ReadFile(journalListFilePath);

                int curPos = 0;
                string journalFlag = "<div class=\"data\">";
                while (curPos < text.Length)
                {
                    curPos = text.IndexOf(journalFlag, curPos);
                    if (curPos == -1)
                        break;

                    Journal jnlInfo = new Journal();
                    jnlInfo.JournalName = Tools.DeleteStringInParent(Tools.StringExtractor(text, "<b>", "</b>", curPos), '<', '>').Trim();
                    //ISSN and EISSN
                    curPos = text.IndexOf("<div style=\"color: #585858\">", curPos);
                    string temp = Tools.StringExtractor(text, "</strong>:", "<br>", curPos);
                    IList<string> ISSNAndEISSN = Tools.GetISSNAndEISSN(temp);
                    if(ISSNAndEISSN.Count == 2)
                    {
                        jnlInfo.ISSN = ISSNAndEISSN[0];
                        jnlInfo.EISSN = ISSNAndEISSN[1];
                    }
                    else if(ISSNAndEISSN.Count == 1)
                    {
                        jnlInfo.ISSN = ISSNAndEISSN[0];
                    }
                    //publisher
                    curPos = text.IndexOf("<br>", curPos);
                    jnlInfo.Publisher = Tools.StringExtractor(text, "</strong>:", "<br>", curPos);
                    //subject
                    curPos = text.IndexOf("<br>", curPos);
                    int nextPos = text.IndexOf("<br>", curPos);
                    string subjectStr = "";
                    while(curPos + 4 < nextPos)
                    {
                        subjectStr += Tools.StringExtractor(text, ">", "</a>", curPos) + " ";
                        curPos = text.IndexOf("</a>", curPos);
                    }
                    jnlInfo.Subject = subjectStr;
                    curPos = nextPos;
                    //country
                    //language
                    jnlInfo.JournalLanguage = Tools.StringExtractor(text, "</b>:", "</br>", curPos);
                    //start year
                    curPos = text.IndexOf("</br>", curPos);
                    jnlInfo.StartYear = Tools.StringExtractor(text, "</b>:", "</b>", curPos);
                    //fee
                    curPos = text.IndexOf("<span", curPos);
                    jnlInfo.PublicationFee = Tools.StringExtractor(text, ">", "</span>", curPos);

                    jnlInfoList.Add(jnlInfo);
                }

            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
            }
            return jnlInfoList;
        }

        
        private void ProcessForButtonDownloadPapers()
        {
            textBoxDisplay.AppendText("The journal List is downloaded,please wait……" + newLine);
            //
            journalList = this.ExtractJournals(directoryPath, webUrl);
            try
            {
                ReadFirstLetterScope();
                foreach (JournalInfo j in journalList)
                {
                    try
                    {
                        #region Download Journal issue index
                        if (IsInScope(j.JournalName, firstLetter_min, firstLetter_max) == false) continue;
                        textBoxDisplay.AppendText(j.ToString());
                        textBoxDisplay.AppendText("The paperlist of Journal<" + j.JournalName + "> is downloading..." + newLine);
                        string journalDirectoryPath = directoryPath + Tools.RemoveInvalidChar(j.JournalName);

                        if (Directory.Exists(journalDirectoryPath) == false)
                            Directory.CreateDirectory(journalDirectoryPath);

                        string journalIndexFilePath = journalDirectoryPath + "\\" + "Index.html";
                        if (Tools.FileExsits(journalIndexFilePath) == false)
                        {
                            InsertNormalInterval();
                            Tools.FileWirter(journalIndexFilePath, Tools.PageDownloader(j.JournalUrl));
                            textBoxDisplay.AppendText("Index of <" + j.JournalName + "> is downloaded." + newLine);
                        }
                        else
                        {
                            textBoxDisplay.AppendText("Index of <" + j.JournalName + "> is exsited." + newLine);
                        }
                        #endregion
                    }
                    catch (Exception ex_for)
                    {
                        textBoxDisplay.AppendText(ex_for.ToString() + newLine);
                    }
                    textBoxDisplay.AppendText(twoLine);
                }

            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
            }
            buttonDownloadPapers.Enabled = true;
            buttonDownloadPapers.Text = "Download Paper List";
            buttonDownloadJournal.Enabled = true;
            buttonSaveToDatabase.Enabled = true;
            buttonSettings.Enabled = false;
        }


        /*************************************************************************/
        /*描    述：提取期刊j的所有文章信息
         *输入参数：[string directoryPath]
         *          [string[] webUrl]
         *返 回 值：List<PaperInfo>
         *开发日期：2012-11-20
         */
        private List<PaperInfo> ExtractPapers(string directoryPath, JournalInfo j)
        {
            string homeURL = "http://www.dovepress.com/";
            PapersBLL paperBLL = new PapersBLL();
            JournalsBLL journalBLL = new JournalsBLL();
            List<PaperInfo> paperInfoList = new List<PaperInfo>();
            try
            {
                //if (IsInScope(j.JournalName, firstLetter_min, firstLetter_max) == false) return null;

                string journalDirectoryPath = directoryPath + Tools.RemoveInvalidChar(j.JournalName);
                string journalIndexFilePath = journalDirectoryPath + "\\" + "Index.html";

                if (Tools.FileExsits(journalIndexFilePath) == true)
                {
                    #region Extract Volumeslist from Index.html and download

                    #region Set Year Scope
                    int yearMax = yearStart;
                    int yearMin = yearEnd;

                    string volumelisttext = Tools.ReadFile(journalIndexFilePath);

                    string volumelistYeartext = Tools.StringExtractor(volumelisttext, "<a href='#'>Article Archive</a>", "</li></ul></li>", 0);
                    string flagYear = "<li>";
                    int curPos = volumelistYeartext.IndexOf(flagYear);
                    int curYear = 0;
                    while (curPos != -1)
                    {
                        string year = Tools.StringExtractor(volumelistYeartext, "'>", "Volume", curPos);
                        if (year == null)
                        {
                            break;
                        }
                        if (IsNum(year))
                        {
                            curYear = Convert.ToInt32(year);
                        }
                        if (curYear < yearMin)
                        {
                            yearMin = curYear;
                        }
                        if (curYear > yearMax)
                        {
                            yearMax = curYear;
                        }
                        curPos += flagYear.Length + year.Length;
                        curPos = volumelistYeartext.IndexOf(flagYear, curPos);
                    }

                    ReadYearScope();
                    if (yearMax > yearEnd)
                    {
                        yearMax = yearEnd;
                    }
                    if (yearMin < yearStart)
                    {
                        yearMin = yearStart;
                    }
                    #endregion

                    textBoxDisplay.AppendText("Papers published in year[" + yearMin + "," + yearMax + "] will be downloaded!" + newLine);
                    for (int i = yearMin; i <= yearMax; i++)
                    {
                        string issueUrl = j.JournalUrl + "/" + i.ToString();
                        string issueDirectoryPath = journalDirectoryPath + "\\" + i.ToString();
                        if (Directory.Exists(issueDirectoryPath) == false)
                        {
                            Directory.CreateDirectory(issueDirectoryPath);
                        }
                    }
                    #endregion
                    #region Download and Extract Papers
                    int volumePos = 0;
                    for (int i = yearMax; i >= yearMin; i--)
                    {
                        string issueDirectoryPath = journalDirectoryPath + "\\" + i.ToString();
                        volumePos = volumelistYeartext.IndexOf(flagYear);

                        while(volumePos != -1)
                        {
                            string textIssue = Tools.StringExtractor(volumelistYeartext, flagYear, "</li>", volumePos);
                            volumePos += flagYear.Length + textIssue.Length;
                            volumePos = volumelistYeartext.IndexOf(flagYear,volumePos);

                            string flagUrl = "<a";
                            int posUrl = textIssue.IndexOf(flagUrl);
                            if(posUrl != -1)
                            {
                                string sufixUrl = Tools.StringExtractor(textIssue, "href='", "'", posUrl);
                                string vol = Tools.StringExtractor(textIssue, "'>", "</a>", posUrl);
                                if (vol == null)
                                {
                                    break;
                                }
                                if (vol.Substring(0, 4) != i.ToString())
                                {
                                    continue;
                                }
                                string paperListUrl =homeURL+sufixUrl;
                                string paperListDirectoryPath = issueDirectoryPath + "\\" + vol;

                                if (Directory.Exists(paperListDirectoryPath) == false)
                                {
                                    Directory.CreateDirectory(paperListDirectoryPath);
                                }

                                string paperListFilePath = paperListDirectoryPath + "\\List_" + vol + ".html";
                                if (Tools.FileExsits(paperListFilePath) == false)
                                {
                                    InsertNormalInterval();
                                    Tools.FileWirter(paperListFilePath, Tools.PageDownloader(paperListUrl));
                                    textBoxDisplay.AppendText(paperListFilePath + " is downloaded!" + newLine);
                                }
                                else
                                {
                                    textBoxDisplay.AppendText(paperListFilePath + " is exsited!" + newLine);
                                }

                                if (Tools.FileExsits(paperListFilePath) == true)
                                {
                                    string textPaperList = Tools.ReadFile(paperListFilePath);
                                    string flagPaperUrl = "</b> <br />";
                                    int posPaperUrl = textPaperList.IndexOf(flagPaperUrl);
                                    while (posPaperUrl != -1)
                                    {
                                       
                                        string paperSufixUrl = Tools.StringExtractor(textPaperList, "<a href=\"", '"', posPaperUrl);
                                      
                                        string paperUrl =homeURL+ paperSufixUrl;
                                        posPaperUrl = textPaperList.IndexOf(flagPaperUrl, posPaperUrl + flagPaperUrl.Length + paperSufixUrl.Length);

                                        string paperFilePath = paperListDirectoryPath + "\\paper_" + Tools.RemoveInvalidChar(paperSufixUrl) + ".html";
                                        if (Tools.FileExsits(paperFilePath) == false)
                                        {
                                            InsertNormalInterval();
                                            //Tools.FileWirter(paperFilePath, Tools.PageDownloader(paperUrl));
                                            Tools.HttpPageDownloadToFile(paperUrl, paperFilePath);
                                            textBoxDisplay.AppendText(paperFilePath + " is downloaded!" + newLine);
                                        }
                                        else
                                        {
                                            textBoxDisplay.AppendText(paperFilePath + " is exsited!" + newLine);
                                        }

                                        #region Read Paper Information
                                        if (Tools.FileExsits(paperFilePath) == true)
                                        {
                                                 string paperText = Tools.ReadFile(paperFilePath);
                                            try
                                            {
                                                string paperPDFFlag = "<a onClick=\"_gaq.push(['_trackPageview','Virtual/Twitter-Share']);";
                                                int paperPDFPos = paperText.IndexOf(paperPDFFlag);
                                                if (paperPDFPos == -1)
                                                { 
                                                    goto html;
                                                }
                                                string paperPDFURL = Tools.StringExtractor(paperText, "href='", "'", paperPDFPos);
                                                string paperPDFFullURL =homeURL+ paperPDFURL;
                                                string paperPDFFilePath = paperListDirectoryPath + "\\paper_" + Tools.RemoveInvalidChar(paperPDFURL) + ".pdf";
                                                if (Tools.FileExsits(paperPDFFilePath) == false)
                                                {
                                                    string message = "";
                                                    bool result = Tools.PDFDownload(paperPDFFullURL, paperPDFFilePath, out message);
                                                    if (!result)
                                                    {
                                                        textBoxDisplay.AppendText(paperPDFFullURL + " is failure!" + newLine + message + newLine);
                                                    }
                                                    else
                                                    {
                                                        textBoxDisplay.AppendText(paperPDFFilePath + " is downloaded!" + newLine);
                                                    }

                                                }
                                                else
                                                {
                                                    textBoxDisplay.AppendText(paperPDFFilePath + " is exsited!" + newLine);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                textBoxDisplay.AppendText(ex.Message + newLine);
                                            }
                                        html:
                                            PaperInfo newpaper = new PaperInfo();

                                            AuthorInfo newauthor = new AuthorInfo();
                                            newpaper.JournalName = j.JournalName;
                                            newpaper.PaperUrl = paperUrl;
                                            newpaper.PaperFullUrl = paperUrl;



                                            string pTitleFlag = "<title>";
                                            int pTitlePos = paperText.IndexOf(pTitleFlag);
                                            if (pTitlePos != -1)
                                            {
                                                newpaper.PaperTitle = Tools.StringExtractor(paperText, pTitleFlag, "</title>", pTitlePos);
                                            }

                                            string pDoiFlag = "<strong>DOI: </strong>";
                                            int pDoiPos = paperText.IndexOf(pDoiFlag);
                                            if (pDoiPos != -1)
                                            {
                                                newpaper.PaperDoi = Tools.StringExtractor(paperText, pDoiFlag, "</div>", pDoiPos);

                                            }

                                            string pAbsFlag = "<strong>Abstract</strong>:";
                                            int pAbsPos = paperText.IndexOf(pAbsFlag);
                                            if (pAbsPos != -1)
                                            {
                                                newpaper.PaperAbstract = Tools.StringExtractor(paperText, pAbsFlag, "<br /><br /><strong>Keywords:</strong>", pAbsPos);
                                            }

                                            string pYearFlag = "<strong>Published Date</strong>";
                                            int pYearPos = paperText.IndexOf(pYearFlag);
                                            if (pYearPos != -1)
                                            {
                                                newpaper.PaperYear = Tools.StringExtractor(paperText, pYearFlag, "<strong>Volume</strong>", pYearPos);
                                            }

                                            string aNameFlag = "<h2>Authors:";
                                            int aNamePos = paperText.IndexOf(aNameFlag);
                                            if (aNamePos != -1)
                                            {
                                               newauthor.AuthorName= Tools.StringExtractor(paperText, aNameFlag, "</h2>", aNamePos);
                                            }

                                            StringBuilder strAuthorEmail = new StringBuilder();
                                            string aEmailFlag = "mailto:";
                                            int aEmailPos = paperText.IndexOf(aEmailFlag);
                                            while (aEmailPos != -1)
                                            {
                                                string authorEmail = Tools.StringExtractor(paperText, aEmailFlag, "\">", aEmailPos);
                                                if (authorEmail == null)
                                                {
                                                    break;
                                                }
                                                aEmailPos += aEmailFlag.Length + authorEmail.Length;
                                                aEmailPos = paperText.IndexOf(aEmailFlag, aEmailPos);
                                                strAuthorEmail.Append(authorEmail + ",");
                                            }
                                            newauthor.AuthorEmail = strAuthorEmail.ToString();

                                            string aAffFlag = "<strong>DOI: </strong>";
                                            int aAffPos = paperText.IndexOf(aAffFlag);
                                            if (aAffPos != -1)
                                            {
                                                newauthor.AuthorAff =Tools.DeleteStringInParent(Tools.StringExtractor(paperText, "</h3>", "<br /><br />", aAffPos),'<','>');
                                            }
                                            newpaper.PaperAuthors.Add(newauthor);
                                            textBoxDisplay.AppendText(newpaper.ToString() + newLine);
                                            textBoxDisplay.AppendText(newauthor.ToString() + newLine);
                                            paperInfoList.Add(newpaper);
                                        }
                                        #endregion

                                    }
                                }

                            }
                        }
                    }
                    #endregion


                }
            }
            catch (Exception ex)
            {
                textBoxDisplay.AppendText(ex.ToString() + newLine);
            }
            return paperInfoList;
        }

        /*************************************************************************/
        /*描    述：保存期刊、文章、作者信息至数据库
         *输入参数：
         *返 回 值：
         *开发日期：2012-11-20
         */
        private void ProcessForButtonSavetoDatabase()
        {

            if (journalList == null) return;

            JournalBLL journalBusiness = new JournalBLL();
            PapersBLL paperBusiness = new PapersBLL();
            AuthorsBLL authorBusiness = new AuthorsBLL();
            foreach (JournalInfo jnlInfo in journalList)
            {
                try
                {
                    journalBusiness.Add(jnlInfo);
                    JournalInfo getJournalInfo = journalBusiness.GetJournalInfoByJournalName(jnlInfo.JournalName);
                    int journalID = getJournalInfo.JournalID;

                    if (IsInScope(jnlInfo.JournalName, firstLetter_min, firstLetter_max) == false) continue;
                    textBoxDisplay.AppendText("Journal<" + jnlInfo.JournalName + ">is conserving to database……" + newLine);

                    string jnlYearScope = getJournalInfo.JournalEstablishedYear;
                    ReadYearScope();
                    int yBegin = yearStart + 1;
                    int yEnd = yearEnd - 1;
                    if (jnlYearScope.Length > 1)
                    {
                        int linePos = jnlYearScope.IndexOf(" ");
                        string ybegin = jnlYearScope.Substring(0, linePos);
                        string yend = jnlYearScope.Substring(linePos + 1);
                        yBegin = Convert.ToInt32(ybegin);
                        yEnd = Convert.ToInt32(yend);
                    }
                    if (yBegin <= yearStart && yEnd >= yearEnd)
                    {
                        textBoxDisplay.AppendText("Papers of year[" + yearStart + "," + yearEnd + "] of Journal<" + jnlInfo.JournalName + "> is already in database!" + twoLine);
                    }

                    if (yBegin > yearStart)
                        yBegin = yearStart;
                    if (yEnd < yearEnd)
                        yEnd = yearEnd;

                    paperList = ExtractPapers(directoryPath, jnlInfo);

                    if (paperList.Count > 0)
                    {
                        foreach (PaperInfo paperInfo in paperList)
                        {
                            try
                            {
                                paperInfo.JournalID = getJournalInfo.JournalID;
                                paperBusiness.Add(paperInfo);
                                JournalInfo newJournalInfo = new JournalInfo();
                                newJournalInfo.JournalName = jnlInfo.JournalName;
                                newJournalInfo.JournalISSN = jnlInfo.JournalISSN;
                                newJournalInfo.JournalEstablishedYear = jnlInfo.JournalEstablishedYear;
                                newJournalInfo.JournalID = journalID;
                                newJournalInfo.JournalUrl = jnlInfo.JournalUrl;
                                newJournalInfo.JournalPaperNumbers = journalBusiness.GetPaperNumber(journalID).ToString();
                                journalBusiness.Update(newJournalInfo);
                                textBoxDisplay.AppendText("Paper<" + paperInfo.PaperTitle + ">is conserved！" + newLine);

                                foreach (AuthorInfo authorInfo in paperInfo.PaperAuthors)
                                {
                                    string[] authorName = new string[] { };
                                    if (authorInfo.AuthorName.IndexOf(',') != -1)
                                    {
                                        authorName = authorInfo.AuthorName.Split(',');
                                    }
                                    else
                                    {
                                        authorName = new string[1] { authorInfo.AuthorName };
                                    }
                                    foreach (string strAuthorInfo in authorName)
                                    {
                                        AuthorInfo addAuthorInfo = new AuthorInfo();
                                        addAuthorInfo.AuthorName = strAuthorInfo;
                                        addAuthorInfo.AuthorAff = authorInfo.AuthorAff;
                                        addAuthorInfo.AuthorEmail = authorInfo.AuthorEmail;
                                        addAuthorInfo.PaperID = paperBusiness.GetPaperIDByUrlTitle(paperInfo);
                                        addAuthorInfo.PaperDoi = paperInfo.PaperDoi;
                                        authorBusiness.Add(addAuthorInfo);
                                        textBoxDisplay.AppendText("Information of Auhtor<" + addAuthorInfo.AuthorName + "> is conserved!" + newLine);
                                    }
                                }
                            }
                            catch (Exception ex_for2)
                            {
                                continue;
                                textBoxDisplay.AppendText(ex_for2.ToString() + newLine);
                            }
                        }

                    }
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
            //paperBusiness.RemoveDuplicate();
            //authorBusiness.RemoveDuplicate();
            buttonSaveToDatabase.Enabled = true;
            buttonDownloadJournal.Enabled = true;
            buttonDownloadPapers.Enabled = true;
            buttonSettings.Enabled = true;
        }


        /*************************************************************************/
        /*描    述：设置期刊文章源代码存取路径
         *输入参数：
         *返 回 值：
         *开发日期：2012-11-20
         */
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            buttonDownloadJournal.Enabled = false;
            buttonDownloadPapers.Enabled = false;
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
            buttonDownloadPapers.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            buttonSettings.Enabled = true;
        }

        private void buttonSaveToDatabase_Click(object sender, EventArgs e)
        {
            buttonDownloadJournal.Enabled = false;
            buttonDownloadPapers.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            buttonSettings.Enabled = false;
            buttonSaveToDatabase.Text = "Conserving……";

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

        private void buttonDownloadPapers_Click(object sender, EventArgs e)
        {
            buttonDownloadJournal.Enabled = false;
            buttonDownloadPapers.Enabled = false;
            buttonDownloadPapers.Text = "Paperlist is downloading……";
            buttonSettings.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            Thread thread = new Thread(new ThreadStart(ProcessForButtonDownloadPapers));
            thread.Start();
        }

        private void buttonDownloadJournal_Click(object sender, EventArgs e)
        {
            buttonSettings.Enabled = false;
            buttonDownloadPapers.Enabled = false;
            buttonSaveToDatabase.Enabled = false;
            buttonDownloadJournal.Enabled = false;
            buttonDownloadJournal.Text = "Journal List downloading……";

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
