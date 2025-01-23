using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace NonogramApp.Models
{
    public class Game
    {
        //#region INotifyPropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion
        private Tile[,] Board;
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool HasWon {  get; private set; }
        public Game(LevelDTO Level) 
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            Board = MakeBoard(Level.DifficultyId, Level.Layout);
        }
        public static Tile[,] MakeBoard(int size, string layout)
        {
            Tile[,] board = new Tile[size, size];
            int j = 0;
            int index = 0;
            string selectedColor = "White";
            for (int i = 0; i < size; i++)
            {
                while (layout[index] != '.')
                {
                    for (int h = 0; h < int.Parse(layout[index].ToString()); h++)
                    {
                        board[i, j].TrueColor = selectedColor;
                        board[i,j].X = j+1;
                        board[i,j].Y = i+1;
                        j++;
                    }
                    index++;
                    if (selectedColor == "White") selectedColor = "Black";
                    else selectedColor = "White";
                }
                index += 1;
                j = 0;
                selectedColor = "White";
            }
            return board;
        }
        public List<Tile> GetBoardAsList(int size)
        {
            List<Tile> list = new List<Tile>(size*size);
            for (int i = 0; i < Board.Length; i++)
            {
                for (int j = 0; j < Board.Length; j++)
                    list.Add(Board[i, j]);
            }
            return list;
        }
    }
}
