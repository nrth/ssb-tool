using System;
using System.Windows.Forms;

namespace ssb_tool
{
    public partial class App : Form
    {
        private ServerBrowserHistory _hisManager = new ServerBrowserHistory();
        private AccountDiscovery _accDiscovery = new AccountDiscovery();

        public App()
        {
            InitializeComponent();

            fetchAccounts();

            account.DataSource = new BindingSource(
                                        _accDiscovery.getAccounts(),
                                        null);

            account.DisplayMember = "Value";
            account.ValueMember = "Key";
        }

        private void fetchAccounts()
        {
            try
            {
                _accDiscovery.fetchAccounts();
            }
            catch
            {
                MessageBox.Show("Can not access Steam Userdata directories.",
                                "Error");
                Environment.Exit(-1);
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            String accountid = getSelectedAccountId();

            OpenFileDialog importDialog = new OpenFileDialog();
            importDialog.Filter = "JSON|*.json|TXT|*.txt";
            importDialog.Title = "IMPORT";

            if (importDialog.ShowDialog() == DialogResult.OK 
                && importDialog.FileName != "")
            {
                try
                {
                    _hisManager.Import(accountid, importDialog.FileName);

                    MessageBox.Show("Please restart Steam for the"
                                  + " import to take effect.",
                                    "STEAM RESTART REQUIRED",
                                    MessageBoxButtons.OK);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    Application.Exit();
                }
            }
        }

        private void backup_Click(object sender, EventArgs e)
        {

            String accountid = getSelectedAccountId();

            SaveFileDialog backupDialog = new SaveFileDialog();
            backupDialog.Filter = "JSON|*.json";
            backupDialog.Title = "BACKUP";
            backupDialog.FileName = "serverlist.json";

            if (backupDialog.ShowDialog() == DialogResult.OK 
                && backupDialog.FileName != "")
            {
                try
                {
                    _hisManager.Backup(accountid, backupDialog.FileName);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    Application.Exit();
                }
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
                                     "You are about to reset your server list and history"
                                   + " for the account {0} ({1}), ALL"
                                   + " information will be lost. \n\nAre you"
                                   + " sure you want to continue?", 
                                   account.Text,
                                   accountid);

            DialogResult confirm = MessageBox.Show(msg,
                                                   "Confirmation", 
                                                   MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _hisManager.Purge(accountid);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    Application.Exit();
                }
            }
        }

        private String getSelectedAccountId()
        {
            return account.SelectedValue.ToString();
        }
    }
}
