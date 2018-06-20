using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ERP.Base
{
    class DataGridCustom
    {

    }

    public class DataGridExtendedTextBoxColumn : DataGridTextBoxColumn
    {
        private Brush backBrush = new SolidBrush(Color.White);
        private Brush foreBrush = new SolidBrush(Color.Black);

        //private Brush backBrush;
        //private Brush foreBrush;

        public Brush BackBrush
        {
            get { return backBrush; }
            set { backBrush = value; }
        }

        public Brush ForeBrush
        {
            get { return foreBrush; }
            set { foreBrush = value; }
        }

        public DataGridExtendedTextBoxColumn()
        {
        }

        public DataGridExtendedTextBoxColumn(Color backColor, Color foreColor)
        {
            this.backBrush = new SolidBrush(backColor);
            this.foreBrush = new SolidBrush(foreColor);
        }

        // You'll need to override the paint method
        // The easy way: only change fore-/backBrush
        protected override void Paint(Graphics _g, Rectangle _bounds, CurrencyManager _source, int _rowNum, Brush _backBrush, Brush _foreBrush, bool _alignToRight)
        {
            _backBrush = this.backBrush;
            _foreBrush = this.foreBrush;
            base.Paint(_g, _bounds, _source, _rowNum, _backBrush, _foreBrush, _alignToRight);
        }
    }
}
