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

namespace PracticaTemperatura
{
    public partial class Form1 : Form
    {
        delegate void SetTextDelegate(string value);
        public SerialPort ArduinoPort { 
            get; 
        } 
        public Form1()
        {
            InitializeComponent();
            ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort.PortName = "COM12"; //Checar su equipo
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 500;
            ArduinoPort.WriteTimeout = 500;
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            //ArduinoPort.Open();

            //vincular eventos

            this.button1.Click += button1_Click;

        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
            //lbTem.Text=indata;
            EscribirTxt(dato);
        }
        private void EscribirTxt(string dato)
        {
            if(InvokeRequired)
                try
                {
                    Invoke(new SetTextDelegate(EscribirTxt), dato);
                }
                catch { }
            else
                label3.Text = dato;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            //button2.Enabled=false;
            try
            {
                if (!ArduinoPort.IsOpen)
                    ArduinoPort.Open();
                if(int.TryParse(textBox1.Text,out int temperatureLimit))
                {
                    //Convierte el valor una cadena y luego en un arreglo de bytes
                    string limitString = temperatureLimit.ToString();
                    ArduinoPort.Write(limitString);
                }
                else
                {
                    MessageBox.Show("Ingresar un valor numerico valido en el TextBox del limite de la temperatura");
                }
                label4.Text = "Conexion OK";
                label4.ForeColor = System.Drawing.Color.Lime;
            }
            catch
            {
                MessageBox.Show("Configure el puerto de comunicacion correcto o Desconecte");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled=false;
            if (ArduinoPort.IsOpen)
                ArduinoPort.Close();
            label4.Text = "Desconecto";
            label4.ForeColor= System.Drawing.Color.Red;
            label3.Text = "00.0";

        }
    }
}
