using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAM.Model
{
    public class Journal
    {
        public Journal()
        {
            this.journalName = "";
            this.iSSN = "";
            this.eISSN = "";
            this.publisher = "";
            this.subject = "";
            this.journalLanguage = "";
            this.startYear = "";
            this.publicationFee = "";
        }
        private int journalID;

        public int JournalID
        {
            get { return journalID; }
            set { journalID = value; }
        }
        private string journalName;

        public string JournalName
        {
            get { return journalName; }
            set { journalName = value; }
        }

        private string iSSN;

        public string ISSN
        {
            get { return iSSN; }
            set { iSSN = value; }
        }
        private string eISSN;

        public string EISSN
        {
            get { return eISSN; }
            set { eISSN = value; }
        }
        private string publisher;

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }
        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string journalLanguage;

        public string JournalLanguage
        {
            get { return journalLanguage; }
            set { journalLanguage = value; }
        }
        private string startYear;

        public string StartYear
        {
            get { return startYear; }
            set { startYear = value; }
        }
        private string publicationFee;

        public string PublicationFee
        {
            get { return publicationFee; }
            set { publicationFee = value; }
        }
        private int categoryWebsiteID;

        public int CategoryWebsiteID
        {
            get { return categoryWebsiteID; }
            set { categoryWebsiteID = value; }
        }
    }
}
