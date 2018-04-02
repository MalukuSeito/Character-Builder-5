namespace Character_Builder_Builder.FeatureForms
{
    partial class ResistanceFeatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResistanceFeatureForm));
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.Vulnerabilities = new Character_Builder_Builder.StringList();
            this.label2 = new System.Windows.Forms.Label();
            this.Immunities = new Character_Builder_Builder.StringList();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.Resistances = new Character_Builder_Builder.StringList();
            this.label1 = new System.Windows.Forms.Label();
            this.SavingThrowAdvantages = new Character_Builder_Builder.StringList();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 510);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(777, 25);
            this.button1.TabIndex = 23;
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(777, 508);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 24;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(0, 0);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(777, 267);
            this.basicFeature1.TabIndex = 27;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(777, 237);
            this.splitContainer2.SplitterDistance = 405;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.Vulnerabilities);
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.Immunities);
            this.splitContainer3.Panel2.Controls.Add(this.label3);
            this.splitContainer3.Size = new System.Drawing.Size(368, 237);
            this.splitContainer3.SplitterDistance = 185;
            this.splitContainer3.TabIndex = 0;
            // 
            // Vulnerabilities
            // 
            this.Vulnerabilities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Vulnerabilities.HistoryManager = null;
            this.Vulnerabilities.Items = null;
            this.Vulnerabilities.Location = new System.Drawing.Point(0, 13);
            this.Vulnerabilities.Name = "Vulnerabilities";
            this.Vulnerabilities.Size = new System.Drawing.Size(185, 224);
            this.Vulnerabilities.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("Vulnerabilities.Suggestions")));
            this.Vulnerabilities.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Vulnerabilities:";
            // 
            // Immunities
            // 
            this.Immunities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Immunities.HistoryManager = null;
            this.Immunities.Items = null;
            this.Immunities.Location = new System.Drawing.Point(0, 13);
            this.Immunities.Name = "Immunities";
            this.Immunities.Size = new System.Drawing.Size(179, 224);
            this.Immunities.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("Immunities.Suggestions")));
            this.Immunities.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Immunities:";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.Resistances);
            this.splitContainer4.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.SavingThrowAdvantages);
            this.splitContainer4.Panel2.Controls.Add(this.label4);
            this.splitContainer4.Size = new System.Drawing.Size(405, 237);
            this.splitContainer4.SplitterDistance = 199;
            this.splitContainer4.TabIndex = 0;
            // 
            // Resistances
            // 
            this.Resistances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Resistances.HistoryManager = null;
            this.Resistances.Items = null;
            this.Resistances.Location = new System.Drawing.Point(0, 13);
            this.Resistances.Name = "Resistances";
            this.Resistances.Size = new System.Drawing.Size(199, 224);
            this.Resistances.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("Resistances.Suggestions")));
            this.Resistances.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Resistances:";
            // 
            // SavingThrowAdvantages
            // 
            this.SavingThrowAdvantages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SavingThrowAdvantages.HistoryManager = null;
            this.SavingThrowAdvantages.Items = null;
            this.SavingThrowAdvantages.Location = new System.Drawing.Point(0, 13);
            this.SavingThrowAdvantages.Name = "SavingThrowAdvantages";
            this.SavingThrowAdvantages.Size = new System.Drawing.Size(202, 224);
            this.SavingThrowAdvantages.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("SavingThrowAdvantages.Suggestions")));
            this.SavingThrowAdvantages.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Saving Throw Advantages:";
            // 
            // ResistanceFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(781, 537);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Name = "ResistanceFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resistance Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private StringList Vulnerabilities;
        private System.Windows.Forms.Label label2;
        private StringList Immunities;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private StringList Resistances;
        private System.Windows.Forms.Label label1;
        private StringList SavingThrowAdvantages;
        private System.Windows.Forms.Label label4;
    }
}