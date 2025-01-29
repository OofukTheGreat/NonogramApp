using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class Hint : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        private int x;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        private int y;
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public Hint()
        {
            this.X = 0;
            this.Y = 0;
            this.Text = "";
        }
        public Hint(int x, int y, string text)
        {
            this.X = x;
            this.Y = y;
            this.Text = text;
        }
    }
}
