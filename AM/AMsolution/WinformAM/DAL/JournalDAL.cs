using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.Model;
using System.Data.SqlClient;
using WinformAM.DBUtility;
using System.Reflection;
using System.Data;

namespace WinformAM.DAL
{
    public partial class JournalDAL
    {
        private const string SQL_INSERT_JOURNAL = "insert into T_Journal(JournalName,ISSN,EISSN,Publisher,Subject,JournalLanguage,StartYear,PublicationFee,CategoryWebsiteID) values(@JournalName,@ISSN,@EISSN,@Publisher,@Subject,@JournalLanguage,@StartYear,@PublicationFee,@CategoryWebsiteID)";

        public bool Add(Journal model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ISSN", SqlDbType.NVarChar,32),
					new SqlParameter("@EISSN", SqlDbType.NVarChar,32),
					new SqlParameter("@Publisher", SqlDbType.NVarChar,1024),
					new SqlParameter("@Subject", SqlDbType.NVarChar,1024),
					new SqlParameter("@JournalLanguage", SqlDbType.NVarChar,128),
                    new SqlParameter("@StartYear", SqlDbType.NVarChar,32),
					new SqlParameter("@PublicationFee", SqlDbType.NVarChar,32),
					new SqlParameter("@CategoryWebsiteID", SqlDbType.Int,4),
                    new SqlParameter("@JournalName", SqlDbType.NVarChar, 1024)
                                        };
            parameters[0].Value = model.ISSN;
            parameters[1].Value = model.EISSN;
            parameters[2].Value = model.Publisher;
            parameters[3].Value = model.Subject;
            parameters[4].Value = model.JournalLanguage;
            parameters[5].Value = model.StartYear;
            parameters[6].Value = model.PublicationFee;
            parameters[7].Value = model.CategoryWebsiteID;
            parameters[8].Value = model.JournalName;

            StringBuilder sb = new StringBuilder();
            sb.Append(SQL_INSERT_JOURNAL);
            bool result =new DBHelperSQL().ExecuteNonQuery(sb, parameters, MethodInfo.GetCurrentMethod().Name);
            return result;
        }
    }
}
