namespace Character_Builder_Builder
{
    partial class MagicForm
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
            System.Windows.Forms.Label label11;
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.Description = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Base = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PostName = new System.Windows.Forms.TextBox();
            this.PreName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Rarity = new System.Windows.Forms.ComboBox();
            this.Requirement = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Slot = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.source = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.preview = new System.Windows.Forms.WebBrowser();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.userControl11 = new Character_Builder_Builder.UserControl1();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.Attuned = new Character_Builder_Builder.Features();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.Carried = new Character_Builder_Builder.Features();
            this.label7 = new System.Windows.Forms.Label();
            this.OnUse = new Character_Builder_Builder.Features();
            this.label12 = new System.Windows.Forms.Label();
            this.Equipped = new Character_Builder_Builder.Features();
            this.label6 = new System.Windows.Forms.Label();
            this.AttunedOnUse = new Character_Builder_Builder.Features();
            this.label14 = new System.Windows.Forms.Label();
            this.AttunedEquipped = new Character_Builder_Builder.Features();
            this.label15 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.AutoEllipsis = true;
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 33);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(381, 13);
            label5.TabIndex = 24;
            label5.Text = "Requirement:";
            // 
            // label11
            // 
            label11.AutoEllipsis = true;
            label11.Dock = System.Windows.Forms.DockStyle.Top;
            label11.Location = new System.Drawing.Point(0, 66);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(381, 13);
            label11.TabIndex = 36;
            label11.Text = "Base Item Condition: (NCalc)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Controls.Add(this.abort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 672);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1003, 30);
            this.panel1.TabIndex = 0;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(841, 4);
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
            this.abort.Location = new System.Drawing.Point(922, 4);
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
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer1.Size = new System.Drawing.Size(753, 648);
            this.splitContainer1.SplitterDistance = 381;
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
            this.splitContainer2.Panel1.Controls.Add(this.Description);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.Base);
            this.splitContainer2.Panel1.Controls.Add(label11);
            this.splitContainer2.Panel1.Controls.Add(this.label9);
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            this.splitContainer2.Panel1.Controls.Add(this.Rarity);
            this.splitContainer2.Panel1.Controls.Add(this.Requirement);
            this.splitContainer2.Panel1.Controls.Add(label5);
            this.splitContainer2.Panel1.Controls.Add(this.label13);
            this.splitContainer2.Panel1.Controls.Add(this.Slot);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.source);
            this.splitContainer2.Panel1.Controls.Add(this.name);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(381, 648);
            this.splitContainer2.SplitterDistance = 367;
            this.splitContainer2.TabIndex = 0;
            // 
            // Description
            // 
            this.Description.AcceptsReturn = true;
            this.Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Description.Location = new System.Drawing.Point(0, 112);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(381, 121);
            this.Description.TabIndex = 39;
            this.Description.TextChanged += new System.EventHandler(this.Description_TextChanged_1);
            this.Description.Enter += new System.EventHandler(this.showPreview);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Description:";
            // 
            // Base
            // 
            this.Base.Dock = System.Windows.Forms.DockStyle.Top;
            this.Base.Location = new System.Drawing.Point(0, 79);
            this.Base.Name = "Base";
            this.Base.Size = new System.Drawing.Size(381, 20);
            this.Base.TabIndex = 37;
            this.Base.TextChanged += new System.EventHandler(this.Base_TextChanged);
            this.Base.Enter += new System.EventHandler(this.showPreviewMatching);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label9.Location = new System.Drawing.Point(0, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Name Modifikation:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PostName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.PreName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 246);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(381, 20);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // PostName
            // 
            this.PostName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostName.Location = new System.Drawing.Point(207, 0);
            this.PostName.Margin = new System.Windows.Forms.Padding(0);
            this.PostName.Name = "PostName";
            this.PostName.Size = new System.Drawing.Size(174, 20);
            this.PostName.TabIndex = 29;
            this.PostName.TextChanged += new System.EventHandler(this.PostName_TextChanged);
            this.PostName.Enter += new System.EventHandler(this.showPreview);
            // 
            // PreName
            // 
            this.PreName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreName.Location = new System.Drawing.Point(0, 0);
            this.PreName.Margin = new System.Windows.Forms.Padding(0);
            this.PreName.Name = "PreName";
            this.PreName.Size = new System.Drawing.Size(174, 20);
            this.PreName.TabIndex = 27;
            this.PreName.TextChanged += new System.EventHandler(this.PreName_TextChanged);
            this.PreName.Enter += new System.EventHandler(this.showPreview);
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(177, 0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.label10.Size = new System.Drawing.Size(27, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "Item Name";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(0, 266);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Rarity:";
            // 
            // Rarity
            // 
            this.Rarity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Rarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Rarity.FormattingEnabled = true;
            this.Rarity.Location = new System.Drawing.Point(0, 279);
            this.Rarity.Name = "Rarity";
            this.Rarity.Size = new System.Drawing.Size(381, 21);
            this.Rarity.TabIndex = 28;
            this.Rarity.SelectedIndexChanged += new System.EventHandler(this.Rarity_SelectedIndexChanged);
            this.Rarity.Enter += new System.EventHandler(this.showPreview);
            // 
            // Requirement
            // 
            this.Requirement.Dock = System.Windows.Forms.DockStyle.Top;
            this.Requirement.Location = new System.Drawing.Point(0, 46);
            this.Requirement.Name = "Requirement";
            this.Requirement.Size = new System.Drawing.Size(381, 20);
            this.Requirement.TabIndex = 25;
            this.Requirement.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.Requirement.Enter += new System.EventHandler(this.showPreview);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label13.Location = new System.Drawing.Point(0, 300);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Slot: (not used for now)";
            // 
            // Slot
            // 
            this.Slot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Slot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Slot.FormattingEnabled = true;
            this.Slot.Location = new System.Drawing.Point(0, 313);
            this.Slot.Name = "Slot";
            this.Slot.Size = new System.Drawing.Size(381, 21);
            this.Slot.TabIndex = 21;
            this.Slot.SelectedIndexChanged += new System.EventHandler(this.ParentClass_SelectedIndexChanged);
            this.Slot.TextChanged += new System.EventHandler(this.ParentClass_SelectedIndexChanged);
            this.Slot.Click += new System.EventHandler(this.showPreview);
            this.Slot.Enter += new System.EventHandler(this.showPreview);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(0, 334);
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
            this.source.Location = new System.Drawing.Point(0, 347);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(381, 20);
            this.source.TabIndex = 7;
            this.source.TextChanged += new System.EventHandler(this.source_TextChanged);
            this.source.Enter += new System.EventHandler(this.showPreview);
            // 
            // name
            // 
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.Location = new System.Drawing.Point(0, 13);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(381, 20);
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
            // preview
            // 
            this.preview.CausesValidation = false;
            this.preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.preview.Location = new System.Drawing.Point(754, 24);
            this.preview.MinimumSize = new System.Drawing.Size(20, 20);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(250, 648);
            this.preview.TabIndex = 10;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer5.Size = new System.Drawing.Size(368, 648);
            this.splitContainer5.SplitterDistance = 309;
            this.splitContainer5.TabIndex = 1;
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
            this.splitContainer6.Panel1.Controls.Add(this.Equipped);
            this.splitContainer6.Panel1.Controls.Add(this.label6);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.AttunedEquipped);
            this.splitContainer6.Panel2.Controls.Add(this.label15);
            this.splitContainer6.Size = new System.Drawing.Size(368, 335);
            this.splitContainer6.SplitterDistance = 158;
            this.splitContainer6.TabIndex = 0;
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl11.Editor = null;
            this.userControl11.Location = new System.Drawing.Point(1, 0);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1003, 24);
            this.userControl11.TabIndex = 9;
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
            this.splitContainer3.Panel1.Controls.Add(this.Carried);
            this.splitContainer3.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.Attuned);
            this.splitContainer3.Panel2.Controls.Add(this.label4);
            this.splitContainer3.Size = new System.Drawing.Size(368, 309);
            this.splitContainer3.SplitterDistance = 159;
            this.splitContainer3.TabIndex = 0;
            // 
            // Attuned
            // 
            this.Attuned.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Attuned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Attuned.features = null;
            this.Attuned.HistoryManager = null;
            this.Attuned.Location = new System.Drawing.Point(0, 13);
            this.Attuned.Name = "Attuned";
            this.Attuned.preview = this.preview;
            this.Attuned.Size = new System.Drawing.Size(368, 133);
            this.Attuned.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Features: (Attuned)";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.OnUse);
            this.splitContainer4.Panel1.Controls.Add(this.label12);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.AttunedOnUse);
            this.splitContainer4.Panel2.Controls.Add(this.label14);
            this.splitContainer4.Size = new System.Drawing.Size(381, 277);
            this.splitContainer4.SplitterDistance = 138;
            this.splitContainer4.TabIndex = 0;
            // 
            // Carried
            // 
            this.Carried.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Carried.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Carried.features = null;
            this.Carried.HistoryManager = null;
            this.Carried.Location = new System.Drawing.Point(0, 13);
            this.Carried.Name = "Carried";
            this.Carried.preview = this.preview;
            this.Carried.Size = new System.Drawing.Size(368, 146);
            this.Carried.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Features: (Carried)";
            // 
            // OnUse
            // 
            this.OnUse.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.OnUse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OnUse.features = null;
            this.OnUse.HistoryManager = null;
            this.OnUse.Location = new System.Drawing.Point(0, 13);
            this.OnUse.Name = "OnUse";
            this.OnUse.preview = this.preview;
            this.OnUse.Size = new System.Drawing.Size(381, 125);
            this.OnUse.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Features: (on Use)";
            // 
            // Equipped
            // 
            this.Equipped.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Equipped.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Equipped.features = null;
            this.Equipped.HistoryManager = null;
            this.Equipped.Location = new System.Drawing.Point(0, 13);
            this.Equipped.Name = "Equipped";
            this.Equipped.preview = this.preview;
            this.Equipped.Size = new System.Drawing.Size(368, 145);
            this.Equipped.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Features: (Euipped)";
            // 
            // AttunedOnUse
            // 
            this.AttunedOnUse.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.AttunedOnUse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttunedOnUse.features = null;
            this.AttunedOnUse.HistoryManager = null;
            this.AttunedOnUse.Location = new System.Drawing.Point(0, 13);
            this.AttunedOnUse.Name = "AttunedOnUse";
            this.AttunedOnUse.preview = this.preview;
            this.AttunedOnUse.Size = new System.Drawing.Size(381, 122);
            this.AttunedOnUse.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Top;
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(155, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Features: (Attuned and on Use)";
            // 
            // AttunedEquipped
            // 
            this.AttunedEquipped.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.AttunedEquipped.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttunedEquipped.features = null;
            this.AttunedEquipped.HistoryManager = null;
            this.AttunedEquipped.Location = new System.Drawing.Point(0, 13);
            this.AttunedEquipped.Name = "AttunedEquipped";
            this.AttunedEquipped.preview = this.preview;
            this.AttunedEquipped.Size = new System.Drawing.Size(368, 160);
            this.AttunedEquipped.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Top;
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(160, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Features: (Attuned and Euipped)";
            // 
            // MagicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1005, 702);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.preview);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.panel1);
            this.Name = "MagicForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Magic Property";
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox Requirement;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox PostName;
        private System.Windows.Forms.TextBox PreName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox Rarity;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox Slot;
        private System.Windows.Forms.TextBox Description;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Base;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Features OnUse;
        private System.Windows.Forms.Label label12;
        private Features AttunedOnUse;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Features Carried;
        private System.Windows.Forms.Label label7;
        private Features Attuned;
        private System.Windows.Forms.Label label4;
        private Features Equipped;
        private System.Windows.Forms.Label label6;
        private Features AttunedEquipped;
        private System.Windows.Forms.Label label15;
    }
}