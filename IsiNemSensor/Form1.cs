using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace IsiNemSensor
{
    public partial class Form1 : Form
    {
        String sicaklik;
        String nem;
        String cihaz_id;
        String sistem_id;
        string RxString;
        public Form1()
        {
            InitializeComponent();
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            
            this.Invoke(new EventHandler(DisplayText));
            
        }

        private void rest_yaz(String sicaklik_, String nem_, String cihaz_id_, String sistem_id_)
        {
            try { 
                var client = new RestClient("http://193.140.154.92:3000/sicaklikAl");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{""sicaklik"" : """+sicaklik_+@""",""nem"" : """+nem_+@""",""cihaz_id"" :"""+cihaz_id_+@""",""sistem_id"" :"""+sistem_id_+@"""}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        
        
        private void DisplayText(object sender, EventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            string[] words = RxString.Split(',');
            int i = 0;
            foreach (string word in words)
            {
               // richTextBox1.AppendText(word+Environment.NewLine);
                if (i == 0 && !words[0].Equals(""))
                {
                    richTextBox1.AppendText("Nem: " + words[0]+ Environment.NewLine);
                    nem = words[0];
                    i++;
                }
                else if (i == 1)
                {
                    richTextBox1.AppendText("Isı: " + words[1]+ Environment.NewLine);
                    sicaklik = words[1];
                }
            }
            try
            {

                if (i == 1) { rest_yaz(sicaklik, nem, "25", "14"); }
            }
           catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           // richTextBox1.AppendText("Nem: "+words[0]+ " Isı: "+ words[1] + Environment.NewLine);
           // Console.WriteLine(RxString);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
                try
                {
                    serialPort1.Open();
                    Console.WriteLine("opened");

                }

                catch (Exception oex)
                {
                    MessageBox.Show(oex.ToString());
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close(); 
            Console.WriteLine("closed");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

    }
}
