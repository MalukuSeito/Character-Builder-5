namespace Character_Builder_Builder
{
    partial class PackForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.userControl11 = new Character_Builder_Builder.UserControl1();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicItem1 = new Character_Builder_Builder.ItemForms.BasicItem();
            this.stringList1 = new Character_Builder_Builder.StringList();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.abort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 653);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1025, 30);
            this.panel1.TabIndex = 0;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(863, 4);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 1;
            this.save.Text = "Save && Exit";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // abort
            // 
            this.abort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abort.Location = new System.Drawing.Point(944, 4);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(78, 23);
            this.abort.TabIndex = 0;
            this.abort.Text = "Exit";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // preview
            // 
            this.preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.preview.Location = new System.Drawing.Point(776, 24);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(250, 629);
            this.preview.TabIndex = 10;
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl11.Editor = null;
            this.userControl11.Location = new System.Drawing.Point(1, 0);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1025, 24);
            this.userControl11.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(1, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.basicItem1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.stringList1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(775, 629);
            this.splitContainer1.SplitterDistance = 346;
            this.splitContainer1.TabIndex = 11;
            // 
            // basicItem1
            // 
            this.basicItem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicItem1.HistoryManager = null;
            this.basicItem1.Item = null;
            this.basicItem1.Location = new System.Drawing.Point(0, 0);
            this.basicItem1.Name = "basicItem1";
            this.basicItem1.Size = new System.Drawing.Size(775, 346);
            this.basicItem1.TabIndex = 12;
            // 
            // stringList1
            // 
            this.stringList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringList1.HistoryManager = null;
            this.stringList1.Items = null;
            this.stringList1.Location = new System.Drawing.Point(0, 13);
            this.stringList1.Name = "stringList1";
            this.stringList1.Size = new System.Drawing.Size(775, 266);
            this.stringList1.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("stringList1.Suggestions")));
            this.stringList1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contents:";
            // 
            // PackForm
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.abort;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1027, 683);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel1);
            this.Name = "PackForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Pack";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RaceForm_FormClosing);
            this.Shown += new System.EventHandler(this.RaceForm_Shown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button abort;
        private UserControl1 userControl11;
        private System.Windows.Forms.WebBrowser preview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ItemForms.BasicItem basicItem1;
        private StringList stringList1;
        private System.Windows.Forms.Label label1;
    }
}