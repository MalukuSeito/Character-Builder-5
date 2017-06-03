namespace Character_Builder_Builder
{
    partial class ImageChooser
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.portraitBox = new System.Windows.Forms.PictureBox();
            this.removePortrait = new System.Windows.Forms.Button();
            this.choosePortrait = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portraitBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.portraitBox);
            this.groupBox1.Controls.Add(this.removePortrait);
            this.groupBox1.Controls.Add(this.choosePortrait);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // portraitBox
            // 
            this.portraitBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portraitBox.Location = new System.Drawing.Point(3, 16);
            this.portraitBox.Name = "portraitBox";
            this.portraitBox.Size = new System.Drawing.Size(144, 85);
            this.portraitBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.portraitBox.TabIndex = 9;
            this.portraitBox.TabStop = false;
            this.portraitBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.portraitBox_DragDrop);
            this.portraitBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.portraitBox_DragEnter);
            // 
            // removePortrait
            // 
            this.removePortrait.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.removePortrait.Location = new System.Drawing.Point(3, 101);
            this.removePortrait.Name = "removePortrait";
            this.removePortrait.Size = new System.Drawing.Size(144, 23);
            this.removePortrait.TabIndex = 8;
            this.removePortrait.Text = "Remove";
            this.removePortrait.UseVisualStyleBackColor = true;
            this.removePortrait.Click += new System.EventHandler(this.removePortrait_Click);
            // 
            // choosePortrait
            // 
            this.choosePortrait.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.choosePortrait.Location = new System.Drawing.Point(3, 124);
            this.choosePortrait.Name = "choosePortrait";
            this.choosePortrait.Size = new System.Drawing.Size(144, 23);
            this.choosePortrait.TabIndex = 7;
            this.choosePortrait.Text = "Choose";
            this.choosePortrait.UseVisualStyleBackColor = true;
            this.choosePortrait.Click += new System.EventHandler(this.choosePortrait_Click);
            // 
            // ImageChooser
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ImageChooser";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portraitBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox portraitBox;
        private System.Windows.Forms.Button removePortrait;
        private System.Windows.Forms.Button choosePortrait;
    }
}
