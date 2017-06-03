namespace Character_Builder_Builder.FeatureForms
{
    partial class AbilityScoreFeatureForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Cha = new System.Windows.Forms.NumericUpDown();
            this.Int = new System.Windows.Forms.NumericUpDown();
            this.Con = new System.Windows.Forms.NumericUpDown();
            this.Wis = new System.Windows.Forms.NumericUpDown();
            this.Dex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.ComboBox();
            this.Str = new System.Windows.Forms.NumericUpDown();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Int)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Con)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Str)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 294);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(703, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.Cha, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.Int, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.Con, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Wis, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.Dex, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Mode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Str, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 241);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(703, 53);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Cha
            // 
            this.Cha.Location = new System.Drawing.Point(582, 29);
            this.Cha.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Cha.Name = "Cha";
            this.Cha.Size = new System.Drawing.Size(115, 20);
            this.Cha.TabIndex = 13;
            // 
            // Int
            // 
            this.Int.Location = new System.Drawing.Point(389, 29);
            this.Int.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Int.Name = "Int";
            this.Int.Size = new System.Drawing.Size(115, 20);
            this.Int.TabIndex = 12;
            // 
            // Con
            // 
            this.Con.Location = new System.Drawing.Point(196, 29);
            this.Con.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Con.Name = "Con";
            this.Con.Size = new System.Drawing.Size(115, 20);
            this.Con.TabIndex = 11;
            // 
            // Wis
            // 
            this.Wis.Location = new System.Drawing.Point(582, 3);
            this.Wis.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Wis.Name = "Wis";
            this.Wis.Size = new System.Drawing.Size(115, 20);
            this.Wis.TabIndex = 10;
            // 
            // Dex
            // 
            this.Dex.Location = new System.Drawing.Point(389, 3);
            this.Dex.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Dex.Name = "Dex";
            this.Dex.Size = new System.Drawing.Size(115, 20);
            this.Dex.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(124, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Strength:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(124, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "Constitution:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(317, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Dexterity:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(317, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 27);
            this.label4.TabIndex = 3;
            this.label4.Text = "Intelligence:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(510, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 26);
            this.label5.TabIndex = 4;
            this.label5.Text = "Wisdom:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(510, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 27);
            this.label6.TabIndex = 5;
            this.label6.Text = "Charisma:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 26);
            this.label7.TabIndex = 6;
            this.label7.Text = "Mode:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Mode
            // 
            this.Mode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Mode.FormattingEnabled = true;
            this.Mode.Items.AddRange(new object[] {
            "Add to Scores",
            "Set Scores",
            "Add to Maximum",
            "Set Maximum",
            "Bonus to Scores (Add)",
            "Bonus to Scores (Set)",
            "Bonus to Maximum (Add)",
            "Bonus to Maximum (Set)"});
            this.Mode.Location = new System.Drawing.Point(3, 29);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(115, 21);
            this.Mode.TabIndex = 7;
            this.Mode.SelectedIndexChanged += new System.EventHandler(this.Mode_SelectedIndexChanged);
            // 
            // Str
            // 
            this.Str.Location = new System.Drawing.Point(196, 3);
            this.Str.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.Str.Name = "Str";
            this.Str.Size = new System.Drawing.Size(115, 20);
            this.Str.TabIndex = 8;
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(703, 239);
            this.basicFeature1.TabIndex = 4;
            // 
            // AbilityScoreFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(707, 321);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Name = "AbilityScoreFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ability Score Feature";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Cha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Int)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Con)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Str)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown Cha;
        private System.Windows.Forms.NumericUpDown Int;
        private System.Windows.Forms.NumericUpDown Con;
        private System.Windows.Forms.NumericUpDown Wis;
        private System.Windows.Forms.NumericUpDown Dex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Mode;
        private System.Windows.Forms.NumericUpDown Str;
        private BasicFeature basicFeature1;
    }
}