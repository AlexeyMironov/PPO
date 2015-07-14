using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplicationWithAnimation
{
    public partial class Form1 : Form
    {
        private Image m_ball, m_field;
        private int m_counter, m_time1, m_time2, m_step;

        //private const int BALL_STEP = 20;       // смещение мячика между срабатываниями таймера
        private const int BALL_AMPLITUDE = 421; // амплитуда движения мяча по ординате
        private const int BALL_X_OFFSET = 716;  // абсцисса мяча

        public Form1()
        {
            InitializeComponent();

            m_counter = 0;
            m_time1 = 0;

            // включаем double-buffering, чтобы изображение
            // не моргало/мерцало при отрисовке
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);

            // если картинка не загрузилась, выходим
            if (!LoadImage(ref m_ball, "ball1.png"))
            {
                this.Close();
            }

            if (!LoadImage(ref m_field, "Field.jpg"))
            {
                this.Close();
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

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Вызвать OnPaint у базового класса
            base.OnPaint(pe);
            //устанавливаем фоном футбольное поле
            base.BackgroundImage = m_field;
            base.BackgroundImageLayout = ImageLayout.Stretch;
            // рассчитываем текущую ординату мячика
            int y = m_counter % (2 * BALL_AMPLITUDE);

            if (y > BALL_AMPLITUDE)
            {
                y = (2 * BALL_AMPLITUDE) - y;
            }

            // рассчитываем текущую абсциссу мячика
            int x = m_counter % (2 * BALL_X_OFFSET);

            if (x > BALL_X_OFFSET)
            {
                x = (2 * BALL_X_OFFSET) - x;
            }

            // рисуем мячик
            pe.Graphics.DrawImage(m_ball, new Point(x, y));            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // на самом деле смещение шарика должно зависеть от реально
            // прошедшего времени между срабатываниями таймера
            m_time2 = Environment.TickCount;
            m_step = (m_time2 - m_time1) / 2;
            m_time1 = m_time2;
            m_counter += m_step;

            // форсируем перерисовку окна
            this.Refresh();
        }
    }
}
