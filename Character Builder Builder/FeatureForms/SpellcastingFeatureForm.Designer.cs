namespace Character_Builder_Builder.FeatureForms
{
    partial class SpellcastingFeatureForm
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SpellcastingAbility = new System.Windows.Forms.CheckedListBox();
            this.label17 = new System.Windows.Forms.Label();
            this.DisplayName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SpellcastingID = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.label4 = new System.Windows.Forms.Label();
            this.Recharge = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PrepareMode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Amount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 504);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(803, 25);
            this.button1.TabIndex = 59;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer2.Location = new System.Drawing.Point(2, 245);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(803, 259);
            this.splitContainer2.SplitterDistance = 312;
            this.splitContainer2.TabIndex = 60;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SpellcastingAbility);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.DisplayName);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 259);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Spellcasting General Options";
            // 
            // SpellcastingAbility
            // 
            this.SpellcastingAbility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpellcastingAbility.FormattingEnabled = true;
            this.SpellcastingAbility.Location = new System.Drawing.Point(3, 95);
            this.SpellcastingAbility.Name = "SpellcastingAbility";
            this.SpellcastingAbility.Size = new System.Drawing.Size(306, 161);
            this.SpellcastingAbility.TabIndex = 18;
            this.SpellcastingAbility.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SpellcastingModifier_ItemCheck);
            // 
            // label17
            // 
            this.label17.AutoEllipsis = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.Location = new System.Drawing.Point(3, 82);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(306, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "Spellcasting Ability:";
            // 
            // DisplayName
            // 
            this.DisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.DisplayName.Location = new System.Drawing.Point(3, 62);
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.Size = new System.Drawing.Size(306, 20);
            this.DisplayName.TabIndex = 16;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Top;
            this.label16.Location = new System.Drawing.Point(3, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 13);
            this.label16.TabIndex = 15;
            this.label16.Text = "Displayed Name:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SpellcastingID);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 20);
            this.panel1.TabIndex = 8;
            // 
            // SpellcastingID
            // 
            this.SpellcastingID.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpellcastingID.Location = new System.Drawing.Point(0, 0);
            this.SpellcastingID.Name = "SpellcastingID";
            this.SpellcastingID.Size = new System.Drawing.Size(178, 20);
            this.SpellcastingID.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox1.Location = new System.Drawing.Point(178, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.checkBox1.Size = new System.Drawing.Size(128, 20);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Ignores Multiclassing";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Top;
            this.label15.Location = new System.Drawing.Point(3, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(260, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Spellcasting Internal ID: (other features reference this)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Recharge);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Condition);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.PrepareMode);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.Amount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spell Preparation Options";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Size = new System.Drawing.Size(803, 243);
            this.basicFeature1.TabIndex = 61;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 131;
            this.label4.Text = "Amount of Prepared Spells: (NCalc)";
            // 
            // Recharge
            // 
            this.Recharge.Dock = System.Windows.Forms.DockStyle.Top;
            this.Recharge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Recharge.FormattingEnabled = true;
            this.Recharge.Location = new System.Drawing.Point(3, 233);
            this.Recharge.Name = "Recharge";
            this.Recharge.Size = new System.Drawing.Size(481, 21);
            this.Recharge.TabIndex = 196;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 195;
            this.label3.Text = "Prepare Spells after: (Not used yet)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Location = new System.Drawing.Point(3, 207);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label10.Size = new System.Drawing.Size(183, 13);
            this.label10.TabIndex = 194;
            this.label10.Text = "ClasssLevel is 0 in that case.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 194);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(421, 13);
            this.label5.TabIndex = 193;
            this.label5.Text = "For Spellbooks, the condition is only checked, when a spell is bought at the stor" +
    "e.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 181);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(383, 13);
            this.label1.TabIndex = 192;
            this.label1.Text = "ClassLevel is the level of the class, ClassSpellLevel is (ClassLevel + 1) / 2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(3, 168);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(475, 13);
            this.label7.TabIndex = 191;
            this.label7.Text = "In addition: Name and Level (number) of the spells, as well as the spells keyword" +
    "s as boolean";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(3, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(430, 13);
            this.label6.TabIndex = 190;
            this.label6.Text = "Note: Only spells matching the condition can be prepared, the same inputs  as the" +
    " amount";
            // 
            // Condition
            // 
            this.Condition.Dock = System.Windows.Forms.DockStyle.Top;
            this.Condition.Location = new System.Drawing.Point(3, 135);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(481, 20);
            this.Condition.TabIndex = 189;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 13);
            this.label2.TabIndex = 188;
            this.label2.Text = "Condition for ClassList: (NCalc Expression)";
            // 
            // PrepareMode
            // 
            this.PrepareMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.PrepareMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrepareMode.FormattingEnabled = true;
            this.PrepareMode.Location = new System.Drawing.Point(3, 101);
            this.PrepareMode.Name = "PrepareMode";
            this.PrepareMode.Size = new System.Drawing.Size(481, 21);
            this.PrepareMode.TabIndex = 187;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(3, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 186;
            this.label11.Text = "Preparation Mode:";
            // 
            // label12
            // 
            this.label12.AutoEllipsis = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(3, 75);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label12.Size = new System.Drawing.Size(481, 13);
            this.label12.TabIndex = 185;
            this.label12.Text = "PlayerLevel (character level), ClassLevel (class level if in class, PlayerLevel o" +
    "therwise), ClassLevel(\"classname\") , function for classlevel";
            // 
            // label9
            // 
            this.label9.AutoEllipsis = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Location = new System.Drawing.Point(3, 62);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label9.Size = new System.Drawing.Size(481, 13);
            this.label9.TabIndex = 184;
            this.label9.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(3, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(481, 13);
            this.label8.TabIndex = 183;
            this.label8.Text = "Note: The Amount expression must result in a number:";
            // 
            // Amount
            // 
            this.Amount.Dock = System.Windows.Forms.DockStyle.Top;
            this.Amount.Location = new System.Drawing.Point(3, 29);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(481, 20);
            this.Amount.TabIndex = 182;
            // 
            // SpellcastingFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(807, 531);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.button1);
            this.Name = "SpellcastingFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Spellcasting Feature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpellcastingFeatureForm_FormClosing);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.CheckedListBox SpellcastingAbility;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox DisplayName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox SpellcastingID;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox Recharge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox PrepareMode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Amount;
        private System.Windows.Forms.Label label4;
    }
}