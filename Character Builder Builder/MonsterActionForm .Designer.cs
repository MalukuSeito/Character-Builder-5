namespace Character_Builder_Builder
{
    partial class MonsterActionForm
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
            this.name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Attack = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Damage = new System.Windows.Forms.TextBox();
            this.descText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Attack)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 343);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(455, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(455, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name/Header:";
            // 
            // name
            // 
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point(2, 25);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(455, 20);
            this.name.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(2, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(455, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Attack Bonus:";
            // 
            // Attack
            // 
            this.Attack.Dock = System.Windows.Forms.DockStyle.Top;
            this.Attack.Location = new System.Drawing.Point(2, 68);
            this.Attack.Name = "Attack";
            this.Attack.Size = new System.Drawing.Size(455, 20);
            this.Attack.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(2, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(455, 23);
            this.label4.TabIndex = 26;
            this.label4.Text = "Damage:";
            // 
            // Damage
            // 
            this.Damage.Dock = System.Windows.Forms.DockStyle.Top;
            this.Damage.Location = new System.Drawing.Point(2, 111);
            this.Damage.Name = "Damage";
            this.Damage.Size = new System.Drawing.Size(455, 20);
            this.Damage.TabIndex = 27;
            // 
            // descText
            // 
            this.descText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descText.Location = new System.Drawing.Point(2, 154);
            this.descText.Multiline = true;
            this.descText.Name = "descText";
            this.descText.Size = new System.Drawing.Size(455, 189);
            this.descText.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(2, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(455, 23);
            this.label2.TabIndex = 29;
            this.label2.Text = "Description:";
            // 
            // MonsterActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(459, 368);
            this.Controls.Add(this.descText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Damage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Attack);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "MonsterActionForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monster Action";
            ((System.ComponentModel.ISupportInitialize)(this.Attack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Attack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Damage;
        private System.Windows.Forms.TextBox descText;
        private System.Windows.Forms.Label label2;
    }
}