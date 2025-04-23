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
        private string currentColor; 
        public string CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        private bool isMarked;
        public bool IsMarked
        {
            get
            {
                return isMarked;
            }
            set
            {
                isMarked = value;
                OnPropertyChanged(nameof(IsMarked));
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
        private int borderWidth;
        public int BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
                OnPropertyChanged(nameof(BorderWidth));
            }
        }
        public Tile()
        {
            this.X = 0;
            this.Y = 0;
            this.CurrentColor = "White";
            this.TrueColor = "White";
        }
        public void Blacken()
        {
            if (this.IsMarked) Mark();
            else if (this.CurrentColor == "Black") this.CurrentColor = "White";
            else this.CurrentColor = "Black";
        }
        public void Mark()
        {
            if (this.CurrentColor == "Black") this.CurrentColor = "White";
            else if (this.IsMarked) this.IsMarked = false;
            else this.IsMarked = true;
        }
    }
}
