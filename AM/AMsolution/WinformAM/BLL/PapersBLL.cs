using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.DAL;
using WinformAM.Model;

namespace WinformAM.BLL
{
    public class PapersBLL
    {
        PapersDAL papersDAL = new PapersDAL();
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(PaperInfo model)
        {
            return papersDAL.Add(model);
        }
          /*************************************************************************/
        /*描    述：根据文章Title从数据表T_Papers中读取数据库自动生成的PaperID
         *输入参数：【PaperInfo p】
         *返 回 值：【int: 不存在文章信息返回0，否则返回文章ID号】
         *开发日期：2011.8.21
         */
        public int GetPaperIDByTitle(PaperInfo p)
        {
            return papersDAL.GetPaperIDByTitle(p);
        }
          /*************************************************************************/
        /*描    述：根据文章URL和Title从数据表T_Papers中读取数据库自动生成的PaperID
         *输入参数：【PaperInfo p】
         *返 回 值：【int: 不存在文章信息返回0，否则返回文章ID号】
         *开发日期：2011.8.21
         */
        public int GetPaperIDByUrlTitle(PaperInfo p)
        {
            return papersDAL.GetPaperIDByUrlTitle(p);
        }
        /// <summary>
        /// 去除重复数据
        /// </summary>
        /// <returns></returns>
        public bool RemoveDuplicate()
        {
            return papersDAL.RemoveDuplicate();
        }
    }
}
