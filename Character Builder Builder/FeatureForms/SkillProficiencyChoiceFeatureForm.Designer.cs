namespace Character_Builder_Builder.FeatureForms
{
    partial class SkillProficiencyChoiceFeatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkillProficiencyChoiceFeatureForm));
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.Restrict = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BonusType = new System.Windows.Forms.ComboBox();
            this.ProfMultiplier = new System.Windows.Forms.NumericUpDown();
            this.SkillList = new Character_Builder_Builder.StringList();
            this.label17 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.UniqueID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfMultiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 591);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(743, 25);
            this.button1.TabIndex = 18;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.SkillList);
            this.splitContainer1.Panel2.Controls.Add(this.label17);
            this.splitContainer1.Panel2.Controls.Add(this.Amount);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.UniqueID);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.Restrict);
            this.splitContainer1.Panel2.Controls.Add(this.label18);
            this.splitContainer1.Size = new System.Drawing.Size(743, 589);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 45;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(0, 0);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(743, 260);
            this.basicFeature1.TabIndex = 45;
            // 
            // Restrict
            // 
            this.Restrict.AutoSize = true;
            this.Restrict.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Restrict.Location = new System.Drawing.Point(0, 308);
            this.Restrict.Name = "Restrict";
            this.Restrict.Size = new System.Drawing.Size(743, 17);
            this.Restrict.TabIndex = 104;
            this.Restrict.Text = "Show only skills the character is already proficient with (only useful for profic" +
    "iency multiplier > 1)";
            this.Restrict.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Top;
            this.label18.Location = new System.Drawing.Point(0, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(278, 13);
            this.label18.TabIndex = 55;
            this.label18.Text = "Proficiency Multiplier (add the proficiency that many times)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ProfMultiplier);
            this.panel1.Controls.Add(this.BonusType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(743, 21);
            this.panel1.TabIndex = 106;
            // 
            // BonusType
            // 
            this.BonusType.Dock = System.Windows.Forms.DockStyle.Right;
            this.BonusType.FormattingEnabled = true;
            this.BonusType.Location = new System.Drawing.Point(524, 0);
            this.BonusType.Name = "BonusType";
            this.BonusType.Size = new System.Drawing.Size(219, 21);
            this.BonusType.TabIndex = 56;
            // 
            // ProfMultiplier
            // 
            this.ProfMultiplier.DecimalPlaces = 2;
            this.ProfMultiplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfMultiplier.Location = new System.Drawing.Point(0, 0);
            this.ProfMultiplier.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.ProfMultiplier.Name = "ProfMultiplier";
            this.ProfMultiplier.Size = new System.Drawing.Size(524, 20);
            this.ProfMultiplier.TabIndex = 57;
            // 
            // SkillList
            // 
            this.SkillList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SkillList.HistoryManager = null;
            this.SkillList.Items = null;
            this.SkillList.Location = new System.Drawing.Point(0, 113);
            this.SkillList.Name = "SkillList";
            this.SkillList.Size = new System.Drawing.Size(743, 195);
            this.SkillList.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("SkillList.Suggestions")));
            this.SkillList.TabIndex = 112;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.Location = new System.Drawing.Point(0, 100);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(133, 13);
            this.label17.TabIndex = 111;
            this.label17.Text = "Skills: (empty list = all skills)";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Top;
            this.Amount.Location = new System.Drawing.Point(0, 80);
            this.Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(743, 20);
            this.Amount.TabIndex = 110;
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
            this.label9.Location = new System.Drawing.Point(0, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 109;
            this.label9.Text = "Amount of Selectable Skills";
            // 
            // UniqueID
            // 
            this.UniqueID.Dock = System.Windows.Forms.DockStyle.Top;
            this.UniqueID.Location = new System.Drawing.Point(0, 47);
            this.UniqueID.Name = "UniqueID";
            this.UniqueID.Size = new System.Drawing.Size(743, 20);
            this.UniqueID.TabIndex = 108;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 13);
            this.label5.TabIndex = 107;
            this.label5.Text = "Unique ID: (to identify the choice in the character file)";
            // 
            // SkillProficiencyChoiceFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(747, 618);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Name = "SkillProficiencyChoiceFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Skill Choice Proficiency Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProfMultiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox Restrict;
        private StringList SkillList;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown Amount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox UniqueID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ProfMultiplier;
        private System.Windows.Forms.ComboBox BonusType;
    }
}