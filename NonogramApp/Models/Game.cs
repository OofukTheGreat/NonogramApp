﻿using System;
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
        public bool HasWon {  get; private set; }
        public Game(LevelDTO Level) 
        {
            Board = MakeGameBoard(Level.Size, Level.Layout);
        }
        public Game(int size) 
        {
            Board = MakeDrawBoard(size);
        }
        public static Tile[,] MakeGameBoard(int size, string layout)
        {
            Tile[,] board = new Tile[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    board[i, k] = new Tile();
                }
            }
            int j = 0;
            int index = 0;
            string selectedColor = "White";
            for (int i = 0; i < size; i++)
            {
                string number = "";
                while (index < layout.Length && layout[index] != '.')
                {
                    if (layout[index] == ',')
                    {
                        int realnumber = int.Parse(number);
                        for (int h = 0; h < realnumber; h++)
                        {
                            board[i, j].TrueColor = selectedColor;
                            board[i, j].X = j;
                            board[i, j].Y = i;
                            board[i, j].BorderColor = Color.FromArgb("#808080");
                            board[i, j].BorderWidth = 1;
                            board[i, j].IsMarked = false;
                            j++;
                        }
                        
                        if (selectedColor == "White") selectedColor = "Black";
                        else selectedColor = "White";
                        number = "";
                    }
                    else number += layout[index];
                    index++;
                }
                index += 1;
                j = 0;
                selectedColor = "White";
            }
            board[0, 0].BorderColor = Color.FromArgb("#FF0000");
            board[0, 0].BorderWidth = 4;
            return board;
        }
        public static Tile[,] MakeDrawBoard(int size)
        {
            Tile[,] board = new Tile[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    board[i, k] = new Tile();
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j].X = j;
                    board[i, j].Y = i;
                    board[i, j].BorderColor = Color.FromArgb("#808080");
                    board[i, j].BorderWidth = 1;
                }
            }
            board[0, 0].BorderColor = Color.FromArgb("#FF0000");
            board[0, 0].BorderWidth = 4;
            return board;
        }
        public List<Tile> GetBoardAsList(int size)
        {
            List<Tile> list = new List<Tile>(size*size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    list.Add(Board[i, j]);
                }
            }
            return list;
        }
        public List<Hint> GetLayoutList(int size)
        {
            List<Hint> list = new List<Hint>(2 * size);
            for (int i = 0; i < size; i++)
            {
                int count = 0;
                string label = "";
                for (int j = 0; j < size; j++)
                {
                    if (this.Board[i, j].TrueColor == "White")
                    {
                        if (count != 0)
                        {
                            label += ' ';
                            label += count;
                        }
                        count = 0;
                    }
                    else if (j == size - 1 && this.Board[i, j].TrueColor == "Black")
                    {
                        count++;
                        label += ' ';
                        label += count;
                        count = 0;
                    }
                    else count++;
                }
                if (label == "") label = "0";
                list.Add(new Hint(0, i+1, label, false));
            }
            for (int i = 0; i < size; i++)
            {
                int count = 0;
                string label = "";
                for (int j = 0; j < size; j++)
                {
                    if (this.Board[j, i].TrueColor == "White")
                    {
                        if (count != 0) label += count;
                        count = 0;
                    }
                    else if (j == size - 1 && this.Board[j, i].TrueColor == "Black")
                    {
                        count++;
                        label += count;
                        count = 0;
                    }
                    else count++;
                }
                if (label == "") label = "0";
                list.Add(new Hint(i + 1, 0, label, true));
            }
            return list;
        }
    }
}
