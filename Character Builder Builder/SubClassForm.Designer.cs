namespace Character_Builder_Builder
{
    partial class SubClassForm
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
            System.Windows.Forms.Label label5;
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.flavour = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.imageChooser1 = new Character_Builder_Builder.ImageChooser();
            this.SheetName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ParentClass = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.source = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.description = new System.Windows.Forms.TextBox();
            this.decriptions1 = new Character_Builder_Builder.Decriptions();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.MulticlassSpellLevels = new Character_Builder_Builder.IntList();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.features1 = new Character_Builder_Builder.Features();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.featuresFirstClass = new Character_Builder_Builder.Features();
            this.label6 = new System.Windows.Forms.Label();
            this.featuresMultiClass = new Character_Builder_Builder.Features();
            this.label7 = new System.Windows.Forms.Label();
            this.userControl11 = new Character_Builder_Builder.UserControl1();
            label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.AutoEllipsis = true;
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 33);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(321, 13);
            label5.TabIndex = 24;
            label5.Text = "Full Classname: (shown on sheet, should include parent class)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.abort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 647);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 30);
            this.panel1.TabIndex = 0;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(856, 4);
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
            this.abort.Location = new System.Drawing.Point(937, 4);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(78, 23);
            this.abort.TabIndex = 0;
            this.abort.Text = "Exit";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(1, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(768, 623);
            this.splitContainer1.SplitterDistance = 321;
            this.splitContainer1.TabIndex = 11;
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            this.splitContainer2.Panel1.Controls.Add(this.SheetName);
            this.splitContainer2.Panel1.Controls.Add(label5);
            this.splitContainer2.Panel1.Controls.Add(this.label13);
            this.splitContainer2.Panel1.Controls.Add(this.ParentClass);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.source);
            this.splitContainer2.Panel1.Controls.Add(this.name);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(321, 623);
            this.splitContainer2.SplitterDistance = 223;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 66);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.flavour);
            this.splitContainer4.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.imageChooser1);
            this.splitContainer4.Size = new System.Drawing.Size(321, 90);
            this.splitContainer4.SplitterDistance = 184;
            this.splitContainer4.TabIndex = 26;
            // 
            // flavour
            // 
            this.flavour.AcceptsReturn = true;
            this.flavour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flavour.Location = new System.Drawing.Point(0, 13);
            this.flavour.Multiline = true;
            this.flavour.Name = "flavour";
            this.flavour.Size = new System.Drawing.Size(184, 77);
            this.flavour.TabIndex = 29;
            this.flavour.TextChanged += new System.EventHandler(this.flavour_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Flavour Text:";
            // 
            // imageChooser1
            // 
            this.imageChooser1.AllowDrop = true;
            this.imageChooser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageChooser1.History = null;
            this.imageChooser1.Image = null;
            this.imageChooser1.Location = new System.Drawing.Point(0, 0);
            this.imageChooser1.Name = "imageChooser1";
            this.imageChooser1.Size = new System.Drawing.Size(133, 90);
            this.imageChooser1.TabIndex = 0;
            // 
            // SheetName
            // 
            this.SheetName.Dock = System.Windows.Forms.DockStyle.Top;
            this.SheetName.Location = new System.Drawing.Point(0, 46);
            this.SheetName.Name = "SheetName";
            this.SheetName.Size = new System.Drawing.Size(321, 20);
            this.SheetName.TabIndex = 25;
            this.SheetName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.SheetName.Enter += new System.EventHandler(this.showPreview);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label13.Location = new System.Drawing.Point(0, 156);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Parent Class";
            // 
            // ParentClass
            // 
            this.ParentClass.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ParentClass.FormattingEnabled = true;
            this.ParentClass.Location = new System.Drawing.Point(0, 169);
            this.ParentClass.Name = "ParentClass";
            this.ParentClass.Size = new System.Drawing.Size(321, 21);
            this.ParentClass.TabIndex = 21;
            this.ParentClass.SelectedIndexChanged += new System.EventHandler(this.ParentClass_SelectedIndexChanged);
            this.ParentClass.TextChanged += new System.EventHandler(this.ParentClass_SelectedIndexChanged);
            this.ParentClass.Click += new System.EventHandler(this.showPreview);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(0, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Source:";
            // 
            // source
            // 
            this.source.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.source.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.source.Location = new System.Drawing.Point(0, 203);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(321, 20);
            this.source.TabIndex = 7;
            this.source.TextChanged += new System.EventHandler(this.source_TextChanged);
            this.source.Enter += new System.EventHandler(this.showPreview);
            // 
            // name
            // 
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point(0, 13);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(321, 20);
            this.name.TabIndex = 1;
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
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.MulticlassSpellLevels);
            this.splitContainer3.Panel2.Controls.Add(this.label8);
            this.splitContainer3.Size = new System.Drawing.Size(321, 396);
            this.splitContainer3.SplitterDistance = 216;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainer7);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 216);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Descriptions";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(3, 16);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.description);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.decriptions1);
            this.splitContainer7.Size = new System.Drawing.Size(315, 197);
            this.splitContainer7.SplitterDistance = 89;
            this.splitContainer7.TabIndex = 0;
            // 
            // description
            // 
            this.description.AcceptsReturn = true;
            this.description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description.Location = new System.Drawing.Point(0, 0);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(315, 89);
            this.description.TabIndex = 0;
            // 
            // decriptions1
            // 
            this.decriptions1.descriptions = null;
            this.decriptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.decriptions1.HistoryManager = null;
            this.decriptions1.Location = new System.Drawing.Point(0, 0);
            this.decriptions1.Name = "decriptions1";
            this.decriptions1.preview = this.preview;
            this.decriptions1.Size = new System.Drawing.Size(315, 104);
            this.decriptions1.TabIndex = 0;
            // 
            // preview
            // 
            this.preview.CausesValidation = false;
            this.preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.preview.Location = new System.Drawing.Point(769, 24);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(250, 623);
            this.preview.TabIndex = 10;
            // 
            // MulticlassSpellLevels
            // 
            this.MulticlassSpellLevels.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.MulticlassSpellLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MulticlassSpellLevels.Format = "Level {0}: {1}";
            this.MulticlassSpellLevels.HistoryManager = null;
            this.MulticlassSpellLevels.Items = null;
            this.MulticlassSpellLevels.Location = new System.Drawing.Point(0, 13);
            this.MulticlassSpellLevels.Name = "MulticlassSpellLevels";
            this.MulticlassSpellLevels.Size = new System.Drawing.Size(321, 163);
            this.MulticlassSpellLevels.Start = 1;
            this.MulticlassSpellLevels.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(321, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Multiclassing Spell Levels (unless the parent class has some)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 623);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Features";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(3, 16);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.features1);
            this.splitContainer5.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer5.Size = new System.Drawing.Size(437, 604);
            this.splitContainer5.SplitterDistance = 263;
            this.splitContainer5.TabIndex = 0;
            // 
            // features1
            // 
            this.features1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.features1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.features1.features = null;
            this.features1.HistoryManager = null;
            this.features1.Location = new System.Drawing.Point(0, 13);
            this.features1.Name = "features1";
            this.features1.preview = this.preview;
            this.features1.Size = new System.Drawing.Size(437, 250);
            this.features1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(341, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Features: (regardless if this class is choosen first or as a multiclass later)";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.featuresFirstClass);
            this.splitContainer6.Panel1.Controls.Add(this.label6);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.featuresMultiClass);
            this.splitContainer6.Panel2.Controls.Add(this.label7);
            this.splitContainer6.Size = new System.Drawing.Size(437, 337);
            this.splitContainer6.SplitterDistance = 174;
            this.splitContainer6.TabIndex = 0;
            // 
            // featuresFirstClass
            // 
            this.featuresFirstClass.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.featuresFirstClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featuresFirstClass.features = null;
            this.featuresFirstClass.HistoryManager = null;
            this.featuresFirstClass.Location = new System.Drawing.Point(0, 13);
            this.featuresFirstClass.Name = "featuresFirstClass";
            this.featuresFirstClass.preview = this.preview;
            this.featuresFirstClass.Size = new System.Drawing.Size(437, 161);
            this.featuresFirstClass.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(224, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Features: (only if this is the first class choosen)";
            // 
            // featuresMultiClass
            // 
            this.featuresMultiClass.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.featuresMultiClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featuresMultiClass.features = null;
            this.featuresMultiClass.HistoryManager = null;
            this.featuresMultiClass.Location = new System.Drawing.Point(0, 13);
            this.featuresMultiClass.Name = "featuresMultiClass";
            this.featuresMultiClass.preview = this.preview;
            this.featuresMultiClass.Size = new System.Drawing.Size(437, 146);
            this.featuresMultiClass.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(275, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Features: (only if this class is chosen as a multiclass later)";
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl11.Editor = null;
            this.userControl11.Location = new System.Drawing.Point(1, 0);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1018, 24);
            this.userControl11.TabIndex = 9;
            // 
            // SubClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1020, 677);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel1);
            this.Name = "SubClassForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Subclass";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RaceForm_FormClosing);
            this.Shown += new System.EventHandler(this.RaceForm_Shown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel1.PerformLayout();
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button abort;
        private UserControl1 userControl11;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox source;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser preview;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ParentClass;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.TextBox description;
        private Decriptions decriptions1;
        private IntList MulticlassSpellLevels;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private Features features1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private Features featuresFirstClass;
        private System.Windows.Forms.Label label6;
        private Features featuresMultiClass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SheetName;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TextBox flavour;
        private System.Windows.Forms.Label label2;
        private ImageChooser imageChooser1;
    }
}