using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WinformAM.Tool
{
    public class LogManager
    {

        private string filePath;
        private string fileName;
        public LogManager()
        {
            filePath = System.Configuration.ConfigurationSettings.AppSettings[SysSetingManager.ENV_SYSTEM_LOG_DIRECTORY_NAME];
            fileName = System.Configuration.ConfigurationSettings.AppSettings[SysSetingManager.ENV_SYSTEM_LOG_NAME];

        }
        public void WriteErrLog(string formID, string actionName, string errMsg)
        {
            StreamWriter vStmOutput = null;
            if (actionName.Length > 20)
            {

            }
            else
            {
                actionName = actionName.PadRight(20);
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            try
            {
                string changeFilePath = System.IO.Path.Combine(filePath, fileName);
                vStmOutput = new StreamWriter(changeFilePath, true, System.Text.Encoding.Default);
                string vOutData = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "     " + formID + "     " + actionName + "     " + errMsg;
                vStmOutput.WriteLine("-----------------------------------------------------------------------------------------------");
                vStmOutput.WriteLine(vOutData);

            }
            catch
            {

            }
            finally
            {
                if (vStmOutput != null)
                {
                    vStmOutput.Close();
                }
            }

        }
    }
}
