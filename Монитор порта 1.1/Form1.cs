using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Монитор_порта_1._1
{
    public partial class Form1 : Form
    {
        //=============================== Введение переменных ===============================//
        public string  data = "";
        public string[] str;
        public string  temp;
        public int        t;

        public int zeropress;

        public int           tim, press;
        public float          barheight;
        public string              stat;
        public double                 a;
        public float volt, temp1, temp2;
        public float         ax, ay, az;
        //===================================================================================//



        //================================ Введение массивов ================================//
        public float[] x = new float[150];
        public float[] h = new float[150];
        public float[] t1 = new float[150];
        public float[] t2 = new float[150];
        public float[] aax = new float[150];
        public float[] aay = new float[150];
        public float[] aaz = new float[150];
        //===================================================================================//


        public int counter = 0;



        public Form1()
        {
            InitializeComponent();
            getAvaliablePorts();
        }


        //*************************** Поиск и введение компортов ***************************//
        void getAvaliablePorts()
        {
            String[] ports = SerialPort.GetPortNames();      //Поиск компортов.
            Porty.Items.AddRange(ports);                 //Добавление компортов в комбокс.
            Comander.AppendText("COM порты найдены.\r\n");
        }
        //**********************************************************************************//


        //***************************** Нажатие кнопки "Подключить" ************************//
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Porty.Text == "" || Speeder.Text == "")
                {
                    Comander.AppendText("Пожалуйста, выберите настройки порта.\r\n");
                }
                else
                {
                    serialPort1.PortName = Porty.Text;
                    serialPort1.BaudRate = Convert.ToInt32(Speeder.Text);
                    serialPort1.Open();
                    Connect.Enabled = false;
                    DisConnect.Enabled = true;
                    Send.Enabled = true;
                    Sender.Enabled = true;
                    Comander.AppendText("Подключение произошло успешно.\r\n");
                }
            }
            catch (UnauthorizedAccessException)
            {
                Comander.AppendText("Ошибка!\r\n");
            }
        }

        //***********************************************************************************//



        //**************************** Нажатие кнопки "Отключить" ***************************//
        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Connect.Enabled = true;
            DisConnect.Enabled = false;
            Send.Enabled = false;
            Sender.Enabled = false;
            Comander.AppendText("Устройство отключено.\r\n");
        }
        //************************************************************************************//



        //**************************** Нажатие кнопки "Отправить" ****************************//
        private void Send_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine(Sender.Text);
            Sender.Text = ("");
        }
        //************************************************************************************//



        //**************************** Автоскролл и получение ********************************//

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            if (data.Length < 60)
            {
                data += serialPort1.ReadExisting();
            }
            else
            {
                data = serialPort1.ReadExisting();
                return;
            }

            if (!((data[0] >= 0x30) && (data[0] < 0x40))) {
                Words.Invoke((MethodInvoker)delegate
                {
                    Words.AppendText(data);

                    Words.SelectionStart = Words.Text.Length;
                    if (Autosc.Checked)
                    {
                        Words.ScrollToCaret();
                    }
                });

                data = "";

                return;
            }
                
            if (data.IndexOf(".") == 61)
            {
                //Все ОК
                try
                {
                    str = data.Split(';');

                    //if (str.Length == 9)
                    //{
                    Int32.TryParse(str[0], out tim);

                    temp = str[1].Substring(0, 4);
                    temp1 = Convert.ToSingle(temp);

                    temp = str[2].Substring(0, 4);
                    temp2 = Convert.ToSingle(temp);

                    temp = str[3].Substring(0, 6);
                    Int32.TryParse(temp, out press);

                    temp = str[4].Substring(0, 5);
                    barheight = Convert.ToSingle(temp);

                    ax = Convert.ToSingle(str[5]);
                    ay = Convert.ToSingle(str[6]);
                    az = Convert.ToSingle(str[7]);

                    temp = str[8].Substring(0, 4);
                    volt = Convert.ToSingle(temp);

                    if (counter < 150)
                    {
                        x[counter] = tim;       //Запись значений в массивы.
                        h[counter] = barheight; //Запись значений в массивы.
                        t1[counter] = temp1;    //Запись значений в массивы.
                        t2[counter] = temp2;    //Запись значений в массивы.
                        aax[counter] = ax;    //Запись значений в массивы.
                        aay[counter] = ay;    //Запись значений в массивы.
                        aaz[counter] = az;    //Запись значений в массивы.

                        for (int abc = counter; abc < 150; abc++) {
                            x[abc] = x[counter];
                            h[abc] = h[counter];
                            t1[abc] = t1[counter];
                            t2[abc] = t2[counter];
                            aax[abc] = aax[counter];
                            aay[abc] = aay[counter];
                            aaz[abc] = aaz[counter];
                        }
                    }
                    else
                    {
                        for (int abc = 1; abc < 150; abc++)
                        {
                            x[abc - 1] = x[abc];
                            h[abc - 1] = h[abc];
                            t1[abc - 1] = t1[abc];
                            t2[abc - 1] = t2[abc];
                            aax[abc - 1] = aax[abc];
                            aay[abc - 1] = aay[abc];
                            aaz[abc - 1] = aaz[abc];
                        }
                        counter--;
                        x[counter] = tim;       //Запись значений в массивы.
                        h[counter] = barheight; //Запись значений в массивы.
                        t1[counter] = temp1;    //Запись значений в массивы.
                        t2[counter] = temp2;    //Запись значений в массивы.
                        aax[counter] = ax;    //Запись значений в массивы.
                        aay[counter] = ay;    //Запись значений в массивы.
                        aaz[counter] = az;    //Запись значений в массивы.
                    }


                    //Запись данных
                    this.Invoke((MethodInvoker)delegate
                        {
                            timeee.Text = (Convert.ToString(tim));
                            Davlenie.Text = (Convert.ToString(press));
                            xxx.Text = (Convert.ToString(ax));
                            yyy.Text = (Convert.ToString(ay));
                            zzz.Text = (Convert.ToString(az));
                            tds.Text = (Convert.ToString(temp1));
                            tbmp.Text = (Convert.ToString(temp2));
                            heighttt.Text = (Convert.ToString(barheight));
                            voltageee.Text = (Convert.ToString(volt));

                            // Вывод графиков.
                            chart1.Series[0].Points.DataBindXY(x, h);
                            chart1.Series[1].Points.DataBindXY(x, t1);
                            chart1.Series[2].Points.DataBindXY(x, t2);

                            chart2.Series[0].Points.DataBindXY(x, aax);
                            chart2.Series[1].Points.DataBindXY(x, aay);
                            chart2.Series[2].Points.DataBindXY(x, aaz);

                        });

                    counter++;
                    //}
                }
                catch (Exception ex)
                {
                    Comander.Invoke((MethodInvoker)delegate
                    {
                        Comander.AppendText("Ошибка" + ex.Message + "\r\n");

                        Comander.SelectionStart = Comander.Text.Length;
                        if (Autosc.Checked)
                        {
                            Comander.ScrollToCaret();
                        }
                    });
                }


                //Автопрокрутка поля вордс.
                Words.Invoke((MethodInvoker)delegate
                {
                    Words.AppendText(data);

                    Words.SelectionStart = Words.Text.Length;
                    if (Autosc.Checked)
                    {
                        Words.ScrollToCaret();
                    }
                });


                //Автопрокрутка поля инициализации.
                Comander.Invoke((MethodInvoker)delegate
                {
                    Comander.SelectionStart = Comander.Text.Length;
                    if (Autosc.Checked)
                    {
                        Comander.ScrollToCaret();
                    }
                });
            }
            else
            {
                /*//Автопрокрутка поля вордс.
                Words.Invoke((MethodInvoker)delegate
                {
                    Words.AppendText(data);
                    
                    Words.SelectionStart = Words.Text.Length;
                    if (Autosc.Checked)
                    {
                        Words.ScrollToCaret();
                    }
                });*/

                //Автопрокрутка поля инициализации.
                Comander.Invoke((MethodInvoker)delegate
                {
                    Comander.AppendText(data.Length.ToString() + "\r\n");

                    Comander.SelectionStart = Comander.Text.Length;
                    if (Autosc.Checked)
                    {
                        Comander.ScrollToCaret();
                    }
                });

                if (data.Length > 60)
                {
                    data = data.Substring(31, 30);
                }
            }
        }
        
        
        //************************************************************************************//
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }



        //************************* Сохранение файла телеметрии ******************************//
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, Words.Text);
            MessageBox.Show("Файл сохранен");
        }
        //************************************************************************************//



        //********************************* Вызов помощи *************************************//
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f7 = new Form2();
            f7.ShowDialog();
        }
        //************************************************************************************//



        //******************************** Кнопка "О нас" ************************************//
        private void оНасToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 r = new AboutBox1();
            r.Hide();
            r.Show();
        }
        //************************************************************************************//



        //******************************* Нажатие кнопки ГМ **********************************//
        private void button1_Click(object sender, EventArgs e)
        {
            //Запись данных
            timeee.Text = (Convert.ToString(tim));
            Davlenie.Text = (Convert.ToString(press));
            xxx.Text = (Convert.ToString(ax));
            yyy.Text = (Convert.ToString(ay));
            zzz.Text = (Convert.ToString(az));
            tds.Text = (Convert.ToString(temp1));
            tbmp.Text = (Convert.ToString(temp2));
            heighttt.Text = (Convert.ToString(barheight));
            voltageee.Text = (Convert.ToString(volt));
        }

        //************************************************************************************//



        //**************************** Нажатие кнопки "Отправить" ****************************//
        private void Send_Click_1(object sender, EventArgs e)
        {
                serialPort1.WriteLine(Sender.Text);
                Sender.Text = ("");
        }
        //************************************************************************************//



        //****************************** Очиска первого графика ******************************//
        private void график1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
        }
        //************************************************************************************//



        //****************************** Очиска второго графика ******************************//
        private void график2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();
        }
        //************************************************************************************//



        //********************************** Переход на сайт *********************************//
        private void нашСайтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://dubos.tech");
        }

        //************************************************************************************//




        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {

            }


            //*********************************** Очистка ****************************************//
            private void полеТелеметрииToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Words.Text = ("");
            }

            private void полеИнициализацииToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Comander.Text = ("");
            }
            //************************************************************************************//
            


            //**************************** Нажатие кнопки "Обновить" *****************************//
            private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Porty.Items.Clear();
                String[] ports = SerialPort.GetPortNames();      //Поиск компортов.
                Porty.Items.AddRange(ports);                 //Добавление компортов в комбокс.
            }
            //************************************************************************************//
            


            //************************************************************************************//
            private void pictureBox1_Click(object sender, EventArgs e)
            {

            }

            //************************************************************************************//

        }
    }


