namespace Character_Builder_Builder.FeatureForms
{
    partial class BonusSpellFeatureForm
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
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            SpellcastingModifier = new System.Windows.Forms.CheckedListBox();
            AddToSpellcastingCheckbox = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            Recharge = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            Spell = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
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
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Size = new System.Drawing.Size(842, 581);
            splitContainer1.SplitterDistance = 327;
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
            basicFeature1.Size = new System.Drawing.Size(842, 327);
            basicFeature1.TabIndex = 45;
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
            splitContainer2.Panel1.Controls.Add(Recharge);
            splitContainer2.Panel1.Controls.Add(label3);
            splitContainer2.Panel1.Controls.Add(Spell);
            splitContainer2.Panel1.Controls.Add(label2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(keywordControl1);
            splitContainer2.Panel2.Controls.Add(label1);
            splitContainer2.Size = new System.Drawing.Size(842, 220);
            splitContainer2.SplitterDistance = 415;
            splitContainer2.TabIndex = 21;
            // 
            // SpellcastingModifier
            // 
            SpellcastingModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            SpellcastingModifier.FormattingEnabled = true;
            SpellcastingModifier.Location = new System.Drawing.Point(0, 91);
            SpellcastingModifier.Margin = new System.Windows.Forms.Padding(4);
            SpellcastingModifier.Name = "SpellcastingModifier";
            SpellcastingModifier.Size = new System.Drawing.Size(415, 110);
            SpellcastingModifier.TabIndex = 7;
            SpellcastingModifier.ItemCheck += SpellcastingModifier_ItemCheck;
            // 
            // AddToSpellcastingCheckbox
            // 
            AddToSpellcastingCheckbox.AutoSize = true;
            AddToSpellcastingCheckbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            AddToSpellcastingCheckbox.Location = new System.Drawing.Point(0, 201);
            AddToSpellcastingCheckbox.Name = "AddToSpellcastingCheckbox";
            AddToSpellcastingCheckbox.Size = new System.Drawing.Size(415, 19);
            AddToSpellcastingCheckbox.TabIndex = 6;
            AddToSpellcastingCheckbox.Text = "Add Spell to All Spellcasting";
            AddToSpellcastingCheckbox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Location = new System.Drawing.Point(0, 76);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(110, 15);
            label4.TabIndex = 4;
            label4.Text = "Spellcasting Ability:";
            // 
            // Recharge
            // 
            Recharge.Dock = System.Windows.Forms.DockStyle.Top;
            Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Recharge.FormattingEnabled = true;
            Recharge.Location = new System.Drawing.Point(0, 53);
            Recharge.Margin = new System.Windows.Forms.Padding(4);
            Recharge.Name = "Recharge";
            Recharge.Size = new System.Drawing.Size(415, 23);
            Recharge.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(0, 38);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(59, 15);
            label3.TabIndex = 2;
            label3.Text = "Recharge:";
            // 
            // Spell
            // 
            Spell.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            Spell.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            Spell.Dock = System.Windows.Forms.DockStyle.Top;
            Spell.FormattingEnabled = true;
            Spell.Location = new System.Drawing.Point(0, 15);
            Spell.Margin = new System.Windows.Forms.Padding(4);
            Spell.Name = "Spell";
            Spell.Size = new System.Drawing.Size(415, 23);
            Spell.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(35, 15);
            label2.TabIndex = 0;
            label2.Text = "Spell:";
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
            keywordControl1.Size = new System.Drawing.Size(423, 205);
            keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(157, 15);
            label1.TabIndex = 0;
            label1.Text = "Keywords to add to the Spell";
            // 
            // button1
            // 
            button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            button1.Location = new System.Drawing.Point(0, 220);
            button1.Margin = new System.Windows.Forms.Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(842, 29);
            button1.TabIndex = 20;
            button1.Text = "Okay";
            button1.UseVisualStyleBackColor = true;
            // 
            // BonusSpellFeatureForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            CancelButton = button1;
            ClientSize = new System.Drawing.Size(848, 585);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "BonusSpellFeatureForm";
            Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bonus Spell Feature";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.ComboBox Spell;
        private System.Windows.Forms.Label label2;
        private KeywordControl keywordControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Recharge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox SpellcastingModifier;
        private System.Windows.Forms.CheckBox AddToSpellcastingCheckbox;
    }
}