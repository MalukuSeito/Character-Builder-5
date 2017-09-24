namespace Character_Builder_Builder.FeatureForms
{
    partial class MultiFeatureForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.features1 = new Character_Builder_Builder.Features();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 541);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1034, 25);
            this.button1.TabIndex = 21;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.preview);
            this.splitContainer1.Size = new System.Drawing.Size(1034, 539);
            this.splitContainer1.SplitterDistance = 761;
            this.splitContainer1.TabIndex = 22;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.basicFeature1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.features1);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.Amount);
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.label6);
            this.splitContainer2.Panel2.Controls.Add(this.label7);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(761, 539);
            this.splitContainer2.SplitterDistance = 260;
            this.splitContainer2.TabIndex = 23;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(0, 0);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(761, 260);
            this.basicFeature1.TabIndex = 0;
            // 
            // features1
            // 
            this.features1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.features1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.features1.features = null;
            this.features1.HistoryManager = null;
            this.features1.Location = new System.Drawing.Point(0, 46);
            this.features1.Name = "features1";
            this.features1.preview = this.preview;
            this.features1.Size = new System.Drawing.Size(761, 177);
            this.features1.TabIndex = 140;
            // 
            // preview
            // 
            this.preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.preview.Location = new System.Drawing.Point(0, 0);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(269, 539);
            this.preview.TabIndex = 0;
            this.preview.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 139;
            this.label1.Text = "Features:";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Top;
            this.Amount.Location = new System.Drawing.Point(0, 13);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(761, 20);
            this.Amount.TabIndex = 137;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 136;
            this.label4.Text = "Condtion (NCalc):";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(761, 13);
            this.label6.TabIndex = 135;
            this.label6.Text = "Note: If no condition is set or the condtion evaluates to true, the features are " +
    "added to the character (The stats are before boni are added from features)";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(0, 236);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(761, 13);
            this.label7.TabIndex = 134;
            this.label7.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(0, 249);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(761, 13);
            this.label8.TabIndex = 133;
            this.label8.Text = "PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel o" +
    "therwise), ClassLevel(\"classname\") , function for classlevel";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 262);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(761, 13);
            this.label2.TabIndex = 132;
            this.label2.Text = "The following text values are known: Race, SubRace, SubClass(\"classname\"): names " +
    "of the subrace, race and subclasses respectively";
            // 
            // MultiFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(1038, 568);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Name = "MultiFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Multi-Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.WebBrowser preview;
        private Features features1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Amount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
    }
}