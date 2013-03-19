using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.DAL;
using WinformAM.Model;

namespace WinformAM.BLL
{
    public class CategoryWebsiteBLL
    {
        CategoryWebsiteDAL categoryWebsiteDAL = new CategoryWebsiteDAL();

        public IList<CategoryWebsite> GetAllCategoryWebsites()
        {
            return categoryWebsiteDAL.GetAllCategoryWebsites();
        }
    }
}
