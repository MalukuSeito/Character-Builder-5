namespace Character_Builder_Builder.FeatureForms
{
    partial class FreeItemAndGoldFeatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreeItemAndGoldFeatureForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.stringList1 = new Character_Builder_Builder.StringList();
            this.label4 = new System.Windows.Forms.Label();
            this.CP = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.SP = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.GP = new System.Windows.Forms.NumericUpDown();
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
            ((System.ComponentModel.ISupportInitialize)(this.CP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GP)).BeginInit();
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
            this.splitContainer1.Size = new System.Drawing.Size(636, 371);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 0;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(0, 0);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(636, 238);
            this.basicFeature1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.stringList1);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.CP);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.SP);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.GP);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(636, 104);
            this.splitContainer2.SplitterDistance = 402;
            this.splitContainer2.TabIndex = 27;
            // 
            // stringList1
            // 
            this.stringList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringList1.HistoryManager = null;
            this.stringList1.Items = null;
            this.stringList1.Location = new System.Drawing.Point(0, 13);
            this.stringList1.Name = "stringList1";
            this.stringList1.Size = new System.Drawing.Size(402, 91);
            this.stringList1.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("stringList1.Suggestions")));
            this.stringList1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Items:";
            // 
            // CP
            // 
            this.CP.Dock = System.Windows.Forms.DockStyle.Top;
            this.CP.Location = new System.Drawing.Point(0, 79);
            this.CP.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.CP.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.CP.Name = "CP";
            this.CP.Size = new System.Drawing.Size(230, 20);
            this.CP.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CP:";
            // 
            // SP
            // 
            this.SP.Dock = System.Windows.Forms.DockStyle.Top;
            this.SP.Location = new System.Drawing.Point(0, 46);
            this.SP.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.SP.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.SP.Name = "SP";
            this.SP.Size = new System.Drawing.Size(230, 20);
            this.SP.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "SP:";
            // 
            // GP
            // 
            this.GP.Dock = System.Windows.Forms.DockStyle.Top;
            this.GP.Location = new System.Drawing.Point(0, 13);
            this.GP.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.GP.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.GP.Name = "GP";
            this.GP.Size = new System.Drawing.Size(230, 20);
            this.GP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "GP:";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 104);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(636, 25);
            this.button1.TabIndex = 26;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FreeItemAndGoldFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(640, 375);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FreeItemAndGoldFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Free Item / Gold Feature";
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
            ((System.ComponentModel.ISupportInitialize)(this.CP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private StringList stringList1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown CP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown GP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}