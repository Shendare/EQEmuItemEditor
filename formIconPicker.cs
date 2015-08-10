/*
 *  EQEmuItemEditor - Application for customizing EQEmu server items
 *  
 *  By Shendare (Jon D. Jackson)
 * 
 *  Portions of this code not covered by another author's or entity's copyright are released under
 *  the Creative Commons Zero (CC0) public domain license.
 *  
 *  To the extent possible under law, Shendare (Jon D. Jackson) has waived all copyright and
 *  related or neighboring rights to this EQEmuItemEditor application.
 *  This work is published from: The United States. 
 *  
 *  You may copy, modify, and distribute the work, even for commercial purposes, without asking permission.
 * 
 *  For more information, read the CC0 summary and full legal text here:
 *  
 *  https://creativecommons.org/publicdomain/zero/1.0/
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EQEmuItemEditor
{
    public partial class formIconPicker : Form
    {
        private formMain MainForm;

        private int IconID = -1;
        private PictureBox IconSelected = null;

        public formIconPicker()
        {
            InitializeComponent();
        }

        private void formIconPicker_Load(object sender, EventArgs e)
        {
            MainForm = (formMain)this.Owner;

            if (MainForm.itemIcons.Images.Count == 0)
            {
                listItemCategory.Enabled = false;
                listItemCategory.Text = "No local item icons found.";
                flowIcons.BackColor = SystemColors.ControlLight;
            }
            else
            {
                listItemCategory.Enabled = true;
                listItemCategory.Items.AddRange(MainForm.IconCategories.ToArray());

                flowIcons.SuspendLayout();

                List<PictureBox> _iconcache = new List<PictureBox>();

                for (int _i = 500; _i < 10000; _i++)
                {
                    int _iconcat;
                    
                    if (MainForm.CategoryOfIcon.TryGetValue(_i, out _iconcat))
                    {
                        PictureBox _icon = new PictureBox();

                        _icon.Name = "Icon" + _i.ToString();
                        _icon.Image = MainForm.IconImages[_i];
                        _icon.Width = 40;
                        _icon.Height = 40;
                        _icon.BackColor = SystemColors.Window;
                        _icon.Tag = _iconcat;
                        _icon.Click += pictureIcon_Click;
                        //new ToolTip().SetToolTip(_icon, _i.ToString());

                        flowIcons.Controls.Add(_icon);
                        _iconcache.Add(_icon);
                    }
                }

                flowIcons.ResumeLayout();
            }
        }

        private void formTextboxNumber_Keypress(object sender, KeyPressEventArgs e)
        {
            int _dummy;

            TextBox _tbox = sender as TextBox;
            ComboBox _cbox = sender as ComboBox;

            int _selstart = (_tbox != null) ? _tbox.SelectionStart : (_cbox != null) ? _cbox.SelectionStart : 0;
            int _sellength = (_tbox != null) ? _tbox.SelectionLength : (_cbox != null) ? _cbox.SelectionLength : 0;
            string _text = ((Control)sender).Text.Remove(_selstart, _sellength);

            if (e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = true; // _text != "";
            }
            else if (e.KeyChar > 31)
            {
                e.Handled = !int.TryParse(_text.Insert(_selstart, "" + e.KeyChar), out _dummy);
            }
        }

        private void listItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IconID != -1)
            {
                flowIcons.SuspendLayout();
                int _cat = listItemCategory.SelectedIndex;

                foreach (PictureBox _box in flowIcons.Controls)
                {
                    _box.Visible = _cat == (int)_box.Tag;
                }

                flowIcons.ResumeLayout();

                PictureBox _icon = (PictureBox)flowIcons.Controls["Icon" + IconID.ToString()];
                if ((_icon != null) && _icon.Visible)
                {
                    flowIcons.ScrollControlIntoView(_icon);
                }
            }
        }

        private void textIconID_TextChanged(object sender, EventArgs e)
        {
            int _iconID = 0;

            if (int.TryParse(textIconID.Text, out _iconID))
            {
                if (IconSelected != null)
                {
                    IconSelected.BackColor = SystemColors.Window;
                }

                IconID = _iconID;
                IconSelected = (PictureBox)flowIcons.Controls["Icon" + IconID.ToString()];
                if (IconSelected != null)
                {
                    IconSelected.BackColor = SystemColors.Highlight;
                }

                try
                {
                    listItemCategory.SelectedIndex = MainForm.CategoryOfIcon[IconID];
                }
                catch
                {
                    listItemCategory.SelectedIndex = -1;
                }
            }
        }

        private void pictureIcon_Click(object sender, EventArgs e)
        {
            textIconID.Text = ((PictureBox)sender).Name.Substring(4);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            MainForm.SetField("icon", textIconID.Text);
            MainForm.UpdatePreviewBox(0);

            this.Visible = false;
        }

        private void formIconPicker_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                textIconID.Text = MainForm.TextField("icon");

                if (MainForm.IconPickerWindowCoordsValid)
                {
                    this.Location = MainForm.IconPickerWindowCoords;
                }
            }
            else
            {
                MainForm.IconPickerWindowCoords = this.Location;
                MainForm.IconPickerWindowCoordsValid = true;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
