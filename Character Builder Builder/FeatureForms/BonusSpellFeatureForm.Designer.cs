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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SpellcastingModifier = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Recharge = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Spell = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(723, 503);
            this.splitContainer1.SplitterDistance = 284;
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
            this.basicFeature1.Size = new System.Drawing.Size(723, 284);
            this.basicFeature1.TabIndex = 45;
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
            this.splitContainer2.Panel1.Controls.Add(this.Recharge);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.Spell);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.keywordControl1);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(723, 190);
            this.splitContainer2.SplitterDistance = 357;
            this.splitContainer2.TabIndex = 21;
            // 
            // SpellcastingModifier
            // 
            this.SpellcastingModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpellcastingModifier.FormattingEnabled = true;
            this.SpellcastingModifier.Location = new System.Drawing.Point(0, 81);
            this.SpellcastingModifier.Name = "SpellcastingModifier";
            this.SpellcastingModifier.Size = new System.Drawing.Size(357, 109);
            this.SpellcastingModifier.TabIndex = 5;
            this.SpellcastingModifier.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SpellcastingModifier_ItemCheck);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Spellcasting Ability:";
            // 
            // Recharge
            // 
            this.Recharge.Dock = System.Windows.Forms.DockStyle.Top;
            this.Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Recharge.FormattingEnabled = true;
            this.Recharge.Location = new System.Drawing.Point(0, 47);
            this.Recharge.Name = "Recharge";
            this.Recharge.Size = new System.Drawing.Size(357, 21);
            this.Recharge.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Recharge:";
            // 
            // Spell
            // 
            this.Spell.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Spell.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Spell.Dock = System.Windows.Forms.DockStyle.Top;
            this.Spell.FormattingEnabled = true;
            this.Spell.Location = new System.Drawing.Point(0, 13);
            this.Spell.Name = "Spell";
            this.Spell.Size = new System.Drawing.Size(357, 21);
            this.Spell.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Spell:";
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
            this.keywordControl1.Size = new System.Drawing.Size(362, 177);
            this.keywordControl1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Keywords to add to the Spell";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(723, 25);
            this.button1.TabIndex = 20;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // BonusSpellFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(727, 507);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BonusSpellFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bonus Spell Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckedListBox SpellcastingModifier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Recharge;
        private System.Windows.Forms.Label label3;
    }
}