namespace HotBabe.Controls
{
    partial class ImageInfoEditor
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
          this.components = new System.ComponentModel.Container();
          this.label1 = new System.Windows.Forms.Label();
          this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
          this.tbImage = new System.Windows.Forms.TextBox();
          this.btnBrowseImage = new System.Windows.Forms.Button();
          this.label2 = new System.Windows.Forms.Label();
          this.tbMeasures = new System.Windows.Forms.TextBox();
          this.btnDelete = new System.Windows.Forms.Button();
          this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
          this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
          ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
          this.SuspendLayout();
          // 
          // label1
          // 
          this.label1.Location = new System.Drawing.Point(3, 0);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(46, 23);
          this.label1.TabIndex = 0;
          this.label1.Text = "Image:";
          this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // openFileDialog1
          // 
          this.openFileDialog1.FileName = "openFileDialog1";
          // 
          // tbImage
          // 
          this.tbImage.Location = new System.Drawing.Point(74, 3);
          this.tbImage.Name = "tbImage";
          this.tbImage.Size = new System.Drawing.Size(251, 20);
          this.tbImage.TabIndex = 1;
          this.tbImage.TextChanged += new System.EventHandler(this.tbImage_TextChanged);
          this.tbImage.MouseLeave += new System.EventHandler(this.tbImage_MouseLeave);
          this.tbImage.MouseEnter += new System.EventHandler(this.tbImage_MouseEnter);
          // 
          // btnBrowseImage
          // 
          this.btnBrowseImage.Location = new System.Drawing.Point(331, 3);
          this.btnBrowseImage.Name = "btnBrowseImage";
          this.btnBrowseImage.Size = new System.Drawing.Size(24, 23);
          this.btnBrowseImage.TabIndex = 2;
          this.btnBrowseImage.Text = "...";
          this.toolTip1.SetToolTip(this.btnBrowseImage, "Browse for image");
          this.btnBrowseImage.UseVisualStyleBackColor = true;
          this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
          // 
          // label2
          // 
          this.label2.Location = new System.Drawing.Point(3, 26);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(65, 23);
          this.label2.TabIndex = 3;
          this.label2.Text = "Measures: ";
          this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // tbMeasures
          // 
          this.tbMeasures.Location = new System.Drawing.Point(74, 29);
          this.tbMeasures.Name = "tbMeasures";
          this.tbMeasures.Size = new System.Drawing.Size(251, 20);
          this.tbMeasures.TabIndex = 4;
          this.tbMeasures.TextChanged += new System.EventHandler(this.tbMeasures_TextChanged);
          // 
          // btnDelete
          // 
          this.btnDelete.Location = new System.Drawing.Point(331, 26);
          this.btnDelete.Name = "btnDelete";
          this.btnDelete.Size = new System.Drawing.Size(24, 23);
          this.btnDelete.TabIndex = 5;
          this.btnDelete.Text = "X";
          this.toolTip1.SetToolTip(this.btnDelete, "Delete image");
          this.btnDelete.UseVisualStyleBackColor = true;
          this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
          // 
          // errorProvider1
          // 
          this.errorProvider1.ContainerControl = this;
          // 
          // ImageInfoEditor
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.AutoSize = true;
          this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
          this.Controls.Add(this.btnDelete);
          this.Controls.Add(this.tbMeasures);
          this.Controls.Add(this.label2);
          this.Controls.Add(this.btnBrowseImage);
          this.Controls.Add(this.tbImage);
          this.Controls.Add(this.label1);
          this.Name = "ImageInfoEditor";
          this.Size = new System.Drawing.Size(358, 52);
          ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbImage;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMeasures;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}