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
    public partial class PapersDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(PaperInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_Papers(");
            strSql.Append("PaperDoi,PaperTitle,PaperAbstract,PaperUrl,PaperYear,JournalID)");
            strSql.Append(" values (");
            strSql.Append("@PaperDoi,@PaperTitle,@PaperAbstract,@PaperUrl,@PaperYear,@JournalID)");
            SqlParameter[] parameters = {
					new SqlParameter("@PaperDoi", SqlDbType.NVarChar,64),
					new SqlParameter("@PaperTitle", SqlDbType.NVarChar,4000),
					new SqlParameter("@PaperAbstract", SqlDbType.NVarChar,4000),
					new SqlParameter("@PaperUrl", SqlDbType.NVarChar,1024),
					new SqlParameter("@PaperYear", SqlDbType.NVarChar,64),
					new SqlParameter("@JournalID", SqlDbType.Int,4)};
            parameters[0].Value = model.PaperDoi;
            parameters[1].Value = model.PaperTitle;
            parameters[2].Value = model.PaperAbstract;
            parameters[3].Value = model.PaperUrl;
            parameters[4].Value = model.PaperYear;
            parameters[5].Value = model.JournalID;

            bool result = new DBHelperSQL().ExecuteNonQuery(strSql, parameters, MethodInfo.GetCurrentMethod().Name);
            return result;
        }
        /*************************************************************************/
        /*描    述：根据文章Title从数据表T_Papers中读取数据库自动生成的PaperID
         *输入参数：【PaperInfo p】
         *返 回 值：【int: 不存在文章信息返回0，否则返回文章ID号】
         *开发日期：2011.8.21
         */
        public int GetPaperIDByTitle(PaperInfo p)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select PaperID from T_Papers where PaperTitle=@PaperTitle");
            SqlParameter[] pramsForID = 
            {
                new SqlParameter("@PaperTitle",SqlDbType.VarChar,4000)
            };
            pramsForID[0].Value=p.PaperTitle;

            DataTable dt = new DataTable();
            dt = new DBHelperSQL().ExecuteQuery(strSql, pramsForID, MethodInfo.GetCurrentMethod().Name);

            if (dt.Rows.Count > 1)
                p.PaperID = GetPaperIDByUrlTitle(p);
            else if (dt.Rows.Count == 1)
                p.PaperID = Convert.ToInt32(dt.Rows[0]["PaperID"].ToString());
            else
                p.PaperID = 0;

            return p.PaperID;
        }


        /*************************************************************************/
        /*描    述：根据文章URL和Title从数据表T_Papers中读取数据库自动生成的PaperID
         *输入参数：【PaperInfo p】
         *返 回 值：【int: 不存在文章信息返回0，否则返回文章ID号】
         *开发日期：2011.8.21
         */
        public int GetPaperIDByUrlTitle(PaperInfo p)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select PaperID from T_Papers where PaperUrl=@PaperUrl and PaperTitle=@PaperTitle");
            SqlParameter[] pramsForID = 
            {
                new SqlParameter("@PaperUrl",SqlDbType.VarChar,1024),
                new SqlParameter("@PaperTitle",SqlDbType.VarChar,4000)
            };
            pramsForID[0].Value = p.PaperUrl;
            pramsForID[1].Value = p.PaperTitle;
            DataTable dt = new DataTable();
            try
            {
                dt = new DBHelperSQL().ExecuteQuery(strSql, pramsForID, MethodInfo.GetCurrentMethod().Name);

            }
            catch (System.Exception ex)
            {
            	
            }
            

            if (dt.Rows.Count > 0)
                p.PaperID = Convert.ToInt32(dt.Rows[0]["PaperID"].ToString());
            else
                p.PaperID = 0;

            return p.PaperID;
        }
        /// <summary>
        /// 去除重复数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveDuplicate()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Papers");
            strSql.Append(" where  PaperTitle in ");
            strSql.Append("(select PaperTitle ");
            strSql.Append(" from T_Papers group ");
            strSql.Append("by PaperTitle  having ");
            strSql.Append("count(PaperTitle) > 1)");
            strSql.Append(" and  PaperID not in (select min(PaperID) ");
            strSql.Append(" from T_Papers group by PaperTitle ");
            strSql.Append("having count(PaperTitle)>1) ");
            bool result = new DBHelperSQL().ExecuteNonQuery(strSql, MethodInfo.GetCurrentMethod().Name);
            return result;
        }
    }
}
