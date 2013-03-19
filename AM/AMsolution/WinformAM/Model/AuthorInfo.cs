using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAM.Model
{
    public class AuthorInfo
    {
        public AuthorInfo()
        {
            this.AuthorName = "0";
            this.AuthorEmail = "0";
            this.AuthorAff = "0";
            this.PaperDoi = "0";
            this.PaperID = 0;
        }

        /*作者姓名*/
        private string _authorName;
        public string AuthorName
        {
            get { return _authorName; }
            set { _authorName = value; }
        }

        /*作者电子邮箱地址*/
        private string _authorEmail;
        public string AuthorEmail
        {
            get { return _authorEmail; }
            set { _authorEmail = value; }
        }

        /*作者所属单位信息*/
        private string _authorAff;
        public string AuthorAff
        {
            get { return _authorAff; }
            set { _authorAff = value; }
        }

        /*作者发表文章的DOI*/
        private string _paperDoi;
        public string PaperDoi
        {
            get { return _paperDoi; }
            set { _paperDoi = value; }
        }

        /*作者编号*/
        private int _authorID;
        public int AuthorID
        {
            get { return _authorID; }
            set { _authorID = value; }
        }

        /*作者文章编号*/
        private int _paperID;
        public int PaperID
        {
            get { return _paperID; }
            set { _paperID = value; }
        }

        public override string ToString()
        {
            return "\tAuthorName:\t" + this.AuthorName + System.Environment.NewLine +
                   "\tAuthorEmail:\t" + this.AuthorEmail + System.Environment.NewLine +
                   "\tAuthorAff:\t" + this.AuthorAff + System.Environment.NewLine;
        }
    }
}
