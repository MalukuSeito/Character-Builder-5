namespace Character_Builder_Builder.FeatureForms
{
    partial class FormsCompanionsFeatureForm
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
            this.FormCompCount = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.FormsCompanionOptions = new System.Windows.Forms.TextBox();
            this.lcalN = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.UniqueID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DisplayName = new System.Windows.Forms.TextBox();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            ((System.ComponentModel.ISupportInitialize)(this.FormCompCount)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 401);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(777, 25);
            this.button1.TabIndex = 23;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormCompCount
            // 
            this.FormCompCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FormCompCount.Location = new System.Drawing.Point(2, 381);
            this.FormCompCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.FormCompCount.Name = "FormCompCount";
            this.FormCompCount.Size = new System.Drawing.Size(777, 20);
            this.FormCompCount.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 368);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(243, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "Forms / Companions Selectable Count (-1: infinte):";
            // 
            // FormsCompanionOptions
            // 
            this.FormsCompanionOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FormsCompanionOptions.Location = new System.Drawing.Point(2, 348);
            this.FormsCompanionOptions.Name = "FormsCompanionOptions";
            this.FormsCompanionOptions.Size = new System.Drawing.Size(777, 20);
            this.FormsCompanionOptions.TabIndex = 53;
            // 
            // lcalN
            // 
            this.lcalN.AutoSize = true;
            this.lcalN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lcalN.Location = new System.Drawing.Point(2, 335);
            this.lcalN.Name = "lcalN";
            this.lcalN.Size = new System.Drawing.Size(321, 13);
            this.lcalN.TabIndex = 54;
            this.lcalN.Text = "Forms / Companions Options (for polymorphs, conjurations; NCalc):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(747, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "Unique ID (Features with the same ID have their  Count added together and Options" +
    " combined; A spell\'s (with forms/companions) ID is it\'s name in lowercase)";
            // 
            // UniqueID
            // 
            this.UniqueID.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UniqueID.Location = new System.Drawing.Point(2, 315);
            this.UniqueID.Name = "UniqueID";
            this.UniqueID.Size = new System.Drawing.Size(777, 20);
            this.UniqueID.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Display Name:";
            // 
            // DisplayName
            // 
            this.DisplayName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DisplayName.Location = new System.Drawing.Point(2, 282);
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.Size = new System.Drawing.Size(777, 20);
            this.DisplayName.TabIndex = 58;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(777, 267);
            this.basicFeature1.TabIndex = 60;
            // 
            // FormsCompanionsFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(781, 428);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DisplayName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UniqueID);
            this.Controls.Add(this.lcalN);
            this.Controls.Add(this.FormsCompanionOptions);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FormCompCount);
            this.Controls.Add(this.button1);
            this.Name = "FormsCompanionsFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form / Companion Feature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormsCompanionsFeatureForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.FormCompCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown FormCompCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox FormsCompanionOptions;
        private System.Windows.Forms.Label lcalN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UniqueID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DisplayName;
        private BasicFeature basicFeature1;
    }
}