namespace Character_Builder_Builder.ItemForms
{
    partial class BasicItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Description = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.StackSize = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.Weight = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.Unit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SingleUnit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Source = new System.Windows.Forms.TextBox();
            this.ItemName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.KeywordControl = new Character_Builder_Builder.KeywordControl();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.CP = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.SP = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.EP = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.GP = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.PP = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StackSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Weight)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PP)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Description);
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.StackSize);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.Weight);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.Unit);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.SingleUnit);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.Source);
            this.splitContainer1.Panel1.Controls.Add(this.ItemName);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.KeywordControl);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(797, 507);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 0;
            // 
            // Description
            // 
            this.Description.AcceptsReturn = true;
            this.Description.AcceptsTab = true;
            this.Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Description.Location = new System.Drawing.Point(0, 46);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(352, 296);
            this.Description.TabIndex = 13;
            this.Description.TextChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.Description.Enter += new System.EventHandler(this.showPreview);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.Location = new System.Drawing.Point(0, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Description:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label12.Location = new System.Drawing.Point(0, 342);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(182, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Size of Stack in Shop and Free Items";
            // 
            // StackSize
            // 
            this.StackSize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StackSize.Location = new System.Drawing.Point(0, 355);
            this.StackSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.StackSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StackSize.Name = "StackSize";
            this.StackSize.Size = new System.Drawing.Size(352, 20);
            this.StackSize.TabIndex = 10;
            this.StackSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StackSize.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.StackSize.Enter += new System.EventHandler(this.showPreview);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label11.Location = new System.Drawing.Point(0, 375);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Weight of a Single Item:";
            // 
            // Weight
            // 
            this.Weight.DecimalPlaces = 5;
            this.Weight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Weight.Location = new System.Drawing.Point(0, 388);
            this.Weight.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.Weight.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.Weight.Name = "Weight";
            this.Weight.Size = new System.Drawing.Size(352, 20);
            this.Weight.TabIndex = 8;
            this.Weight.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.Weight.Enter += new System.EventHandler(this.showPreview);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label10.Location = new System.Drawing.Point(0, 408);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(203, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Unit of Multiple Items: (pieces, vials, fl.oz.)";
            // 
            // Unit
            // 
            this.Unit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Unit.Location = new System.Drawing.Point(0, 421);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(352, 20);
            this.Unit.TabIndex = 6;
            this.Unit.TextChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.Unit.Enter += new System.EventHandler(this.showPreview);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label9.Location = new System.Drawing.Point(0, 441);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(196, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Unit of a Single Item: (piece, vial, fl. oz.):";
            // 
            // SingleUnit
            // 
            this.SingleUnit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SingleUnit.Location = new System.Drawing.Point(0, 454);
            this.SingleUnit.Name = "SingleUnit";
            this.SingleUnit.Size = new System.Drawing.Size(352, 20);
            this.SingleUnit.TabIndex = 4;
            this.SingleUnit.TextChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.SingleUnit.Enter += new System.EventHandler(this.showPreview);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(0, 474);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Source:";
            // 
            // Source
            // 
            this.Source.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Source.Location = new System.Drawing.Point(0, 487);
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(352, 20);
            this.Source.TabIndex = 2;
            this.Source.TextChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.Source.Enter += new System.EventHandler(this.showPreview);
            // 
            // ItemName
            // 
            this.ItemName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ItemName.Location = new System.Drawing.Point(0, 13);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(352, 20);
            this.ItemName.TabIndex = 1;
            this.ItemName.TextChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.ItemName.Enter += new System.EventHandler(this.showPreview);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Name:";
            // 
            // KeywordControl
            // 
            this.KeywordControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.KeywordControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KeywordControl.Group = Character_Builder_Builder.KeywordControl.KeywordGroup.ITEM;
            this.KeywordControl.HistoryManager = null;
            this.KeywordControl.Keywords = null;
            this.KeywordControl.Location = new System.Drawing.Point(0, 13);
            this.KeywordControl.Name = "KeywordControl";
            this.KeywordControl.Size = new System.Drawing.Size(441, 380);
            this.KeywordControl.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Keywords:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 393);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 114);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.CP, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SP, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.EP, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.GP, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PP, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(435, 95);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 19);
            this.label5.TabIndex = 8;
            this.label5.Text = "CP:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CP
            // 
            this.CP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CP.Location = new System.Drawing.Point(36, 79);
            this.CP.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.CP.Name = "CP";
            this.CP.Size = new System.Drawing.Size(396, 20);
            this.CP.TabIndex = 9;
            this.CP.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.CP.Enter += new System.EventHandler(this.showPreview);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "SP:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SP
            // 
            this.SP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SP.Location = new System.Drawing.Point(36, 60);
            this.SP.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.SP.Name = "SP";
            this.SP.Size = new System.Drawing.Size(396, 20);
            this.SP.TabIndex = 7;
            this.SP.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.SP.Enter += new System.EventHandler(this.showPreview);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "EP:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EP
            // 
            this.EP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EP.Location = new System.Drawing.Point(36, 41);
            this.EP.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.EP.Name = "EP";
            this.EP.Size = new System.Drawing.Size(396, 20);
            this.EP.TabIndex = 5;
            this.EP.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.EP.Enter += new System.EventHandler(this.showPreview);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "GP:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GP
            // 
            this.GP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GP.Location = new System.Drawing.Point(36, 22);
            this.GP.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.GP.Name = "GP";
            this.GP.Size = new System.Drawing.Size(396, 20);
            this.GP.TabIndex = 3;
            this.GP.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.GP.Enter += new System.EventHandler(this.showPreview);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "PP:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PP
            // 
            this.PP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PP.Location = new System.Drawing.Point(36, 3);
            this.PP.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.PP.Name = "PP";
            this.PP.Size = new System.Drawing.Size(396, 20);
            this.PP.TabIndex = 1;
            this.PP.ValueChanged += new System.EventHandler(this.ItemName_TextChanged);
            this.PP.Enter += new System.EventHandler(this.showPreview);
            // 
            // BasicItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BasicItem";
            this.Size = new System.Drawing.Size(797, 507);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StackSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Weight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown Weight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Unit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SingleUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Source;
        private System.Windows.Forms.TextBox ItemName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown CP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown SP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown EP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown GP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PP;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown StackSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox Description;
        private System.Windows.Forms.Label label13;
        public KeywordControl KeywordControl;
    }
}
