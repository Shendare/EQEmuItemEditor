namespace EQEmuItemEditor
{
    partial class formIconPicker
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
            this.label1 = new System.Windows.Forms.Label();
            this.listItemCategory = new System.Windows.Forms.ComboBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textIconID = new System.Windows.Forms.TextBox();
            this.flowIcons = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Category:";
            // 
            // listItemCategory
            // 
            this.listItemCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listItemCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listItemCategory.FormattingEnabled = true;
            this.listItemCategory.Location = new System.Drawing.Point(241, 8);
            this.listItemCategory.Name = "listItemCategory";
            this.listItemCategory.Size = new System.Drawing.Size(390, 21);
            this.listItemCategory.TabIndex = 1;
            this.listItemCategory.SelectedIndexChanged += new System.EventHandler(this.listItemCategory_SelectedIndexChanged);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelect.Location = new System.Drawing.Point(165, 376);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(135, 35);
            this.buttonSelect.TabIndex = 3;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(351, 376);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(135, 35);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Icon ID:";
            // 
            // textIconID
            // 
            this.textIconID.Location = new System.Drawing.Point(60, 8);
            this.textIconID.Name = "textIconID";
            this.textIconID.Size = new System.Drawing.Size(100, 20);
            this.textIconID.TabIndex = 0;
            this.textIconID.TextChanged += new System.EventHandler(this.textIconID_TextChanged);
            this.textIconID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.formTextboxNumber_Keypress);
            // 
            // flowIcons
            // 
            this.flowIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowIcons.AutoScroll = true;
            this.flowIcons.BackColor = System.Drawing.SystemColors.Window;
            this.flowIcons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowIcons.Location = new System.Drawing.Point(15, 34);
            this.flowIcons.Name = "flowIcons";
            this.flowIcons.Size = new System.Drawing.Size(616, 336);
            this.flowIcons.TabIndex = 2;
            this.flowIcons.TabStop = true;
            // 
            // formIconPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 423);
            this.Controls.Add(this.flowIcons);
            this.Controls.Add(this.textIconID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.listItemCategory);
            this.Controls.Add(this.label1);
            this.Name = "formIconPicker";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Icon Picker";
            this.Load += new System.EventHandler(this.formIconPicker_Load);
            this.VisibleChanged += new System.EventHandler(this.formIconPicker_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox listItemCategory;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textIconID;
        private System.Windows.Forms.FlowLayoutPanel flowIcons;
    }
}