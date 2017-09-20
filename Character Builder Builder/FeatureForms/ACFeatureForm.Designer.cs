namespace Character_Builder_Builder.FeatureForms
{
    partial class ACFeatureForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Expr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 383);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(768, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 370);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(768, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "The following string values are available: Category (Category of the equipped Arm" +
    "or), Name (Name of the Armor).";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 357);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(768, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "as well as any Keywords the Armor has.";
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 344);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(768, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "The following boolean flags are available: Unarmored, Armor, OffHand (weapon in o" +
    "ff-Hand), Shield, Two-Handed (weapon), FreeHand";
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 331);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(768, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Str, Dex, Con, Int, Wis, Cha (Total value), StrMod, DexMod, ConMod, IntMod, WisMo" +
    "d, ChaMod (Modifier).";
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(2, 318);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(768, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "The following values are available: BaseAC (of Armor), ShieldBonus (if equiped), " +
    "ACBonus (Bonus that will be added due to other features),";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(2, 305);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(768, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Note: The expression must result in a number. If there are multiple AC Calculatio" +
    "n features, the one returning the highest AC is taken.";
            // 
            // Expr
            // 
            this.Expr.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Expr.Location = new System.Drawing.Point(2, 285);
            this.Expr.Name = "Expr";
            this.Expr.Size = new System.Drawing.Size(768, 20);
            this.Expr.TabIndex = 10;
            this.Expr.Text = "asdf";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Expression: (NCalc)";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(768, 270);
            this.basicFeature1.TabIndex = 12;
            // 
            // ACFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(772, 410);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Expr);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "ACFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "AC Calculation Feature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Expr;
        private System.Windows.Forms.Label label7;
        private BasicFeature basicFeature1;
    }
}