namespace Character_Builder_Builder.FeatureForms
{
    partial class HitPointsFeatureForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 297);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(625, 25);
            this.button1.TabIndex = 27;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 126;
            this.label4.Text = "Amount: (NCalc)";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Amount.Location = new System.Drawing.Point(2, 238);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(625, 20);
            this.Amount.TabIndex = 125;
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(2, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(625, 13);
            this.label6.TabIndex = 124;
            this.label6.Text = "Note: The Amount expression must result in a number:";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 271);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(625, 13);
            this.label7.TabIndex = 123;
            this.label7.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(2, 284);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(625, 13);
            this.label8.TabIndex = 122;
            this.label8.Text = "PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel o" +
    "therwise), ClassLevel(\"classname\") , function for classlevel";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(625, 223);
            this.basicFeature1.TabIndex = 127;
            // 
            // HitPointsFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(629, 324);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Name = "HitPointsFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hit Point Feature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Amount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private BasicFeature basicFeature1;
    }
}