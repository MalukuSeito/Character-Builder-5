namespace Character_Builder_Builder.FeatureForms
{
    partial class BasicFeature
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Description = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Prereq = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Action = new System.Windows.Forms.ComboBox();
            this.NoPreview = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Level = new System.Windows.Forms.NumericUpDown();
            this.Hidden = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NameTF = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.keywordControl1 = new Character_Builder_Builder.KeywordControl();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Level)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Description);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.Prereq);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.NameTF);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.keywordControl1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(490, 284);
            this.splitContainer1.SplitterDistance = 268;
            this.splitContainer1.TabIndex = 0;
            // 
            // Description
            // 
            this.Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Description.Location = new System.Drawing.Point(0, 136);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(268, 148);
            this.Description.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Description:";
            // 
            // Prereq
            // 
            this.Prereq.Dock = System.Windows.Forms.DockStyle.Top;
            this.Prereq.Location = new System.Drawing.Point(0, 103);
            this.Prereq.Name = "Prereq";
            this.Prereq.Size = new System.Drawing.Size(268, 20);
            this.Prereq.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Prerequesite: (standalone features)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Action);
            this.panel2.Controls.Add(this.NoPreview);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(268, 22);
            this.panel2.TabIndex = 8;
            // 
            // Action
            // 
            this.Action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Action.FormattingEnabled = true;
            this.Action.Location = new System.Drawing.Point(0, 0);
            this.Action.Name = "Action";
            this.Action.Size = new System.Drawing.Size(113, 21);
            this.Action.TabIndex = 1;
            // 
            // NoPreview
            // 
            this.NoPreview.AutoSize = true;
            this.NoPreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.NoPreview.Location = new System.Drawing.Point(113, 0);
            this.NoPreview.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.NoPreview.Name = "NoPreview";
            this.NoPreview.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.NoPreview.Size = new System.Drawing.Size(155, 22);
            this.NoPreview.TabIndex = 0;
            this.NoPreview.Text = "Hide feature in preview";
            this.NoPreview.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Level);
            this.panel1.Controls.Add(this.Hidden);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 22);
            this.panel1.TabIndex = 3;
            // 
            // Level
            // 
            this.Level.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Level.Location = new System.Drawing.Point(0, 0);
            this.Level.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.Level.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(113, 20);
            this.Level.TabIndex = 1;
            // 
            // Hidden
            // 
            this.Hidden.Dock = System.Windows.Forms.DockStyle.Right;
            this.Hidden.Location = new System.Drawing.Point(113, 0);
            this.Hidden.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.Hidden.Name = "Hidden";
            this.Hidden.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.Hidden.Size = new System.Drawing.Size(155, 22);
            this.Hidden.TabIndex = 0;
            this.Hidden.Text = "Hide feature on sheet";
            this.Hidden.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Level:";
            // 
            // NameTF
            // 
            this.NameTF.Dock = System.Windows.Forms.DockStyle.Top;
            this.NameTF.Location = new System.Drawing.Point(0, 13);
            this.NameTF.Name = "NameTF";
            this.NameTF.Size = new System.Drawing.Size(268, 20);
            this.NameTF.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // keywordControl1
            // 
            this.keywordControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.keywordControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywordControl1.Group = Character_Builder_Builder.KeywordControl.KeywordGroup.FEAT;
            this.keywordControl1.HistoryManager = null;
            this.keywordControl1.Keywords = null;
            this.keywordControl1.Location = new System.Drawing.Point(0, 13);
            this.keywordControl1.Name = "keywordControl1";
            this.keywordControl1.Size = new System.Drawing.Size(218, 271);
            this.keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keywords: (standalone features)";
            // 
            // BasicFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BasicFeature";
            this.Size = new System.Drawing.Size(490, 284);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Level)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown Level;
        private System.Windows.Forms.CheckBox Hidden;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NameTF;
        private System.Windows.Forms.Label label2;
        private KeywordControl keywordControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Description;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Prereq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox Action;
        private System.Windows.Forms.CheckBox NoPreview;
    }
}
