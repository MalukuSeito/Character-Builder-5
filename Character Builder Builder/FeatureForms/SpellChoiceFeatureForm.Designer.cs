namespace Character_Builder_Builder.FeatureForms
{
    partial class SpellChoiceFeatureForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.NumericUpDown();
            this.label = new System.Windows.Forms.Label();
            this.AddTo = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.UniqueID = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.SpellcastingID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.keywordControl1 = new Character_Builder_Builder.KeywordControl();
            this.label14 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 518);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(772, 25);
            this.button1.TabIndex = 21;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer3.Location = new System.Drawing.Point(2, 282);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label4);
            this.splitContainer3.Panel1.Controls.Add(this.label1);
            this.splitContainer3.Panel1.Controls.Add(this.label8);
            this.splitContainer3.Panel1.Controls.Add(this.label7);
            this.splitContainer3.Panel1.Controls.Add(this.label6);
            this.splitContainer3.Panel1.Controls.Add(this.Condition);
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            this.splitContainer3.Panel1.Controls.Add(this.Amount);
            this.splitContainer3.Panel1.Controls.Add(this.label);
            this.splitContainer3.Panel1.Controls.Add(this.AddTo);
            this.splitContainer3.Panel1.Controls.Add(this.label11);
            this.splitContainer3.Panel1.Controls.Add(this.UniqueID);
            this.splitContainer3.Panel1.Controls.Add(this.label15);
            this.splitContainer3.Panel1.Controls.Add(this.SpellcastingID);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.keywordControl1);
            this.splitContainer3.Panel2.Controls.Add(this.label14);
            this.splitContainer3.Size = new System.Drawing.Size(772, 236);
            this.splitContainer3.SplitterDistance = 384;
            this.splitContainer3.TabIndex = 62;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 219);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(216, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = "ClassSpellLevel is (ClassLevel + 1) / 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 206);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(198, 13);
            this.label1.TabIndex = 93;
            this.label1.Text = "ClassLevel is the level of the class";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 193);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(234, 13);
            this.label8.TabIndex = 92;
            this.label8.Text = "As well as the spells keywords as boolean";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 180);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(267, 13);
            this.label7.TabIndex = 91;
            this.label7.Text = "Variables: Name and Level (number) of the spells";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(275, 13);
            this.label6.TabIndex = 90;
            this.label6.Text = "Note: Only spells matching the condition can be selected";
            // 
            // Condition
            // 
            this.Condition.Dock = System.Windows.Forms.DockStyle.Top;
            this.Condition.Location = new System.Drawing.Point(0, 147);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(384, 20);
            this.Condition.TabIndex = 89;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 13);
            this.label2.TabIndex = 88;
            this.label2.Text = "Condition for ClassList: (NCalc Expression)";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Top;
            this.Amount.Location = new System.Drawing.Point(0, 114);
            this.Amount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(384, 20);
            this.Amount.TabIndex = 87;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Dock = System.Windows.Forms.DockStyle.Top;
            this.label.Location = new System.Drawing.Point(0, 101);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(328, 13);
            this.label.TabIndex = 86;
            this.label.Text = "Amount of Spells: (can be modified by Increase Spellchoice Feature)";
            // 
            // AddTo
            // 
            this.AddTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.AddTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AddTo.FormattingEnabled = true;
            this.AddTo.Location = new System.Drawing.Point(0, 80);
            this.AddTo.Name = "AddTo";
            this.AddTo.Size = new System.Drawing.Size(384, 21);
            this.AddTo.TabIndex = 78;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(0, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 77;
            this.label11.Text = "Add Spells To:";
            // 
            // UniqueID
            // 
            this.UniqueID.Dock = System.Windows.Forms.DockStyle.Top;
            this.UniqueID.Location = new System.Drawing.Point(0, 47);
            this.UniqueID.Name = "UniqueID";
            this.UniqueID.Size = new System.Drawing.Size(384, 20);
            this.UniqueID.TabIndex = 76;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Top;
            this.label15.Location = new System.Drawing.Point(0, 34);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(227, 13);
            this.label15.TabIndex = 75;
            this.label15.Text = "Unique ID: (some other features reference this)";
            // 
            // SpellcastingID
            // 
            this.SpellcastingID.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpellcastingID.FormattingEnabled = true;
            this.SpellcastingID.Location = new System.Drawing.Point(0, 13);
            this.SpellcastingID.Name = "SpellcastingID";
            this.SpellcastingID.Size = new System.Drawing.Size(384, 21);
            this.SpellcastingID.TabIndex = 74;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(270, 13);
            this.label3.TabIndex = 73;
            this.label3.Text = "Spellcasting ID: (of the associated Spellcasting Feature)";
            // 
            // keywordControl1
            // 
            this.keywordControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.keywordControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywordControl1.Group = Character_Builder_Builder.KeywordControl.KeywordGroup.SPELL;
            this.keywordControl1.HistoryManager = null;
            this.keywordControl1.Keywords = null;
            this.keywordControl1.Location = new System.Drawing.Point(0, 13);
            this.keywordControl1.Name = "keywordControl1";
            this.keywordControl1.Size = new System.Drawing.Size(384, 223);
            this.keywordControl1.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoEllipsis = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Top;
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(384, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Keywords to add to the spell(s)";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(772, 280);
            this.basicFeature1.TabIndex = 63;
            // 
            // SpellChoiceFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(776, 545);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.button1);
            this.Name = "SpellChoiceFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Spellchoice Feature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpellChoiceFeatureForm_FormClosing);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox AddTo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox UniqueID;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox SpellcastingID;
        private System.Windows.Forms.Label label3;
        private KeywordControl keywordControl1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Amount;
        private System.Windows.Forms.Label label;
    }
}