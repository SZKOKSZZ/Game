/*
 * Created by SharpDevelop.
 * User: szzua
 * Date: 2017. 11. 29.
 * Time: 15:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace gameproject.Models
{
	/// <summary>
	/// Description of Board.
	/// </summary>
	public class Board
	{
		
		public byte GridSize { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public Unit Unitp { get; set; }
		public ScrollViewer Viewer { get; set; }
        public List<Rectangle> CellsPath { get; set; }
        public List<BoardPiece> boardPieces;
        public int Split { get; set; }

        public Board(ScrollViewer viewer, byte gridsize,int width, int height)
		{
			Viewer =viewer;
            Width = width;
            Height = height;
            GridSize =gridsize;
		}
		
		
	}
}
