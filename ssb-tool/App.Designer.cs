namespace ssb_tool
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backup = new System.Windows.Forms.Button();
            this.import = new System.Windows.Forms.Button();
            this.account = new System.Windows.Forms.ComboBox();
            this.refresh = new System.Windows.Forms.Button();
            this.purge = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // backup
            // 
            this.backup.Location = new System.Drawing.Point(12, 39);
            this.backup.Name = "backup";
            this.backup.Size = new System.Drawing.Size(301, 53);
            this.backup.TabIndex = 0;
            this.backup.Text = "Backup";
            this.backup.UseVisualStyleBackColor = true;
            this.backup.Click += new System.EventHandler(this.backup_Click);
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(12, 98);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(301, 53);
            this.import.TabIndex = 1;
            this.import.Text = "Import";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // account
            // 
            this.account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.account.FormattingEnabled = true;
            this.account.Location = new System.Drawing.Point(12, 12);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(237, 21);
            this.account.TabIndex = 2;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(255, 11);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(58, 23);
            this.refresh.TabIndex = 3;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // purge
            // 
            this.purge.Location = new System.Drawing.Point(12, 157);
            this.purge.Name = "purge";
            this.purge.Size = new System.Drawing.Size(113, 32);
            this.purge.TabIndex = 4;
            this.purge.Text = "Purge";
            this.purge.UseVisualStyleBackColor = true;
            this.purge.Click += new System.EventHandler(this.purge_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 198);
            this.Controls.Add(this.purge);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.account);
            this.Controls.Add(this.import);
            this.Controls.Add(this.backup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Steam Server Browser Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button backup;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.ComboBox account;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.Button purge;
    }
}

