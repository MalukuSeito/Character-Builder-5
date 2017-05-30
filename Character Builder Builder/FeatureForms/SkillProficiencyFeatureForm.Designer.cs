namespace Character_Builder_Builder.FeatureForms
{
    partial class SkillProficiencyFeatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkillProficiencyFeatureForm));
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.SkillList = new Character_Builder_Builder.StringList();
            this.label17 = new System.Windows.Forms.Label();
            this.ProfMultiplier = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfMultiplier)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 474);
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
            this.splitContainer1.Panel2.Controls.Add(this.ProfMultiplier);
            this.splitContainer1.Panel2.Controls.Add(this.label18);
            this.splitContainer1.Size = new System.Drawing.Size(743, 472);
            this.splitContainer1.SplitterDistance = 211;
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
            this.basicFeature1.Size = new System.Drawing.Size(743, 211);
            this.basicFeature1.TabIndex = 45;
            // 
            // SkillList
            // 
            this.SkillList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SkillList.HistoryManager = null;
            this.SkillList.Items = null;
            this.SkillList.Location = new System.Drawing.Point(0, 46);
            this.SkillList.Name = "SkillList";
            this.SkillList.Size = new System.Drawing.Size(743, 211);
            this.SkillList.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("SkillList.Suggestions")));
            this.SkillList.TabIndex = 103;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.Location = new System.Drawing.Point(0, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(272, 13);
            this.label17.TabIndex = 102;
            this.label17.Text = "Skills: (empty list = all skill the character is proficient with)";
            // 
            // ProfMultiplier
            // 
            this.ProfMultiplier.DecimalPlaces = 2;
            this.ProfMultiplier.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProfMultiplier.Location = new System.Drawing.Point(0, 13);
            this.ProfMultiplier.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.ProfMultiplier.Name = "ProfMultiplier";
            this.ProfMultiplier.Size = new System.Drawing.Size(743, 20);
            this.ProfMultiplier.TabIndex = 54;
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
            // SkillProficiencyFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(747, 501);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Name = "SkillProficiencyFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Skill Proficiency Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProfMultiplier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.NumericUpDown ProfMultiplier;
        private System.Windows.Forms.Label label18;
        private StringList SkillList;
        private System.Windows.Forms.Label label17;
    }
}