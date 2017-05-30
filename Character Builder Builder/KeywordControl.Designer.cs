namespace Character_Builder_Builder
{
    partial class KeywordControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.newKeyword = new System.Windows.Forms.ComboBox();
            this.AddBtn = new System.Windows.Forms.Button();
            this.keywords = new System.Windows.Forms.CheckedListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.newKeyword);
            this.panel1.Controls.Add(this.AddBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 330);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 23);
            this.panel1.TabIndex = 0;
            // 
            // newKeyword
            // 
            this.newKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.newKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.newKeyword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newKeyword.FormattingEnabled = true;
            this.newKeyword.Location = new System.Drawing.Point(0, 0);
            this.newKeyword.Name = "newKeyword";
            this.newKeyword.Size = new System.Drawing.Size(318, 21);
            this.newKeyword.TabIndex = 1;
            this.newKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newKeyword_KeyDown);
            // 
            // AddBtn
            // 
            this.AddBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.AddBtn.Location = new System.Drawing.Point(318, 0);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(39, 23);
            this.AddBtn.TabIndex = 0;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.Add_Click);
            // 
            // keywords
            // 
            this.keywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywords.FormattingEnabled = true;
            this.keywords.Location = new System.Drawing.Point(0, 0);
            this.keywords.Name = "keywords";
            this.keywords.Size = new System.Drawing.Size(357, 330);
            this.keywords.TabIndex = 1;
            this.keywords.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.keywords_ItemCheck);
            // 
            // KeywordControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.keywords);
            this.Controls.Add(this.panel1);
            this.Name = "KeywordControl";
            this.Size = new System.Drawing.Size(357, 353);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.CheckedListBox keywords;
        private System.Windows.Forms.ComboBox newKeyword;
    }
}
