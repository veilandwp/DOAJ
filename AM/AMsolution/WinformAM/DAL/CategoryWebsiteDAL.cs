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
    public class CategoryWebsiteDAL
    {
        private const string SQL_SELECT_CategoryWebsite = "select * from T_CategoryWebsite";

        public IList<CategoryWebsite> GetAllCategoryWebsites()
        {
            IList<CategoryWebsite> categoryWebsites = new List<CategoryWebsite>();
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(SQL_SELECT_CategoryWebsite);
                dt =new DBHelperSQL().ExecuteQuery(sb, MethodInfo.GetCurrentMethod().Name);
                foreach (DataRow dr in dt.Rows)
                {
                    CategoryWebsite cw = new CategoryWebsite(Convert.ToInt32(dr["CategoryWebsiteID"].ToString()), dr["CategoryLevelOne"].ToString(), dr["CategoryLevelTwo"].ToString(), dr["CategoryLevelThree"].ToString(), dr["Website"].ToString(), Convert.ToInt32(dr["WebsiteCount"].ToString()));
                    categoryWebsites.Add(cw);
                }
                return categoryWebsites;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
