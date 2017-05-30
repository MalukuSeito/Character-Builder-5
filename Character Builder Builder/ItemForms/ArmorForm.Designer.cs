namespace Character_Builder_Builder
{
    partial class ArmorForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.userControl11 = new Character_Builder_Builder.UserControl1();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DisStealth = new System.Windows.Forms.CheckBox();
            this.StrengthRequired = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.BaseAC = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.basicItem1 = new Character_Builder_Builder.ItemForms.BasicItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StrengthRequired)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseAC)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.abort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 571);
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
            this.preview.CausesValidation = false;
            this.preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.preview.Location = new System.Drawing.Point(776, 24);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(250, 547);
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
            // panel2
            // 
            this.panel2.Controls.Add(this.StrengthRequired);
            this.panel2.Controls.Add(this.DisStealth);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(1, 551);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 20);
            this.panel2.TabIndex = 20;
            // 
            // DisStealth
            // 
            this.DisStealth.AutoSize = true;
            this.DisStealth.Dock = System.Windows.Forms.DockStyle.Right;
            this.DisStealth.Location = new System.Drawing.Point(583, 0);
            this.DisStealth.Name = "DisStealth";
            this.DisStealth.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.DisStealth.Size = new System.Drawing.Size(192, 20);
            this.DisStealth.TabIndex = 17;
            this.DisStealth.Text = "Disadvantage on Stealth Checks";
            this.DisStealth.UseVisualStyleBackColor = true;
            this.DisStealth.CheckedChanged += new System.EventHandler(this.DisStealth_CheckedChanged);
            this.DisStealth.Enter += new System.EventHandler(this.showPreview);
            // 
            // StrengthRequired
            // 
            this.StrengthRequired.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StrengthRequired.Location = new System.Drawing.Point(0, 0);
            this.StrengthRequired.Name = "StrengthRequired";
            this.StrengthRequired.Size = new System.Drawing.Size(583, 20);
            this.StrengthRequired.TabIndex = 18;
            this.StrengthRequired.ValueChanged += new System.EventHandler(this.StrengthRequired_ValueChanged);
            this.StrengthRequired.Enter += new System.EventHandler(this.showPreview);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(1, 505);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Base AC:";
            // 
            // BaseAC
            // 
            this.BaseAC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BaseAC.Location = new System.Drawing.Point(1, 518);
            this.BaseAC.Name = "BaseAC";
            this.BaseAC.Size = new System.Drawing.Size(775, 20);
            this.BaseAC.TabIndex = 23;
            this.BaseAC.ValueChanged += new System.EventHandler(this.BaseAC_ValueChanged);
            this.BaseAC.Enter += new System.EventHandler(this.showPreview);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(1, 538);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Strength Required:";
            // 
            // basicItem1
            // 
            this.basicItem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicItem1.HistoryManager = null;
            this.basicItem1.Item = null;
            this.basicItem1.Location = new System.Drawing.Point(1, 24);
            this.basicItem1.Name = "basicItem1";
            this.basicItem1.Size = new System.Drawing.Size(775, 481);
            this.basicItem1.TabIndex = 25;
            // 
            // ArmorForm
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.abort;
            this.ClientSize = new System.Drawing.Size(1027, 601);
            this.Controls.Add(this.basicItem1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseAC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel1);
            this.Name = "ArmorForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Armor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RaceForm_FormClosing);
            this.Shown += new System.EventHandler(this.RaceForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StrengthRequired)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseAC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button abort;
        private UserControl1 userControl11;
        private System.Windows.Forms.WebBrowser preview;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown StrengthRequired;
        private System.Windows.Forms.CheckBox DisStealth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BaseAC;
        private System.Windows.Forms.Label label1;
        private ItemForms.BasicItem basicItem1;
    }
}