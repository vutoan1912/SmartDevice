using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ERP
{
    public partial class TextEditable : Form
    {
        public TextEditable()
        {
            InitializeComponent();
        }

        private string _valueEdit;
        public string valueEdit { set { _valueEdit = value; } get { return _valueEdit; } }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this._valueEdit = txtValue.Text;
                this.Close();
            }
        }
    }
}