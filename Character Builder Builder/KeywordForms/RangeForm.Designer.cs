namespace Character_Builder_Builder.KeywordForms
{
    partial class RangeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ShortRange = new System.Windows.Forms.NumericUpDown();
            this.LongRange = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ShortRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongRange)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(261, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Short Range:";
            // 
            // ShortRange
            // 
            this.ShortRange.Dock = System.Windows.Forms.DockStyle.Top;
            this.ShortRange.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ShortRange.Location = new System.Drawing.Point(0, 13);
            this.ShortRange.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ShortRange.Name = "ShortRange";
            this.ShortRange.Size = new System.Drawing.Size(261, 20);
            this.ShortRange.TabIndex = 23;
            // 
            // LongRange
            // 
            this.LongRange.Dock = System.Windows.Forms.DockStyle.Top;
            this.LongRange.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.LongRange.Location = new System.Drawing.Point(0, 46);
            this.LongRange.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.LongRange.Name = "LongRange";
            this.LongRange.Size = new System.Drawing.Size(261, 20);
            this.LongRange.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Long Range:";
            // 
            // RangeForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(261, 89);
            this.Controls.Add(this.LongRange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ShortRange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "RangeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Material Keyword";
            ((System.ComponentModel.ISupportInitialize)(this.ShortRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LongRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ShortRange;
        private System.Windows.Forms.NumericUpDown LongRange;
        private System.Windows.Forms.Label label2;
    }
}