namespace Character_Builder_Builder.FeatureForms
{
    partial class SubClassFeatureForm
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
            this.Parentclass = new System.Windows.Forms.ComboBox();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 335);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(777, 25);
            this.button1.TabIndex = 23;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Parentclass
            // 
            this.Parentclass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Parentclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Parentclass.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Parentclass.FormattingEnabled = true;
            this.Parentclass.Location = new System.Drawing.Point(2, 314);
            this.Parentclass.Name = "Parentclass";
            this.Parentclass.Size = new System.Drawing.Size(777, 21);
            this.Parentclass.TabIndex = 27;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(777, 299);
            this.basicFeature1.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Allow Subclass Options for Class: (usually same as the class this is defined in)";
            // 
            // SubClassFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(781, 362);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Parentclass);
            this.Controls.Add(this.button1);
            this.Name = "SubClassFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Subclass Feature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox Parentclass;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.Label label1;
    }
}