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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            basicFeature1 = new BasicFeature();
            label2 = new System.Windows.Forms.Label();
            Condition = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            SpellcastingModifier = new System.Windows.Forms.CheckedListBox();
            AddToSpellcastingCheckbox = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            UniqueID = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            Recharge = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            Amount = new System.Windows.Forms.NumericUpDown();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            keywordControl1 = new KeywordControl();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Amount).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(3, 2);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(basicFeature1);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(Condition);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(label7);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Size = new System.Drawing.Size(842, 757);
            splitContainer1.SplitterDistance = 311;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 20;
            // 
            // basicFeature1
            // 
            basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            basicFeature1.Feature = null;
            basicFeature1.Location = new System.Drawing.Point(0, 0);
            basicFeature1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            basicFeature1.Name = "basicFeature1";
            basicFeature1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            basicFeature1.Size = new System.Drawing.Size(842, 243);
            basicFeature1.TabIndex = 57;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            label2.Location = new System.Drawing.Point(0, 243);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(190, 15);
            label2.TabIndex = 56;
            label2.Text = "Affected Spells: (NCalc Expression)";
            // 
            // Condition
            // 
            Condition.Dock = System.Windows.Forms.DockStyle.Bottom;
            Condition.Location = new System.Drawing.Point(0, 258);
            Condition.Margin = new System.Windows.Forms.Padding(4);
            Condition.Name = "Condition";
            Condition.Size = new System.Drawing.Size(842, 23);
            Condition.TabIndex = 55;
            // 
            // label6
            // 
            label6.AutoEllipsis = true;
            label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            label6.Location = new System.Drawing.Point(0, 281);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(842, 15);
            label6.TabIndex = 53;
            label6.Text = "Note: Only spells matching the condition can be selected.";
            // 
            // label7
            // 
            label7.AutoEllipsis = true;
            label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            label7.Location = new System.Drawing.Point(0, 296);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            label7.Size = new System.Drawing.Size(842, 15);
            label7.TabIndex = 49;
            label7.Text = "Variables: Name (spell: string, lowercase), Level (spell: number). the spell keywords as boolean (see right for examples)";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(SpellcastingModifier);
            splitContainer2.Panel1.Controls.Add(AddToSpellcastingCheckbox);
            splitContainer2.Panel1.Controls.Add(label4);
            splitContainer2.Panel1.Controls.Add(UniqueID);
            splitContainer2.Panel1.Controls.Add(label5);
            splitContainer2.Panel1.Controls.Add(Recharge);
            splitContainer2.Panel1.Controls.Add(label3);
            splitContainer2.Panel1.Controls.Add(Amount);
            splitContainer2.Panel1.Controls.Add(label9);
            splitContainer2.Panel1.Controls.Add(label8);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(keywordControl1);
            splitContainer2.Panel2.Controls.Add(label1);
            splitContainer2.Size = new System.Drawing.Size(842, 412);
            splitContainer2.SplitterDistance = 415;
            splitContainer2.TabIndex = 21;
            // 
            // SpellcastingModifier
            // 
            SpellcastingModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            SpellcastingModifier.FormattingEnabled = true;
            SpellcastingModifier.Location = new System.Drawing.Point(0, 144);
            SpellcastingModifier.Margin = new System.Windows.Forms.Padding(4);
            SpellcastingModifier.Name = "SpellcastingModifier";
            SpellcastingModifier.Size = new System.Drawing.Size(415, 249);
            SpellcastingModifier.TabIndex = 53;
            SpellcastingModifier.ItemCheck += SpellcastingModifier_ItemCheck;
            // 
            // AddToSpellcastingCheckbox
            // 
            AddToSpellcastingCheckbox.AutoSize = true;
            AddToSpellcastingCheckbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            AddToSpellcastingCheckbox.Location = new System.Drawing.Point(0, 393);
            AddToSpellcastingCheckbox.Name = "AddToSpellcastingCheckbox";
            AddToSpellcastingCheckbox.Size = new System.Drawing.Size(415, 19);
            AddToSpellcastingCheckbox.TabIndex = 52;
            AddToSpellcastingCheckbox.Text = "Add Spell to All Spellcasting";
            AddToSpellcastingCheckbox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Location = new System.Drawing.Point(0, 129);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(110, 15);
            label4.TabIndex = 50;
            label4.Text = "Spellcasting Ability:";
            // 
            // UniqueID
            // 
            UniqueID.Dock = System.Windows.Forms.DockStyle.Top;
            UniqueID.Location = new System.Drawing.Point(0, 106);
            UniqueID.Margin = new System.Windows.Forms.Padding(4);
            UniqueID.Name = "UniqueID";
            UniqueID.Size = new System.Drawing.Size(415, 23);
            UniqueID.TabIndex = 49;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 91);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(394, 15);
            label5.TabIndex = 48;
            label5.Text = "Unique ID: (to identify the choice in the character file, and as Resource ID)";
            // 
            // Recharge
            // 
            Recharge.Dock = System.Windows.Forms.DockStyle.Top;
            Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Recharge.FormattingEnabled = true;
            Recharge.Location = new System.Drawing.Point(0, 68);
            Recharge.Margin = new System.Windows.Forms.Padding(4);
            Recharge.Name = "Recharge";
            Recharge.Size = new System.Drawing.Size(415, 23);
            Recharge.TabIndex = 47;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(0, 53);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(59, 15);
            label3.TabIndex = 46;
            label3.Text = "Recharge:";
            // 
            // Amount
            // 
            Amount.Dock = System.Windows.Forms.DockStyle.Top;
            Amount.Location = new System.Drawing.Point(0, 30);
            Amount.Margin = new System.Windows.Forms.Padding(4);
            Amount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Amount.Name = "Amount";
            Amount.Size = new System.Drawing.Size(415, 23);
            Amount.TabIndex = 39;
            Amount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = System.Windows.Forms.DockStyle.Top;
            label9.Location = new System.Drawing.Point(0, 15);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(193, 15);
            label9.TabIndex = 38;
            label9.Text = "Amount of Selectable Bonus Spells:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = System.Windows.Forms.DockStyle.Top;
            label8.Location = new System.Drawing.Point(0, 0);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Padding = new System.Windows.Forms.Padding(35, 0, 0, 0);
            label8.Size = new System.Drawing.Size(35, 15);
            label8.TabIndex = 29;
            // 
            // keywordControl1
            // 
            keywordControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            keywordControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            keywordControl1.Group = KeywordControl.KeywordGroup.SPELL;
            keywordControl1.HistoryManager = null;
            keywordControl1.Keywords = null;
            keywordControl1.Location = new System.Drawing.Point(0, 15);
            keywordControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            keywordControl1.Name = "keywordControl1";
            keywordControl1.Size = new System.Drawing.Size(423, 397);
            keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(169, 15);
            label1.TabIndex = 0;
            label1.Text = "Keywords to add to the spell(s)";
            // 
            // button1
            // 
            button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            button1.Location = new System.Drawing.Point(0, 412);
            button1.Margin = new System.Windows.Forms.Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(842, 29);
            button1.TabIndex = 20;
            button1.Text = "Okay";
            button1.UseVisualStyleBackColor = true;
            // 
            // BonusSpellKeywordChoiceFeatureForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            CancelButton = button1;
            ClientSize = new System.Drawing.Size(848, 761);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "BonusSpellKeywordChoiceFeatureForm";
            Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bonus Spell Choice Feature";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Amount).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private KeywordControl keywordControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.CheckedListBox SpellcastingModifier;
        private System.Windows.Forms.CheckBox AddToSpellcastingCheckbox;
    }
}