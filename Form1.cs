using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // загрузка пикчи
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog nf = new OpenFileDialog();
            nf.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|Все файлы(*.*)|*.*";
            if (nf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(nf.FileName);
                    pictureBox2.Image = new Bitmap(nf.FileName);
                    
                    button2.Visible = true;
                    var bmp = new Bitmap(pictureBox1.Image);
                    Hist(bmp);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл");
                }
            }
        }

        public int SRV(int l)
        {
            if (l > 255)
            {
                l = 255;
            }
            else if (l < 0)
            {
                l = 0;
            }
            return l;
        }

        // процедура превращения в чб
        public int GRY(int K, int B, int O)
        {
            double GrayDouble = 0.2125 * K + 0.7154 * B + 0.0721 * O;
            int Gray = Convert.ToInt32(GrayDouble);
            return Gray;
        }

        //процедура построения гистограмы
        public void Hist(Bitmap pic)
        {
            int[] x = new int[256];
            for (int i = 0; i < pic.Width; i++)
                for (int j = 0; j < pic.Height; j++)
                {
                    int R = pic.GetPixel(i, j).R;
                    int G = pic.GetPixel(i, j).G;
                    int B = pic.GetPixel(i, j).B;
                    int Gray = GRY(R, G, B);
                    x[Gray]++;

                }

            chart1.Series[0].Points.Clear();
            chart1.Series["Series1"]["PointWidth"] = "1";

            int c = 0;
            while (c < 255)
            {
                this.chart1.Series["Series1"].Points.AddXY(c, x[c]);
                c++;
            }
        }

        //Осветление
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = new Bitmap(pictureBox1.Image);

                int k = Convert.ToInt32(textBox1.Text);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R + k;
                        R = SRV(R);
                        int G = bmp.GetPixel(i, j).G + k;
                        G = SRV(G);
                        int B = bmp.GetPixel(i, j).B + k;
                        B = SRV(B);
                        Color RGB = Color.FromArgb(255, R, G, B);
                        bmp.SetPixel(i, j, RGB);
                    }
                Hist(bmp);
                pictureBox1.Image = bmp;
            }
            catch
            {
                MessageBox.Show("Не загружено изображение или не правильно указано значение для осветления, верните стол на место " + Environment.NewLine +
                   "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //негатив оттенков серого
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = new Bitmap(pictureBox1.Image);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R;
                        int G = bmp.GetPixel(i, j).G;
                        int B = bmp.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        Color RGB = Color.FromArgb(255, 255 - Gray, 255 - Gray, 255 - Gray);
                        bmp.SetPixel(i, j, RGB);
                    }
                pictureBox1.Image = bmp;
                Hist(bmp);
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                   "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //откат
        private void button4_Click(object sender, EventArgs e)
        {
            try
            { 
            pictureBox1.Image = pictureBox2.Image;
            var bmp = new Bitmap(pictureBox1.Image);
            Hist(bmp);
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " +Environment.NewLine+
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //бинеризация
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int t = Convert.ToInt32(textBox2.Text);
                if (t < 0 || t > 255)
                {
                    MessageBox.Show("Значение t должно находиться в промежутке от 0 и до 255");
                    textBox2.Text = "0";
                }
                else
                {
                    var bmp = new Bitmap(pictureBox1.Image);
                    for (int i = 0; i < bmp.Width; i++)
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            int R = bmp.GetPixel(i, j).R;
                            int G = bmp.GetPixel(i, j).G;
                            int B = bmp.GetPixel(i, j).B;
                            int Gray = GRY(R, G, B);
                            if (Gray < t)
                            {
                                R = G = B = 0;
                            }
                            else
                            {
                                R = G = B = 255;
                            }
                            Color RGB = Color.FromArgb(255, R, G, B);
                            bmp.SetPixel(i, j, RGB);
                        }

                    pictureBox1.Image = bmp;
                    Hist(bmp);
                }
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, не указано значение t, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Гистограммное растягивание
        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                int a = 0;
                int b = 0;
                int[] x = new int[256];
                var bmp = new Bitmap(pictureBox1.Image);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R;
                        int G = bmp.GetPixel(i, j).G;
                        int B = bmp.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        x[Gray]++;

                    }

                int c = 0;
                while (c <= 255)
                {
                    if (x[c] > 0)
                        a = c;
                    if (x[255 - c] > 0)
                        b = 255 - c;

                    c++;
                }

                double px = 255 / ((double)a - (double)b);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R;
                        int G = bmp.GetPixel(i, j).G;
                        int B = bmp.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        Color RGB = Color.FromArgb(255, (Convert.ToInt32((Gray - b) * px)),
                            (Convert.ToInt32((Gray - b) * px)), (Convert.ToInt32((Gray - b) * px)));
                        bmp.SetPixel(i, j, RGB);

                    }

                Hist(bmp);
                pictureBox1.Image = bmp;
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //псеводраскрашивание
        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = new Bitmap(pictureBox1.Image);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R;
                        int G = bmp.GetPixel(i, j).G;
                        int B = bmp.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        Color RGB = Color.FromArgb(255, 50, Gray, 100);
                        bmp.SetPixel(i, j, RGB);
                    }
                Hist(bmp);
                pictureBox1.Image = bmp;
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //сглаживание
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var res = new Bitmap(pictureBox1.Image);
                int[,] imageMas = new int[res.Width + 2, res.Height + 2];
                int[,] newMas = new int[res.Width, res.Height];

                for (int i = 0; i < res.Width; i++)
                {
                    for (int j = 0; j < res.Height; j++)
                    {
                        int R = res.GetPixel(i, j).R;
                        int G = res.GetPixel(i, j).G;
                        int B = res.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        imageMas[i + 1, j + 1] = Gray;
                    }
                }

                // 1....2
                // ......
                // 3....4

                //1 Ready
                imageMas[0, 0] = imageMas[1, 1];
                //2
                imageMas[res.Width + 1, 0] = imageMas[res.Width, 1];
                //3
                imageMas[0, res.Height + 1] = imageMas[1, res.Height];
                //4
                imageMas[res.Width + 1, res.Height + 1] = imageMas[res.Width, res.Height];

                // ......
                // 1.....
                // ......
                for (int j = 1; j < res.Height + 1; j++)
                    imageMas[0, j] = imageMas[1, j];

                // ......
                // .....1
                // ......
                for (int j = 1; j < res.Height + 1; j++)
                    imageMas[res.Width + 1, j] = imageMas[res.Width, j];

                // .1111.
                // ......  Ready
                // ......
                for (int i = 1; i < res.Width + 1; i++)
                    imageMas[i, 0] = imageMas[i, 1];

                // ......
                // ......
                // .1111.
                for (int i = 1; i < res.Width + 1; i++)
                    imageMas[i, res.Height + 1] = imageMas[i, res.Height];

                // Получаем новые значения с применения маски
                int t1 = 1; int t2 = 2; int t3 = 1;
                int t4 = 2; int t5 = 4; int t6 = 2;
                int t7 = 1; int t8 = 2; int t9 = 1;

                for (int i = 1; i < res.Width; i++)
                {
                    for (int j = 1; j < res.Height; j++)
                    {
                        newMas[i - 1, j - 1] = t1 * imageMas[i - 1, j - 1] +
                            t2 * imageMas[i - 1, j] +
                            t3 * imageMas[i - 1, j + 1] +
                            t4 * imageMas[i, j - 1] +
                            t5 * imageMas[i, j] +
                            t6 * imageMas[i, j + 1] +
                            t7 * imageMas[i + 1, j - 1] +
                            t8 * imageMas[i + 1, j] +
                            t9 * imageMas[i + 1, j + 1];
                        newMas[i - 1, j - 1] = Convert.ToInt32(newMas[i - 1, j - 1] / 16.0);
                    }
                }

                for (int i = 0; i < res.Width - 1; i++)
                    for (int j = 0; j < res.Height - 1; j++)
                    {
                        Color p = Color.FromArgb(255, newMas[i, j], newMas[i, j], newMas[i, j]);
                        // Записываем цвет в текущую точку 
                        res.SetPixel(i, j, p);
                    }
                Hist(res);
                pictureBox1.Image = res;
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Усиления края
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = new Bitmap(pictureBox1.Image);

                int[,] imageMas = new int[bmp.Width + 2, bmp.Height + 2];
                int[,] newMas = new int[bmp.Width, bmp.Height];

                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int R = bmp.GetPixel(i, j).R;
                        int G = bmp.GetPixel(i, j).G;
                        int B = bmp.GetPixel(i, j).B;
                        int Gray = GRY(R, G, B);
                        imageMas[i + 1, j + 1] = Gray;
                    }
                }

                // 1....2
                // ......
                // 3....4

                //1 Ready
                imageMas[0, 0] = imageMas[1, 1];
                //2
                imageMas[bmp.Width + 1, 0] = imageMas[bmp.Width, 1];
                //3
                imageMas[0, bmp.Height + 1] = imageMas[1, bmp.Height];
                //4
                imageMas[bmp.Width + 1, bmp.Height + 1] = imageMas[bmp.Width, bmp.Height];

                // ......
                // 1.....
                // ......
                for (int j = 1; j < bmp.Height + 1; j++)
                    imageMas[0, j] = imageMas[1, j];

                // ......
                // .....1
                // ......
                for (int j = 1; j < bmp.Height + 1; j++)
                    imageMas[bmp.Width + 1, j] = imageMas[bmp.Width, j];

                // .1111.
                // ......  Ready
                // ......
                for (int i = 1; i < bmp.Width + 1; i++)
                    imageMas[i, 0] = imageMas[i, 1];

                // ......
                // ......
                // .1111.
                for (int i = 1; i < bmp.Width + 1; i++)
                    imageMas[i, bmp.Height + 1] = imageMas[i, bmp.Height];

                //Лапласа
                if (radioButton1.Checked)
                {

                    int t1 = -1; int t2 = -2; int t3 = -1;
                    int t4 = -2; int t5 = 12; int t6 = -2;
                    int t7 = -1; int t8 = -2; int t9 = -1;

                    for (int i = 1; i < bmp.Width; i++)
                    {
                        for (int j = 1; j < bmp.Height; j++)
                        {
                            newMas[i - 1, j - 1] = t1 * imageMas[i - 1, j - 1] +
                                t2 * imageMas[i - 1, j] +
                                t3 * imageMas[i - 1, j + 1] +
                                t4 * imageMas[i, j - 1] +
                                t5 * imageMas[i, j] +
                                t6 * imageMas[i, j + 1] +
                                t7 * imageMas[i + 1, j - 1] +
                                t8 * imageMas[i + 1, j] +
                                t9 * imageMas[i + 1, j + 1];

                        }
                    }

                    for (int i = 0; i < bmp.Width - 1; i++)
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            Color p = Color.FromArgb(255, SRV(newMas[i, j]), SRV(newMas[i, j]), SRV(newMas[i, j]));
                            bmp.SetPixel(i, j, p);
                        }
                }

                //Робертса
                if (radioButton2.Checked)
                {

                    for (int i = 0; i < bmp.Width - 1; i++)
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            int R = bmp.GetPixel(i, j).R;
                            int G = bmp.GetPixel(i, j).G;
                            int B = bmp.GetPixel(i, j).B;
                            int A = GRY(R, G, B);
                            R = bmp.GetPixel(i + 1, j).R;
                            G = bmp.GetPixel(i + 1, j).G;
                            B = bmp.GetPixel(i + 1, j).B;
                            int C = GRY(R, G, B);
                            R = bmp.GetPixel(i, j + 1).R;
                            G = bmp.GetPixel(i, j + 1).G;
                            B = bmp.GetPixel(i, j + 1).B;
                            int B1 = GRY(R, G, B);
                            R = bmp.GetPixel(i + 1, j + 1).R;
                            G = bmp.GetPixel(i + 1, j + 1).G;
                            B = bmp.GetPixel(i + 1, j + 1).B;
                            int D = GRY(R, G, B);
                            double A1 = Math.Sqrt((A - D) * (A - D) + (B1 - C) * (B1 - C));
                            Color RGB = Color.FromArgb(255, SRV(Convert.ToInt32(A1)), SRV(Convert.ToInt32(A1)), SRV(Convert.ToInt32(A1)));
                            bmp.SetPixel(i, j, RGB);
                        }
                }
                //Кирша
                if (radioButton3.Checked)
                {
                    for (int x = 1; x < bmp.Width - 1; x++)
                        for (int y = 1; y < bmp.Height - 1; y++)
                        {
                            int max = 0;
                            int[] Si = new int[8];
                            int[] Ti = new int[8];
                            int[] F = new int[8];
                            int[] A = new int[8];
                            A[0] = imageMas[x - 1, y - 1];
                            A[1] = imageMas[x, y - 1];
                            A[2] = imageMas[x + 1, y - 1];
                            A[3] = imageMas[x + 1, y];
                            A[4] = imageMas[x + 1, y + 1];
                            A[5] = imageMas[x, y + 1];
                            A[6] = imageMas[x - 1, y + 1];
                            A[7] = imageMas[x - 1, y];

                            for (int i = 0; i < 8; i++)
                            {
                                Si[i] = A[i] + A[(i + 1) % 8] + A[(i + 2) % 8];
                                Ti[i] = A[(i + 3) % 8] + A[(i + 4) % 8] + A[(i + 5) % 8] + A[(i + 6) % 8] + A[(i + 7) % 8];
                                F[i] = Math.Abs(5 * Si[i] - 3 * Ti[i]);

                                if (F[i] > max) max = F[i];
                            }

                            newMas[x, y] = max;
                        }

                    for (int x = 0; x < bmp.Width - 1; x++)
                        for (int y = 0; y < bmp.Height - 1; y++)
                        {
                            int Kirsh = newMas[x, y];
                            Kirsh = SRV(Kirsh);
                            Color next = Color.FromArgb(255, Kirsh, Kirsh, Kirsh);
                            bmp.SetPixel(x, y, next);
                        }
                }
                //Собеля
                if (radioButton4.Checked)
                {
                    int t1 = -1; int t2 = -2; int t3 = -1;
                    int t4 = 0; int t5 = 0; int t6 = 0;
                    int t7 = 1; int t8 = 2; int t9 = 1;

                    for (int i = 1; i < bmp.Width; i++)
                    {
                        for (int j = 1; j < bmp.Height; j++)
                        {
                            double gx = t1 * imageMas[i - 1, j - 1] + t2 * imageMas[i - 1, j] +
                               t3 * imageMas[i - 1, j + 1] +
                               t4 * imageMas[i, j - 1] +
                               t5 * imageMas[i, j] +
                               t6 * imageMas[i, j + 1] +
                               t7 * imageMas[i + 1, j - 1] +
                               t8 * imageMas[i + 1, j] +
                               t9 * imageMas[i + 1, j + 1];

                            double gy = t1 * imageMas[i - 1, j - 1] + t4 * imageMas[i - 1, j] +
                                t7 * imageMas[i - 1, j + 1] +
                                t2 * imageMas[i, j - 1] +
                                t5 * imageMas[i, j] +
                                t8 * imageMas[i, j + 1] +
                                t3 * imageMas[i + 1, j - 1] +
                                t6 * imageMas[i + 1, j] +
                                t9 * imageMas[i + 1, j + 1];

                            newMas[i - 1, j - 1] = Convert.ToInt32(Math.Sqrt(gx * gx + gy * gy));
                        }
                    }

                    for (int i = 0; i < bmp.Width - 1; i++)
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            Color p = Color.FromArgb(255, SRV(newMas[i, j]), SRV(newMas[i, j]), SRV(newMas[i, j]));
                            bmp.SetPixel(i, j, p);
                        }
                }

                Hist(bmp);
                pictureBox1.Image = bmp;
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Сохранение
        private void Button10_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = new Bitmap(pictureBox1.Image);
                if (pictureBox1.Image != null)
                {
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Title = "Сохранить как...";
                    //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                    savedialog.OverwritePrompt = true;
                    //отображать ли предупреждение, если пользователь указывает несуществующий путь
                    savedialog.CheckPathExists = true;
                    savedialog.Filter = "Image Files(*.JPG)|*.JPG|Image Files(*.BMP)|*.BMP|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                    //отображается ли кнопка "Справка" в диалоговом окне
                    savedialog.ShowHelp = true;
                    if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                    {
                        try
                        {
                            bmp.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не загружено изображение, верните стол на место " + Environment.NewLine +
                    "┬─┬ノ( º _ ºノ)", "(╯°益°)╯彡┻━┻", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
