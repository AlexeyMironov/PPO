using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("Имя не заполнено!", "Ошибка!");
                return;
            }

            int age = (int)numericUpDown1.Value;
            if (age > 17 || age == 0)
            {
                MessageBox.Show("Возраст вне диапазона!", "Ошибка!");
                return;
            }

            Form2 newForm = new Form2();
            newForm.SetName(this.textBox1.Text);
            newForm.SetAge(this.numericUpDown1.Value.ToString());
            newForm.SetGift(this.comboBox1.Text);

            this.Visible = false;
            newForm.ShowDialog();
            this.Close();
        }
    }
}
