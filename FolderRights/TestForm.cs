using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FolderRights
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string executionFolder = GetExecutionFolder();
            //executionFolder += "\\File.txt";

            string commonAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Scan";

            if (!Directory.Exists(commonAppDataFolder))
                Directory.CreateDirectory(commonAppDataFolder);

            commonAppDataFolder += "\\File.txt";

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Scan";

            if (!Directory.Exists(appDataFolder))
                Directory.CreateDirectory(appDataFolder);

            appDataFolder += "\\File.txt";


            //File.AppendAllText(executionFolder, "Appended Data");
            File.AppendAllText(commonAppDataFolder, "Appended Data");
            File.AppendAllText(appDataFolder, "Appended Data");
        }

        public string GetExecutionFolder()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}
