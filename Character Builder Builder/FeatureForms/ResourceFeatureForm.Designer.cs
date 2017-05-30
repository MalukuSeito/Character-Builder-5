namespace Character_Builder_Builder.FeatureForms
{
    partial class ResourceFeatureForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ResourceID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExclusionID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Recharge = new System.Windows.Forms.ComboBox();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 433);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(733, 25);
            this.button1.TabIndex = 58;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(2, 420);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(733, 13);
            this.label8.TabIndex = 111;
            this.label8.Text = "PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel o" +
    "therwise), ClassLevel(\"classname\") , function for classlevel";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(2, 394);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(733, 13);
            this.label6.TabIndex = 113;
            this.label6.Text = "Note: The Amount expression must result in a number:";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 407);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(733, 13);
            this.label7.TabIndex = 112;
            this.label7.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 121;
            this.label4.Text = "Amount (NCalc):";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Amount.Location = new System.Drawing.Point(2, 272);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(733, 20);
            this.Amount.TabIndex = 120;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(733, 13);
            this.label1.TabIndex = 119;
            this.label1.Text = "Resource ID (if multiple features have the same Resource ID, they are added toget" +
    "her)";
            // 
            // ResourceID
            // 
            this.ResourceID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ResourceID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ResourceID.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ResourceID.FormattingEnabled = true;
            this.ResourceID.Location = new System.Drawing.Point(2, 305);
            this.ResourceID.Name = "ResourceID";
            this.ResourceID.Size = new System.Drawing.Size(733, 21);
            this.ResourceID.TabIndex = 118;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 326);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(733, 13);
            this.label2.TabIndex = 117;
            this.label2.Text = "Exclusion ID: (Any other resource with a different Resource ID and the same Exclu" +
    "sion ID is ignored, i.e. Channel Divinity Multiclassing)";
            // 
            // ExclusionID
            // 
            this.ExclusionID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ExclusionID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ExclusionID.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExclusionID.FormattingEnabled = true;
            this.ExclusionID.Location = new System.Drawing.Point(2, 339);
            this.ExclusionID.Name = "ExclusionID";
            this.ExclusionID.Size = new System.Drawing.Size(733, 21);
            this.ExclusionID.TabIndex = 116;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 360);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 115;
            this.label3.Text = "Recharge:";
            // 
            // Recharge
            // 
            this.Recharge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Recharge.FormattingEnabled = true;
            this.Recharge.Location = new System.Drawing.Point(2, 373);
            this.Recharge.Name = "Recharge";
            this.Recharge.Size = new System.Drawing.Size(733, 21);
            this.Recharge.TabIndex = 114;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(733, 257);
            this.basicFeature1.TabIndex = 122;
            // 
            // ResourceFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(737, 460);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResourceID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExclusionID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Recharge);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Name = "ResourceFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resource Feature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ResourceFeatureForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Amount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ResourceID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ExclusionID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Recharge;
        private BasicFeature basicFeature1;
    }
}