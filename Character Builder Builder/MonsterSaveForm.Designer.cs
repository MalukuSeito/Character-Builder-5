namespace Character_Builder_Builder
{
    partial class MonsterSaveForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.Ability = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Bonus = new System.Windows.Forms.NumericUpDown();
            this.text = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Bonus)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(284, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Ability:";
            // 
            // Ability
            // 
            this.Ability.Dock = System.Windows.Forms.DockStyle.Top;
            this.Ability.FormattingEnabled = true;
            this.Ability.Location = new System.Drawing.Point(0, 15);
            this.Ability.Name = "Ability";
            this.Ability.Size = new System.Drawing.Size(284, 21);
            this.Ability.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 15);
            this.label1.TabIndex = 25;
            this.label1.Text = "Bonus:";
            // 
            // Bonus
            // 
            this.Bonus.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bonus.Location = new System.Drawing.Point(0, 51);
            this.Bonus.Name = "Bonus";
            this.Bonus.Size = new System.Drawing.Size(284, 20);
            this.Bonus.TabIndex = 26;
            // 
            // text
            // 
            this.text.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.text.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.text.Dock = System.Windows.Forms.DockStyle.Top;
            this.text.Location = new System.Drawing.Point(0, 89);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(284, 20);
            this.text.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(284, 18);
            this.label5.TabIndex = 27;
            this.label5.Text = "Text (optional):";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(0, 109);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(2);
            this.button1.Size = new System.Drawing.Size(284, 30);
            this.button1.TabIndex = 29;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MonsterSaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(284, 139);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.text);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Bonus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ability);
            this.Controls.Add(this.label4);
            this.Name = "MonsterSaveForm";
            ((System.ComponentModel.ISupportInitialize)(this.Bonus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Ability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Bonus;
        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}