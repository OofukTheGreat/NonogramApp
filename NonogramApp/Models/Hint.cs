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
        private Thickness margin;
        public Thickness Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }
        private TextAlignment horAlign;
        public TextAlignment HorAlign
        {
            get
            {
                return horAlign;
            }
            set
            {
                horAlign = value;
                OnPropertyChanged(nameof(HorAlign));
            }
        }
        private TextAlignment vertAlign;
        public TextAlignment VertAlign
        {
            get
            {
                return vertAlign;
            }
            set
            {
                vertAlign = value;
                OnPropertyChanged(nameof(VertAlign));
            }
        }
        private string textColor;
        public string TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }
        public Hint()
        {
            this.X = 0;
            this.Y = 0;
            this.Text = "";
        }
        public Hint(int x, int y, string text, bool istop)
        {
            if (istop)
            {
                HorAlign = TextAlignment.Center;
                VertAlign = TextAlignment.End;
                Margin = new Thickness(12, 4, 12, 4);
            }
            else if (!istop)
            {
                HorAlign = TextAlignment.End;
                VertAlign = TextAlignment.Center;
                Margin = new Thickness(4, 9, 4, 9);
            }
            this.TextColor = "White";
            this.X = x;
            this.Y = y;
            this.Text = text;
        }
    }
}
