namespace Character_Builder_Builder.FeatureForms
{
    partial class ToolKWProficiencyFeatureForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.label2 = new System.Windows.Forms.Label();
            this.Expression = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 389);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(770, 25);
            this.button1.TabIndex = 22;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 376);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(770, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "In addition all keywords of the item can be used as boolean values as well.";
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 363);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(770, 13);
            this.label4.TabIndex = 80;
            this.label4.Text = "The following can be used to differentiate types: Armor, Tool, Weapon, Shield. Al" +
    "l are boolean values.";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(770, 282);
            this.basicFeature1.TabIndex = 88;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 284);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(494, 13);
            this.label2.TabIndex = 87;
            this.label2.Text = "Proficiency Expression: (NCalc Expression, character is proficient with all items" +
    " matching the expression)";
            // 
            // Expression
            // 
            this.Expression.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Expression.Location = new System.Drawing.Point(2, 297);
            this.Expression.Name = "Expression";
            this.Expression.Size = new System.Drawing.Size(770, 20);
            this.Expression.TabIndex = 86;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(2, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 13);
            this.label5.TabIndex = 83;
            this.label5.Text = "Description of the Expression: (shown on sheet)";
            // 
            // Description
            // 
            this.Description.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Description.Location = new System.Drawing.Point(2, 330);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(770, 20);
            this.Description.TabIndex = 82;
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 350);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(770, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Note: The Expression must result in a boolean value. The following values can be " +
    "used: Category, Name (lowercase) of the item";
            // 
            // ToolKWProficiencyFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(774, 416);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Expression);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "ToolKWProficiencyFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tool Proficiency by Expression Feature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Expression;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Description;
        private System.Windows.Forms.Label label3;
    }
}