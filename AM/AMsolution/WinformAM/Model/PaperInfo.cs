using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAM.Model
{
     public class PaperInfo
    {
        public PaperInfo()
        {
            this.PaperTitle = "0";
            this.PaperDoi = "0";
            this.PaperAbstract = "0";
            this.PaperFullUrl = "0";
            this.JournalName = "0";
            this.JournalISSN = "0";
            this.PaperYear = "0";
	        this.PaperUrl = "0";
            this.PaperAuthors = new List<AuthorInfo>();
            this.AuthorEmail = "0";
        }

        /*文章编号DOI*/
        private string _paperDoi;
        public string PaperDoi
        {
            get { return _paperDoi; }
            set { _paperDoi = value; }
        }

        /*文章标题*/
        private string _paperTitle;
        public string PaperTitle
        {
            get { return _paperTitle; }
            set { _paperTitle = value; }
        }


        /*文章作者*/
        private List<AuthorInfo> _authors;
        public List<AuthorInfo> PaperAuthors
        {
            get { return _authors; }
            set { _authors = value; }
        }
        /*作者邮箱*/
        private string _authorEmail;
        public string AuthorEmail
        {
            get { return _authorEmail; }
            set { _authorEmail = value; }
        }
        /*文章摘要*/
        private string _paperAbstract;
        public string PaperAbstract
        {
            get { return _paperAbstract; }
            set { _paperAbstract = value; }
        }

        /*文章链接*/
        private string _paperUrl;
        public string PaperUrl
        {
            get { return _paperUrl; }
            set { _paperUrl = value; }
        }

        /*文章全文下载地址*/
        private string _paperFullUrl;
        public string PaperFullUrl
        {
            get { return _paperFullUrl; }
            set { _paperFullUrl = value; }
        }

        /*文章发表年份*/
        private string _paperYear;
        public string PaperYear
        {
            get { return _paperYear; }
            set { _paperYear = value; }
        }

        /*文章所属期刊编号ISSN*/
        private string _journalIssn;
        public string JournalISSN
        {
            get { return _journalIssn; }
            set { _journalIssn = value; }
        }

        /*文章所属期刊名*/
        private string _journalName;
        public string JournalName
        {
            get { return _journalName; }
            set { _journalName = value; }
        }

        /*文章数据库内编号ID*/
        private int _id;
        public int PaperID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _journalId;
        public int JournalID
        {
            get { return _journalId; }
            set { _journalId = value; }
        }

        public override string ToString()
        {
            string newLine = System.Environment.NewLine;
            return "PaperTitle:\t" + this.PaperTitle + newLine +
                   "PaperDOI:\t" + this.PaperDoi + newLine +
                //   "PaperAbstract:\t" + this.PaperAbstract + newLine +
                   "PaperYear:\t" + this.PaperYear + newLine +
                   "PaperUrl:\t" + this.PaperUrl + newLine +
                   "JournalName:\t" + this.JournalName + newLine;
        }
    }
}
