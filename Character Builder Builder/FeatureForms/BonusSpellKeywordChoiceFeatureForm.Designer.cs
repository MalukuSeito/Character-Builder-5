namespace Character_Builder_Builder.FeatureForms
{
    partial class BonusSpellKeywordChoiceFeatureForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.label2 = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SpellcastingModifier = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.UniqueID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Recharge = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.keywordControl1 = new Character_Builder_Builder.KeywordControl();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.basicFeature1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.Condition);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(723, 656);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.TabIndex = 20;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(0, 0);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(723, 212);
            this.basicFeature1.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Affected Spells: (NCalc Expression)";
            // 
            // Condition
            // 
            this.Condition.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Condition.Location = new System.Drawing.Point(0, 225);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(723, 20);
            this.Condition.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(723, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Note: Only spells matching the condition can be selected.";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(0, 258);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(723, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Variables: Name (spell: string, lowercase), Level (spell: number). the spell keyw" +
    "ords as boolean (see right for examples)";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.SpellcastingModifier);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.UniqueID);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.Recharge);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.Amount);
            this.splitContainer2.Panel1.Controls.Add(this.label9);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.keywordControl1);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(723, 356);
            this.splitContainer2.SplitterDistance = 357;
            this.splitContainer2.TabIndex = 21;
            // 
            // SpellcastingModifier
            // 
            this.SpellcastingModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpellcastingModifier.FormattingEnabled = true;
            this.SpellcastingModifier.Location = new System.Drawing.Point(0, 126);
            this.SpellcastingModifier.Name = "SpellcastingModifier";
            this.SpellcastingModifier.Size = new System.Drawing.Size(357, 230);
            this.SpellcastingModifier.TabIndex = 51;
            this.SpellcastingModifier.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SpellcastingModifier_ItemCheck);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Spellcasting Ability:";
            // 
            // UniqueID
            // 
            this.UniqueID.Dock = System.Windows.Forms.DockStyle.Top;
            this.UniqueID.Location = new System.Drawing.Point(0, 93);
            this.UniqueID.Name = "UniqueID";
            this.UniqueID.Size = new System.Drawing.Size(357, 20);
            this.UniqueID.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(359, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "Unique ID: (to identify the choice in the character file, and as Resource ID)";
            // 
            // Recharge
            // 
            this.Recharge.Dock = System.Windows.Forms.DockStyle.Top;
            this.Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Recharge.FormattingEnabled = true;
            this.Recharge.Location = new System.Drawing.Point(0, 59);
            this.Recharge.Name = "Recharge";
            this.Recharge.Size = new System.Drawing.Size(357, 21);
            this.Recharge.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Recharge:";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Top;
            this.Amount.Location = new System.Drawing.Point(0, 26);
            this.Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(357, 20);
            this.Amount.TabIndex = 39;
            this.Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Location = new System.Drawing.Point(0, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(175, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Amount of Selectable Bonus Spells:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 29;
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
            this.keywordControl1.Size = new System.Drawing.Size(362, 343);
            this.keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keywords to add to the spell(s)";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(723, 25);
            this.button1.TabIndex = 20;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // BonusSpellKeywordChoiceFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(727, 660);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BonusSpellKeywordChoiceFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bonus Spell Choice Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private KeywordControl keywordControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckedListBox SpellcastingModifier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox UniqueID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Recharge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Amount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.Label label6;
    }
}