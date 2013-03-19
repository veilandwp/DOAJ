using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAM.DAL;
using WinformAM.Model;

namespace WinformAM.BLL
{
    public class JournalBLL
    {
        JournalDAL journalDAL = new JournalDAL();

        public bool Add(Journal model)
        {
            return journalDAL.Add(model);
        }
    }
}
