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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            Description = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            Prereq = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            Action = new System.Windows.Forms.ComboBox();
            NoPreview = new System.Windows.Forms.CheckBox();
            panel1 = new System.Windows.Forms.Panel();
            Level = new System.Windows.Forms.NumericUpDown();
            Hidden = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            NameTF = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            keywordControl1 = new KeywordControl();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Level).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Description);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(Prereq);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(NameTF);
            splitContainer1.Panel1.Controls.Add(label2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(keywordControl1);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new System.Drawing.Size(572, 328);
            splitContainer1.SplitterDistance = 312;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // Description
            // 
            Description.Dock = System.Windows.Forms.DockStyle.Fill;
            Description.Location = new System.Drawing.Point(0, 156);
            Description.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Description.Multiline = true;
            Description.Name = "Description";
            Description.Size = new System.Drawing.Size(312, 172);
            Description.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 141);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(70, 15);
            label5.TabIndex = 13;
            label5.Text = "Description:";
            // 
            // Prereq
            // 
            Prereq.Dock = System.Windows.Forms.DockStyle.Top;
            Prereq.Location = new System.Drawing.Point(0, 118);
            Prereq.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Prereq.Name = "Prereq";
            Prereq.Size = new System.Drawing.Size(312, 23);
            Prereq.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Location = new System.Drawing.Point(0, 103);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(189, 15);
            label4.TabIndex = 11;
            label4.Text = "Prerequesite: (standalone features)";
            // 
            // panel2
            // 
            panel2.Controls.Add(Action);
            panel2.Controls.Add(NoPreview);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 78);
            panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(312, 25);
            panel2.TabIndex = 8;
            // 
            // Action
            // 
            Action.Dock = System.Windows.Forms.DockStyle.Fill;
            Action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Action.FormattingEnabled = true;
            Action.Location = new System.Drawing.Point(0, 0);
            Action.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Action.Name = "Action";
            Action.Size = new System.Drawing.Size(141, 23);
            Action.TabIndex = 1;
            // 
            // NoPreview
            // 
            NoPreview.AutoSize = true;
            NoPreview.Dock = System.Windows.Forms.DockStyle.Right;
            NoPreview.Location = new System.Drawing.Point(141, 0);
            NoPreview.Margin = new System.Windows.Forms.Padding(23, 3, 4, 3);
            NoPreview.Name = "NoPreview";
            NoPreview.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            NoPreview.Size = new System.Drawing.Size(171, 25);
            NoPreview.TabIndex = 0;
            NoPreview.Text = "Hide feature in preview";
            NoPreview.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(Level);
            panel1.Controls.Add(Hidden);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 53);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(312, 25);
            panel1.TabIndex = 3;
            // 
            // Level
            // 
            Level.Dock = System.Windows.Forms.DockStyle.Fill;
            Level.Location = new System.Drawing.Point(0, 0);
            Level.Margin = new System.Windows.Forms.Padding(4, 3, 23, 3);
            Level.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            Level.Name = "Level";
            Level.Size = new System.Drawing.Size(131, 23);
            Level.TabIndex = 1;
            // 
            // Hidden
            // 
            Hidden.Dock = System.Windows.Forms.DockStyle.Right;
            Hidden.Location = new System.Drawing.Point(131, 0);
            Hidden.Margin = new System.Windows.Forms.Padding(23, 3, 4, 3);
            Hidden.Name = "Hidden";
            Hidden.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            Hidden.Size = new System.Drawing.Size(181, 25);
            Hidden.TabIndex = 0;
            Hidden.Text = "Hide feature on sheet";
            Hidden.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(0, 38);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(37, 15);
            label3.TabIndex = 2;
            label3.Text = "Level:";
            // 
            // NameTF
            // 
            NameTF.Dock = System.Windows.Forms.DockStyle.Top;
            NameTF.Location = new System.Drawing.Point(0, 15);
            NameTF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NameTF.Name = "NameTF";
            NameTF.Size = new System.Drawing.Size(312, 23);
            NameTF.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(42, 15);
            label2.TabIndex = 0;
            label2.Text = "Name:";
            // 
            // keywordControl1
            // 
            keywordControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            keywordControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            keywordControl1.Group = KeywordControl.KeywordGroup.FEAT;
            keywordControl1.HistoryManager = null;
            keywordControl1.Keywords = null;
            keywordControl1.Location = new System.Drawing.Point(0, 15);
            keywordControl1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            keywordControl1.Name = "keywordControl1";
            keywordControl1.Size = new System.Drawing.Size(255, 313);
            keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(175, 15);
            label1.TabIndex = 0;
            label1.Text = "Keywords: (standalone features)";
            // 
            // BasicFeature
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BasicFeature";
            Size = new System.Drawing.Size(572, 328);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Level).EndInit();
            ResumeLayout(false);
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
