using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AuroraServer
{
    public partial class about : Form
    {
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;

        public about()
        {
            InitializeComponent();
            label4.Font = new Font(label4.Font, FontStyle.Underline);
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastCursor = Cursor.Position;
                lastForm = Location;
            }
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int deltaX = Cursor.Position.X - lastCursor.X;
                int deltaY = Cursor.Position.Y - lastCursor.Y;
                Location = new Point(lastForm.X + deltaX, lastForm.Y + deltaY);
            }
        }

        private void topPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/k3rnel-dev");
        }
    }
}
