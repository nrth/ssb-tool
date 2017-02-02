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
    //              - better error handling
    //              - dependency cleanup
    //              - solution for unusual (other drive etc.) steam path (path discovery?)

    public partial class App : Form
    {
        private ServerBrowserHistory _historyManager = new ServerBrowserHistory();
        private AccountDiscovery _accountDiscovery = new AccountDiscovery();

        public App()
        {
            InitializeComponent();

            fetchAccounts();

            account.DataSource = new BindingSource(_accountDiscovery.getAccounts(), null);
            account.DisplayMember = "Value";
            account.ValueMember = "Key";
        }

        private void fetchAccounts()
        {
            try
            {
                _accountDiscovery.fetchAccounts();
            }
            catch
            {
                MessageBox.Show("Could not access steam userdata directories.",
                                "Error");
                Environment.Exit(-1);
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            String accountid = getSelectedAccountId();

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

            String accountid = getSelectedAccountId();

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
            fetchAccounts();
            account.ResetBindings();
        }

        private void purge_Click(object sender, EventArgs e)
        {
            String accountid = getSelectedAccountId();

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

        private String getSelectedAccountId()
        {
            return account.SelectedValue.ToString();
        }
    }
}
