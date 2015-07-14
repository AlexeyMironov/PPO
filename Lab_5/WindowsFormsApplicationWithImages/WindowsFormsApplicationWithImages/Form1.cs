using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplicationWithImages
{
    public partial class Form1 : Form
    {        
        private Image m_imageNull;      // картинка под нолик
        private Image m_imageCross;     // картинка под крестик

        private bool m_isOk;            // статус, загрузились ли картинки
        private int m_lastFigure;       // последняя фигура (ход)
        private GameField m_gameField;  // игровое поле

        public const int CELL_SIZE = 100;  // размер клетки в пикселях

        public Form1()
        {
            InitializeComponent();

            // включаем double-buffering, чтобы изображение
            // не моргало/мерцало при отрисовке
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);

            // если картинки загрузились
            if (m_isOk = LoadImages())
            {
                // инициализируем игровое поле
                m_gameField = new GameField();

                m_gameField.SetCellContents(0, 0, GameField.CELL_NULL);
                m_lastFigure = GameField.CELL_CROSS;
                m_gameField.SetCellContents(1, 1, m_lastFigure);                
            }
        }

        private bool LoadImage(ref Image image, string fileName)
        {
            try
            {
                image = Image.FromFile(fileName);                
            }
            catch (Exception e)
            {
                MessageBox.Show("Картинки не могут быть загружены!", "Ошибка");
                return false;
            }

            return true;
        }

        private bool LoadImages()
        {
            if (!LoadImage(ref m_imageNull, "image1.jpg"))
            {
                return false;
            }

            if (!LoadImage(ref m_imageCross, "image2.jpg"))
            {
                m_imageNull.Dispose();
                m_imageNull = null;
                return false;
            }               

            return true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Вызвать OnPaint у базового класса
            base.OnPaint(pe);

            if (m_isOk)
            {
                for (int x = 0; x < GameField.CELL_COUNT; x++)
                for (int y = 0; y < GameField.CELL_COUNT; y++)
                {
                    Point imageOffset = new Point(x * CELL_SIZE, y * CELL_SIZE);
                    int cellContent = m_gameField.GetCellContents(x, y);
                    if (cellContent == GameField.CELL_NULL)
                    {
                        pe.Graphics.DrawImage(m_imageNull, imageOffset);
                    }
                    else if (cellContent == GameField.CELL_CROSS)
                    {
                        pe.Graphics.DrawImage(m_imageCross, imageOffset);
                    }
                }
            }
        }

        ~Form1()
        {
            if (m_imageNull != null)
            {
                m_imageNull.Dispose();
            }
             if (m_imageCross != null)
            {
                m_imageCross.Dispose();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_isOk)
            for (int x = 0; x < GameField.CELL_COUNT; x++)
            for (int y = 0; y < GameField.CELL_COUNT; y++)
            if ((e.X >= x * CELL_SIZE) && (e.X < (x + 1) * CELL_SIZE) &&
                (e.Y >= y * CELL_SIZE) && (e.Y < (y + 1) * CELL_SIZE))
            {
                m_lastFigure = (m_lastFigure == GameField.CELL_CROSS) ? GameField.CELL_NULL : GameField.CELL_CROSS;
                m_gameField.SetCellContents(x, y, m_lastFigure);
                this.Refresh();
                break;
            }
        }
    }
}
