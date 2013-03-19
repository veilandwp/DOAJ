using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.DAL;
using WinformAM.Model;

namespace WinformAM.BLL
{
    public class AuthorsBLL
    {
        AuthorsDAL authorsDAL = new AuthorsDAL();
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(AuthorInfo model)
        {

            return authorsDAL.Add(model);
        }
             /// <summary>
        /// 去除重复数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveDuplicate()
        {
            return authorsDAL.RemoveDuplicate();
        }
    }
}
