using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.Model;
using WinformAM.DBUtility;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

namespace WinformAM.DAL
{
    public partial class AuthorsDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(AuthorInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_Authors(");
            strSql.Append("AuthorName,AuthorEmail,AuthorAff,PaperDoi,PaperID)");
            strSql.Append(" values (");
            strSql.Append("@AuthorName,@AuthorEmail,@AuthorAff,@PaperDoi,@PaperID)");
            SqlParameter[] parameters = {
					new SqlParameter("@AuthorName", SqlDbType.NVarChar,256),
					new SqlParameter("@AuthorEmail", SqlDbType.NVarChar,256),
					new SqlParameter("@AuthorAff", SqlDbType.NVarChar,1024),
					new SqlParameter("@PaperDoi", SqlDbType.NVarChar,64),
					new SqlParameter("@PaperID", SqlDbType.Int,4)};
            parameters[0].Value = model.AuthorName;
            parameters[1].Value = model.AuthorEmail;
            parameters[2].Value = model.AuthorAff;
            parameters[3].Value = model.PaperDoi;
            parameters[4].Value = model.PaperID;

            bool result = new DBHelperSQL().ExecuteNonQuery(strSql, parameters, MethodInfo.GetCurrentMethod().Name);
            return result;
        }
        /// <summary>
        /// 去除重复数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveDuplicate()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Authors");
            strSql.Append(" where  AuthorName in ");
            strSql.Append("(select AuthorName ");
            strSql.Append(" from T_Authors group ");
            strSql.Append("by AuthorName  having ");
            strSql.Append("count(AuthorName) > 1)");
            strSql.Append(" and  AuthorID not in (select min(AuthorID) ");
            strSql.Append(" from T_Authors group by AuthorName ");
            strSql.Append("having count(AuthorName)>1) ");
            bool result = new DBHelperSQL().ExecuteNonQuery(strSql, MethodInfo.GetCurrentMethod().Name);
            return result;
        }
    }

}
