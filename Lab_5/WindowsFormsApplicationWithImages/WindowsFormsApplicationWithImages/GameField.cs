using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplicationWithImages
{
    class GameField
    {
        private int[,] m_field;         // игровое поле
        
        public const int CELL_COUNT = 3;   // линейный размер игрового поля в клетках

        public const int CELL_EMPTY = -1;  // пустая клетка
        public const int CELL_NULL = 0;    // клетка с ноликом
        public const int CELL_CROSS = 1;   // клетка с крестиком

        public GameField()
        {
            m_field = new int[CELL_COUNT, CELL_COUNT];

            for (int x = 0; x < CELL_COUNT; x++)
            for (int y = 0; y < CELL_COUNT; y++)
            {
                m_field[x, y] = CELL_EMPTY;
            }
        }

        public void SetCellContents(int x, int y, int value)
        {
            if ((x < 0) || (y < 0) || (x >= CELL_COUNT) ||
                (y >= CELL_COUNT))
            {
                return;
            }

            if ((value != CELL_EMPTY) && (value != CELL_NULL) &&
                (value != CELL_CROSS))
            {
                return;
            }

            m_field[x, y] = value;
        }

        public int GetCellContents(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= CELL_COUNT) ||
                (y >= CELL_COUNT))
            {
                return CELL_EMPTY;
            }

            return m_field[x, y];
        }
    }
}
