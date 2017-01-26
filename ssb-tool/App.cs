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
        private String historySubPath = @"\7\remote\serverbrowser_hist.vdf";
        private String localconfigPath = @"\config\localconfig.vdf";

        private Dictionary<String, String> accounts_ = new Dictionary<String, String>();

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

        private void touchFile(String path)
        {
            File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
        }

        private void import_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON|*.json|TXT|*.txt";
            openFileDialog.Title = "Import Server List Backup";

            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                String accountid = getIDForAccount(account.Text);
                String historyPath = userdataPath + accountid + historySubPath;

                String import = File.ReadAllText(openFileDialog.FileName);
                String[] current = File.ReadAllLines(historyPath);

                // handle situation with faulty json
                dynamic imp = JObject.Parse(import);
                // TODO handle situation where file is empty
                dynamic cur = VDFConvert.ToJObject(current);

                cur.Filters.favorites = imp;

                StreamWriter output = new StreamWriter(historyPath);

                Vdf.Convert(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(cur.ToString()))), output);

                output.Close();
                output.Dispose(); // make sure there are no references to this file left

                touchFile(historyPath);
            }
        }

        private void backup_Click(object sender, EventArgs e)
        {
            String accountid = getIDForAccount(account.Text);

            String path = userdataPath + accountid + historySubPath;

            dynamic hist = VDFConvert.ToJObject(File.ReadAllLines(path));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON|*.json|TXT|*.txt";
            saveFileDialog.Title = "Save Server List Backup";
            saveFileDialog.FileName = "serverlist.json";
            saveFileDialog.ShowDialog();

            if(saveFileDialog.FileName != "")
            {
                StreamWriter file = new StreamWriter(saveFileDialog.FileName);

                file.WriteLine(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        hist.Filters.favorites,
                        Formatting.Indented
                ));

                file.Close();
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

            String path = userdataPath + accountid + historySubPath;

            // TODO ask if you are sure
            File.WriteAllText(path, string.Empty);
            touchFile(path);
        }
    }
}
