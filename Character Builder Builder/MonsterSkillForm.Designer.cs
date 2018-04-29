namespace Character_Builder_Builder
{
    partial class MonsterSkillForm
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
            this.Skill = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.text = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Bonus = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Ability = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Bonus)).BeginInit();
            this.SuspendLayout();
            // 
            // Skill
            // 
            this.Skill.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Skill.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Skill.Dock = System.Windows.Forms.DockStyle.Top;
            this.Skill.FormattingEnabled = true;
            this.Skill.Location = new System.Drawing.Point(0, 15);
            this.Skill.Name = "Skill";
            this.Skill.Size = new System.Drawing.Size(284, 21);
            this.Skill.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 15);
            this.label2.TabIndex = 30;
            this.label2.Text = "Skill:";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(0, 145);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(284, 43);
            this.button1.TabIndex = 38;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // text
            // 
            this.text.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.text.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.text.Dock = System.Windows.Forms.DockStyle.Top;
            this.text.Location = new System.Drawing.Point(0, 125);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(284, 20);
            this.text.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(284, 18);
            this.label5.TabIndex = 36;
            this.label5.Text = "Text (optional):";
            // 
            // Bonus
            // 
            this.Bonus.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bonus.Location = new System.Drawing.Point(0, 87);
            this.Bonus.Name = "Bonus";
            this.Bonus.Size = new System.Drawing.Size(284, 20);
            this.Bonus.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 15);
            this.label1.TabIndex = 34;
            this.label1.Text = "Bonus:";
            // 
            // Ability
            // 
            this.Ability.Dock = System.Windows.Forms.DockStyle.Top;
            this.Ability.FormattingEnabled = true;
            this.Ability.Location = new System.Drawing.Point(0, 51);
            this.Ability.Name = "Ability";
            this.Ability.Size = new System.Drawing.Size(284, 21);
            this.Ability.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(284, 15);
            this.label4.TabIndex = 32;
            this.label4.Text = "Base Ability (for custom skills and no modified xsl):";
            // 
            // MonsterSkillForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 188);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.text);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Bonus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ability);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Skill);
            this.Controls.Add(this.label2);
            this.Name = "MonsterSkillForm";
            ((System.ComponentModel.ISupportInitialize)(this.Bonus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Skill;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Bonus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Ability;
        private System.Windows.Forms.Label label4;
    }
}