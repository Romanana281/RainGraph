using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project2
{
    public partial class BackgroundStyle : Form
    {
        public Color color1 { get; private set; }
        public Color color2 { get; private set; }
        public int index { get; private set; }


        public BackgroundStyle()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        private void ChangeColor()
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                color1 = dialog.Color;
                MessageBox.Show("Цвет 1 изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    color2 = dialog.Color;
                    MessageBox.Show("Цвет 2 изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void SolidColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                color1 = dialog.Color;
                MessageBox.Show("Цвет изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Hatching_Click(object sender, EventArgs e)
        {
            ChangeColor();
            index = 2;
        }

        private void Chess_Click(object sender, EventArgs e)
        {
            ChangeColor();
            index = 3;
        }

        private void Zigzag_Click(object sender, EventArgs e)
        {
            ChangeColor();
            index = 4;
        }

        private void Bricks_Click(object sender, EventArgs e)
        {
            ChangeColor();
            index = 5;
        }

        private void DiagonalG_Click(object sender, EventArgs e)
        {
            ChangeColor();
            index = 6;
        }

    }
}
