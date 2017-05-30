namespace Character_Builder_Builder.FeatureForms
{
    partial class SpeedFeatureForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.IgnoreArmor = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ExtraSpeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BaseSpeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BaseSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 466);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(777, 25);
            this.button1.TabIndex = 23;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.IgnoreArmor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 446);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(777, 20);
            this.panel1.TabIndex = 30;
            // 
            // IgnoreArmor
            // 
            this.IgnoreArmor.AutoSize = true;
            this.IgnoreArmor.Dock = System.Windows.Forms.DockStyle.Right;
            this.IgnoreArmor.Location = new System.Drawing.Point(649, 0);
            this.IgnoreArmor.Name = "IgnoreArmor";
            this.IgnoreArmor.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.IgnoreArmor.Size = new System.Drawing.Size(128, 20);
            this.IgnoreArmor.TabIndex = 0;
            this.IgnoreArmor.Text = "Ignore Armor Penalty";
            this.IgnoreArmor.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoEllipsis = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label9.Location = new System.Drawing.Point(2, 433);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(777, 13);
            this.label9.TabIndex = 127;
            this.label9.Text = "Note: The Bonus expression must result in a number, the values are the same as fo" +
    "r the activation";
            // 
            // ExtraSpeed
            // 
            this.ExtraSpeed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ExtraSpeed.Location = new System.Drawing.Point(2, 413);
            this.ExtraSpeed.Name = "ExtraSpeed";
            this.ExtraSpeed.Size = new System.Drawing.Size(777, 20);
            this.ExtraSpeed.TabIndex = 146;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 13);
            this.label3.TabIndex = 157;
            this.label3.Text = "Condition for Activation: (NCalc Expression)";
            // 
            // Condition
            // 
            this.Condition.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Condition.Location = new System.Drawing.Point(2, 269);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(777, 20);
            this.Condition.TabIndex = 156;
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(777, 13);
            this.label4.TabIndex = 155;
            this.label4.Text = "Note: The condition results in a true value if the bonus should be applied and fa" +
    "lse otherwise";
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(2, 302);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(777, 13);
            this.label5.TabIndex = 154;
            this.label5.Text = "The following string values are known: Category (of the worn armor), Name (of the" +
    " worn armor)";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(2, 315);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(777, 13);
            this.label6.TabIndex = 153;
            this.label6.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label11
            // 
            this.label11.AutoEllipsis = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label11.Location = new System.Drawing.Point(2, 328);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label11.Size = new System.Drawing.Size(777, 13);
            this.label11.TabIndex = 152;
            this.label11.Text = "PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel o" +
    "therwise), ClassLevel(\"classname\") , function for classlevel";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 341);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(777, 13);
            this.label7.TabIndex = 151;
            this.label7.Text = "The following boolean values are known: All Keywords of the worn Armor, Light, He" +
    "avy, Medium";
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(2, 354);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(777, 13);
            this.label8.TabIndex = 150;
            this.label8.Text = "In addition:  Unarmored, Two_Handed (wielding two-handed weapon), Offhand (wieldi" +
    "ng offhand weapon), FreeHand, Shield (wielding shield).";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 149;
            this.label2.Text = "Base Speed:";
            // 
            // BaseSpeed
            // 
            this.BaseSpeed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BaseSpeed.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.BaseSpeed.Location = new System.Drawing.Point(2, 380);
            this.BaseSpeed.Name = "BaseSpeed";
            this.BaseSpeed.Size = new System.Drawing.Size(777, 20);
            this.BaseSpeed.TabIndex = 148;
            this.BaseSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 400);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 147;
            this.label1.Text = "Bonus Speed (NClalc):";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(777, 254);
            this.basicFeature1.TabIndex = 158;
            // 
            // SpeedFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(781, 493);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Condition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseSpeed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExtraSpeed);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "SpeedFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Speed Feature";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BaseSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox IgnoreArmor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ExtraSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BaseSpeed;
        private System.Windows.Forms.Label label1;
        private BasicFeature basicFeature1;
    }
}