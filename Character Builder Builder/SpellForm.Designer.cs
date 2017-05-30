namespace Character_Builder_Builder
{
    partial class SpellForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.description = new System.Windows.Forms.TextBox();
            this.decriptions1 = new Character_Builder_Builder.Decriptions();
            this.source = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Duration = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Range = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CastingTime = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Level = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.keywordControl1 = new Character_Builder_Builder.KeywordControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControl11 = new Character_Builder_Builder.UserControl1();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.abort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 532);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1025, 30);
            this.panel1.TabIndex = 0;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(863, 4);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 1;
            this.save.Text = "Save && Exit";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // abort
            // 
            this.abort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abort.Location = new System.Drawing.Point(944, 4);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(78, 23);
            this.abort.TabIndex = 0;
            this.abort.Text = "Exit";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // preview
            // 
            this.preview.CausesValidation = false;
            this.preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.preview.Location = new System.Drawing.Point(776, 24);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(250, 508);
            this.preview.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(1, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.source);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.Duration);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.Range);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.CastingTime);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.Level);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.name);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(775, 508);
            this.splitContainer1.SplitterDistance = 385;
            this.splitContainer1.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 307);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Descriptions";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 16);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.description);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.decriptions1);
            this.splitContainer3.Size = new System.Drawing.Size(379, 288);
            this.splitContainer3.SplitterDistance = 140;
            this.splitContainer3.TabIndex = 0;
            // 
            // description
            // 
            this.description.AcceptsReturn = true;
            this.description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description.Location = new System.Drawing.Point(0, 0);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(379, 140);
            this.description.TabIndex = 0;
            this.description.TextChanged += new System.EventHandler(this.description_TextChanged);
            this.description.Enter += new System.EventHandler(this.showPreview);
            // 
            // decriptions1
            // 
            this.decriptions1.descriptions = null;
            this.decriptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.decriptions1.HistoryManager = null;
            this.decriptions1.Location = new System.Drawing.Point(0, 0);
            this.decriptions1.Name = "decriptions1";
            this.decriptions1.preview = this.preview;
            this.decriptions1.Size = new System.Drawing.Size(379, 144);
            this.decriptions1.TabIndex = 0;
            // 
            // source
            // 
            this.source.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.source.Dock = System.Windows.Forms.DockStyle.Top;
            this.source.Location = new System.Drawing.Point(0, 181);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(385, 20);
            this.source.TabIndex = 41;
            this.source.TextChanged += new System.EventHandler(this.source_TextChanged);
            this.source.Enter += new System.EventHandler(this.showPreview);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Source:";
            // 
            // Duration
            // 
            this.Duration.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Duration.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Duration.Dock = System.Windows.Forms.DockStyle.Top;
            this.Duration.FormattingEnabled = true;
            this.Duration.Location = new System.Drawing.Point(0, 147);
            this.Duration.Name = "Duration";
            this.Duration.Size = new System.Drawing.Size(385, 21);
            this.Duration.TabIndex = 38;
            this.Duration.TextChanged += new System.EventHandler(this.Duration_TextChanged);
            this.Duration.Enter += new System.EventHandler(this.showPreview);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "Duration:";
            // 
            // Range
            // 
            this.Range.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Range.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Range.Dock = System.Windows.Forms.DockStyle.Top;
            this.Range.FormattingEnabled = true;
            this.Range.Location = new System.Drawing.Point(0, 113);
            this.Range.Name = "Range";
            this.Range.Size = new System.Drawing.Size(385, 21);
            this.Range.TabIndex = 35;
            this.Range.TextChanged += new System.EventHandler(this.Range_TextChanged);
            this.Range.Enter += new System.EventHandler(this.showPreview);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Range:";
            // 
            // CastingTime
            // 
            this.CastingTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CastingTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.CastingTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.CastingTime.FormattingEnabled = true;
            this.CastingTime.Location = new System.Drawing.Point(0, 79);
            this.CastingTime.Name = "CastingTime";
            this.CastingTime.Size = new System.Drawing.Size(385, 21);
            this.CastingTime.TabIndex = 29;
            this.CastingTime.TextChanged += new System.EventHandler(this.CastingTime_TextChanged);
            this.CastingTime.Enter += new System.EventHandler(this.showPreview);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Casting Time:";
            // 
            // Level
            // 
            this.Level.Dock = System.Windows.Forms.DockStyle.Top;
            this.Level.Location = new System.Drawing.Point(0, 46);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(385, 20);
            this.Level.TabIndex = 27;
            this.Level.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            this.Level.Enter += new System.EventHandler(this.showPreview);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Level:";
            // 
            // name
            // 
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point(0, 13);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(385, 20);
            this.name.TabIndex = 17;
            this.name.TextChanged += new System.EventHandler(this.name_TextChanged);
            this.name.Enter += new System.EventHandler(this.showPreview);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Name:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(386, 508);
            this.splitContainer2.SplitterDistance = 243;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.keywordControl1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(386, 243);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Keywords";
            // 
            // keywordControl1
            // 
            this.keywordControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.keywordControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywordControl1.Group = Character_Builder_Builder.KeywordControl.KeywordGroup.SPELL;
            this.keywordControl1.HistoryManager = null;
            this.keywordControl1.Keywords = null;
            this.keywordControl1.Location = new System.Drawing.Point(3, 16);
            this.keywordControl1.Name = "keywordControl1";
            this.keywordControl1.Size = new System.Drawing.Size(380, 224);
            this.keywordControl1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 261);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Damage (Cantrips)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(380, 242);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserAddedRow);
            this.dataGridView1.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserAddedRow);
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl11.Editor = null;
            this.userControl11.Location = new System.Drawing.Point(1, 0);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1025, 24);
            this.userControl11.TabIndex = 9;
            // 
            // SpellForm
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.abort;
            this.ClientSize = new System.Drawing.Size(1027, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel1);
            this.Name = "SpellForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Spell";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RaceForm_FormClosing);
            this.Shown += new System.EventHandler(this.SpellForm_Shown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Level)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button abort;
        private UserControl1 userControl11;
        private System.Windows.Forms.WebBrowser preview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox description;
        private Decriptions decriptions1;
        private System.Windows.Forms.TextBox source;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Duration;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Range;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CastingTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Level;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private KeywordControl keywordControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}