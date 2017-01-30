using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using SourceSchemaParser.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ssb_tool
{
    // TODO
    // Refactoring: - separate business logic for serverdata and user handling
    //              - error handling
    //              - implement confirmation dialogs
    //              - improve json2vdf converter
    //              - dependency cleanup
    //              - solution for unusual (other drive etc.) steam path
    //              - improve performance (stream converting etc.)

    public partial class App : Form
    {
        private String userdataPath = @"C:\Program Files (x86)\Steam\userdata\";
        private String localconfigPath = @"\config\localconfig.vdf";

        private Dictionary<String, String> accounts_ = new Dictionary<String, String>();

        private ServerBrowserHistory _historyManager = new ServerBrowserHistory();

        public App()
        {
            InitializeComponent();

            fetchAccounts();

            account.DataSource = accounts_.Keys.ToList();
        }

        private void fetchAccounts()
        {
            String[] accountDirectories = Directory.GetFileSystemEntries(userdataPath);

            foreach (var item in accountDirectories)
            {
                String[] localconfigF = File.ReadAllLines(item + localconfigPath);
                dynamic localconfig = VDFConvert.ToJObject(localconfigF);

                accounts_.Add(
                    localconfig.UserLocalConfigStore.friends.PersonaName.ToString(),
                    item.Split('\\').Last()
                );
            }
        }

        private String getIDForAccount(String accountPersona)
        {
            String accountid;
            accounts_.TryGetValue(accountPersona, out accountid);
            return accountid;
        }

        private void import_Click(object sender, EventArgs e)
        {
            String accountid = getIDForAccount(account.Text);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON|*.json|TXT|*.txt";
            openFileDialog.Title = "Import Server List Backup";

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                _historyManager.Import(accountid, openFileDialog.FileName);
            }
        }

        private void backup_Click(object sender, EventArgs e)
        {

            String accountid = getIDForAccount(account.Text);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON|*.json|TXT|*.txt";
            saveFileDialog.Title = "Save Server List Backup";
            saveFileDialog.FileName = "serverlist.json";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                _historyManager.Backup(accountid, saveFileDialog.FileName);
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            accounts_.Clear();
            fetchAccounts();
        }

        private void purge_Click(object sender, EventArgs e)
        {
            String accountid = getIDForAccount(account.Text);

            String msg = string.Format(
                                     "You are about to empty your server list"
                                   + "for the account {0} ({1}), all "
                                   + "information will be lost. It is "
                                   + "recommended to make a backup of your "
                                   + "current server list first. \n\nAre you "
                                   + "sure you want to continue?", 
                                   account.Text,
                                   accountid);

            DialogResult confirm = MessageBox.Show(msg,
                                                   "Confirmation", 
                                                   MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _historyManager.Purge(accountid);
            }
        }
    }
}
