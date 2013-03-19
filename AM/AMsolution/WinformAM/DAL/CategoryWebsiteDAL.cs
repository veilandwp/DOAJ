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
                dt = DBHelperSQL().ExecuteNonQuery(SQL_SELECT_CategoryWebsite, MethodInfo.GetCurrentMethod().Name);
                foreach (DataRow dr in dt.Rows)
                {
                    CategoryWebsite cw = new CategoryWebsite(dr["CategoryWebsiteID"], dr["CategoryLevelOne"], dr["CategoryLevelTwo"], dr["CategoryLevelThree"], dr["Website"], dr["WebsiteCount"]);
                    categoryWebsites.Add(cw);
                }
                return categoryWebsites;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }        
        }
    }
}
