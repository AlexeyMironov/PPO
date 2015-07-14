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
    public partial class Form2 : Form
    {
        private string m_name;
        private string m_age;
        private string m_gift;

        public Form2()
        {
            InitializeComponent();
        }

        public void SetName(string name)
        {
            m_name = (string)name.Clone();
        }

        public void SetAge(string age)
        {
            m_age = (string)age.Clone();
        }

        public void SetGift(string gift)
        {
            m_gift = (string)gift.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder("", 1000);
            result.Append("Дедушка Мороз, подари мне на Новый Год ");
            result.Append(m_gift);
            result.Append("                                                             ");
            result.Append(m_name);
            result.Append(", ");
            result.Append(m_age);
            result.Append(" лет ");
            textBox1.Text = result.ToString();
        }
    }
}
