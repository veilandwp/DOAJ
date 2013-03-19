using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAM.Model
{
    public class JournalInfo
    {
        public JournalInfo()
        {
            this.JournalISSN = "0";
            this.JournalName = "0";
            this.JournalEstablishedYear = "0";
            this.JournalPaperNumbers = "0";
            this.JournalUrl = "0";
        }

        /*期刊数据库内编号ID*/
        private int _id;
        public int JournalID
        {
            get { return _id; }
            set { _id = value; }
        }

        /*期刊ISSN*/
        private string _issn;
        public string JournalISSN
        {
            get { return _issn; }
            set { _issn = value; }
        }

        /*期刊名称*/
        private string _name;
        public string JournalName
        {
            get { return _name; }
            set { _name = value; }
        }

        /*期刊主页链接*/
        private string _url;
        public string JournalUrl
        {
            get { return _url; }
            set { _url = value; }
        }

        /*期刊创办年份*/
        private string _year;
        public string JournalEstablishedYear
        {
            get { return _year; }
            set { _year = value; }
        }

        /*期刊包含文章数量*/
        private string _paperNumbers;
        public string JournalPaperNumbers
        {
            get { return _paperNumbers; }
            set { _paperNumbers = value; }
        }

        public override string ToString()
        {
            string newLine = System.Environment.NewLine;
            return "Journal Name:\t" + this.JournalName + newLine +
                   "Journal ISSN:\t" + this.JournalISSN + newLine +
                   "Established Year:\t" + this.JournalEstablishedYear + newLine +
                   "Including Paper Numbers:\t" + this.JournalPaperNumbers + newLine +
                   "Journal URL:\t" + this.JournalUrl + newLine;
        }
    }
}
