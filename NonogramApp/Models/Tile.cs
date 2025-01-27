using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class Tile : INotifyPropertyChanged
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
        private string currentColor;  //קונבנציה - טרו זה לבן, פולס זה שחור
        public string CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }
        private string trueColor;
        public string TrueColor
        {
            get
            {
                return trueColor;
            }
            set
            {
                trueColor = value;
                OnPropertyChanged(nameof(TrueColor));
            }
        }
        private Color borderColor;
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }
        public Tile()
        {
            this.X = 0;
            this.Y = 0;
            this.CurrentColor = "White";
            this.TrueColor = "White";
        }
        public void FlipColor()
        {
            if (this.CurrentColor == "Black") this.CurrentColor = "White";
            else this.CurrentColor = "Black";
        }
        public void Black()
        {
            this.CurrentColor = "Black";
        }
        public void White()
        {
            this.CurrentColor = "White";
        }
    }
}
