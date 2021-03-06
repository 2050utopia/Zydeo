﻿namespace ZD.Gui
{
    partial class ResultsCtxtControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsCtxtControl));
            this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tblSense = new System.Windows.Forms.TableLayoutPanel();
            this.lblSense = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblHead = new System.Windows.Forms.Label();
            this.pbCopyIcon = new System.Windows.Forms.PictureBox();
            this.tblFull = new System.Windows.Forms.TableLayoutPanel();
            this.lblFullCedict = new System.Windows.Forms.Label();
            this.lblFullFormatted = new System.Windows.Forms.Label();
            this.tblZho = new System.Windows.Forms.TableLayoutPanel();
            this.lblPinyin = new System.Windows.Forms.Label();
            this.lblHanzi2 = new System.Windows.Forms.Label();
            this.lblHanzi1 = new System.Windows.Forms.Label();
            this.tblLayout.SuspendLayout();
            this.tblSense.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyIcon)).BeginInit();
            this.tblFull.SuspendLayout();
            this.tblZho.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblLayout
            // 
            this.tblLayout.BackColor = System.Drawing.Color.LightGray;
            this.tblLayout.ColumnCount = 1;
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayout.Controls.Add(this.tblSense, 0, 3);
            this.tblLayout.Controls.Add(this.pnlTop, 0, 0);
            this.tblLayout.Controls.Add(this.tblFull, 0, 1);
            this.tblLayout.Controls.Add(this.tblZho, 0, 2);
            this.tblLayout.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayout.Location = new System.Drawing.Point(1, 1);
            this.tblLayout.Margin = new System.Windows.Forms.Padding(0);
            this.tblLayout.Name = "tblLayout";
            this.tblLayout.RowCount = 4;
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayout.Size = new System.Drawing.Size(400, 186);
            this.tblLayout.TabIndex = 0;
            // 
            // tblSense
            // 
            this.tblSense.BackColor = System.Drawing.Color.White;
            this.tblSense.ColumnCount = 1;
            this.tblSense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSense.Controls.Add(this.lblSense, 0, 1);
            this.tblSense.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblSense.Location = new System.Drawing.Point(0, 163);
            this.tblSense.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.tblSense.Name = "tblSense";
            this.tblSense.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.tblSense.RowCount = 3;
            this.tblSense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblSense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblSense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tblSense.Size = new System.Drawing.Size(400, 23);
            this.tblSense.TabIndex = 5;
            // 
            // lblSense
            // 
            this.lblSense.BackColor = System.Drawing.Color.Transparent;
            this.lblSense.Location = new System.Drawing.Point(12, 1);
            this.lblSense.Margin = new System.Windows.Forms.Padding(0);
            this.lblSense.Name = "lblSense";
            this.lblSense.Size = new System.Drawing.Size(376, 21);
            this.lblSense.TabIndex = 0;
            this.lblSense.Text = "Sense: language";
            this.lblSense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblHead);
            this.pnlTop.Controls.Add(this.pbCopyIcon);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(400, 48);
            this.pnlTop.TabIndex = 0;
            // 
            // lblHead
            // 
            this.lblHead.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHead.Location = new System.Drawing.Point(64, 6);
            this.lblHead.Name = "lblHead";
            this.lblHead.Size = new System.Drawing.Size(330, 32);
            this.lblHead.TabIndex = 1;
            this.lblHead.Text = "Copy...";
            this.lblHead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbCopyIcon
            // 
            this.pbCopyIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbCopyIcon.BackgroundImage")));
            this.pbCopyIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbCopyIcon.Location = new System.Drawing.Point(12, 4);
            this.pbCopyIcon.Name = "pbCopyIcon";
            this.pbCopyIcon.Size = new System.Drawing.Size(40, 40);
            this.pbCopyIcon.TabIndex = 0;
            this.pbCopyIcon.TabStop = false;
            // 
            // tblFull
            // 
            this.tblFull.BackColor = System.Drawing.Color.White;
            this.tblFull.ColumnCount = 1;
            this.tblFull.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblFull.Controls.Add(this.lblFullCedict, 0, 2);
            this.tblFull.Controls.Add(this.lblFullFormatted, 0, 1);
            this.tblFull.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblFull.Location = new System.Drawing.Point(0, 49);
            this.tblFull.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.tblFull.Name = "tblFull";
            this.tblFull.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.tblFull.RowCount = 4;
            this.tblFull.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblFull.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblFull.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblFull.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tblFull.Size = new System.Drawing.Size(400, 45);
            this.tblFull.TabIndex = 3;
            // 
            // lblFullCedict
            // 
            this.lblFullCedict.BackColor = System.Drawing.Color.Transparent;
            this.lblFullCedict.Location = new System.Drawing.Point(12, 22);
            this.lblFullCedict.Margin = new System.Windows.Forms.Padding(0);
            this.lblFullCedict.Name = "lblFullCedict";
            this.lblFullCedict.Size = new System.Drawing.Size(376, 21);
            this.lblFullCedict.TabIndex = 1;
            this.lblFullCedict.Text = "Full Entry (CEDICT)";
            this.lblFullCedict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFullFormatted
            // 
            this.lblFullFormatted.BackColor = System.Drawing.Color.Transparent;
            this.lblFullFormatted.Location = new System.Drawing.Point(12, 1);
            this.lblFullFormatted.Margin = new System.Windows.Forms.Padding(0);
            this.lblFullFormatted.Name = "lblFullFormatted";
            this.lblFullFormatted.Size = new System.Drawing.Size(376, 21);
            this.lblFullFormatted.TabIndex = 0;
            this.lblFullFormatted.Text = "Full Entry (Formatted)";
            this.lblFullFormatted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tblZho
            // 
            this.tblZho.BackColor = System.Drawing.Color.White;
            this.tblZho.ColumnCount = 1;
            this.tblZho.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblZho.Controls.Add(this.lblPinyin, 0, 3);
            this.tblZho.Controls.Add(this.lblHanzi2, 0, 2);
            this.tblZho.Controls.Add(this.lblHanzi1, 0, 1);
            this.tblZho.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblZho.Location = new System.Drawing.Point(0, 96);
            this.tblZho.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.tblZho.Name = "tblZho";
            this.tblZho.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.tblZho.RowCount = 5;
            this.tblZho.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblZho.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblZho.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblZho.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tblZho.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tblZho.Size = new System.Drawing.Size(400, 66);
            this.tblZho.TabIndex = 4;
            // 
            // lblPinyin
            // 
            this.lblPinyin.BackColor = System.Drawing.Color.Transparent;
            this.lblPinyin.Location = new System.Drawing.Point(12, 43);
            this.lblPinyin.Margin = new System.Windows.Forms.Padding(0);
            this.lblPinyin.Name = "lblPinyin";
            this.lblPinyin.Size = new System.Drawing.Size(376, 21);
            this.lblPinyin.TabIndex = 2;
            this.lblPinyin.Text = "Pinyin: yǔ ​yán";
            this.lblPinyin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHanzi2
            // 
            this.lblHanzi2.BackColor = System.Drawing.Color.Transparent;
            this.lblHanzi2.Location = new System.Drawing.Point(12, 22);
            this.lblHanzi2.Margin = new System.Windows.Forms.Padding(0);
            this.lblHanzi2.Name = "lblHanzi2";
            this.lblHanzi2.Size = new System.Drawing.Size(376, 21);
            this.lblHanzi2.TabIndex = 1;
            this.lblHanzi2.Text = "Traditional: 語言";
            this.lblHanzi2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHanzi1
            // 
            this.lblHanzi1.BackColor = System.Drawing.Color.Transparent;
            this.lblHanzi1.Location = new System.Drawing.Point(12, 1);
            this.lblHanzi1.Margin = new System.Windows.Forms.Padding(0);
            this.lblHanzi1.Name = "lblHanzi1";
            this.lblHanzi1.Size = new System.Drawing.Size(376, 21);
            this.lblHanzi1.TabIndex = 0;
            this.lblHanzi1.Text = "Simplified: 语言";
            this.lblHanzi1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResultsCtxtControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.Controls.Add(this.tblLayout);
            this.Name = "ResultsCtxtControl";
            this.Size = new System.Drawing.Size(402, 175);
            this.tblLayout.ResumeLayout(false);
            this.tblSense.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyIcon)).EndInit();
            this.tblFull.ResumeLayout(false);
            this.tblZho.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblLayout;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblHead;
        private System.Windows.Forms.PictureBox pbCopyIcon;
        private System.Windows.Forms.Label lblFullFormatted;
        private System.Windows.Forms.TableLayoutPanel tblFull;
        private System.Windows.Forms.TableLayoutPanel tblZho;
        private System.Windows.Forms.Label lblPinyin;
        private System.Windows.Forms.Label lblHanzi2;
        private System.Windows.Forms.Label lblHanzi1;
        private System.Windows.Forms.Label lblFullCedict;
        private System.Windows.Forms.TableLayoutPanel tblSense;
        private System.Windows.Forms.Label lblSense;
    }
}
