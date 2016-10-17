using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Interfaces;
using SCDB_API;

namespace SCBD_Client
{
    public partial class Form1 : Form
    {
        public IScdbClient client = new Client();

        public Form1()
        {
            InitializeComponent();
            client.Connection = System.Configuration.ConfigurationManager.AppSettings["address"] + ":" + System.Configuration.ConfigurationManager.AppSettings["port"];
            if (client.Connect())
            {
                dataGridView1[0, 0].Value = "Connection succesful!";
            }
            else
            {
                dataGridView1[0, 0].Value = "Couldn´t connect to the database!";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                bool state = client.State(textBox1.Text);
                if (!client.IsConnected)
                {
                    dataGridView1[0, 0].Value = "Not connected to the database!";
                    return;
                }
                if (state)
                {
                    dataGridView1[0, 0].Value = "Opperation succesful!";
                }
                else
                {
                    dataGridView1[0, 0].Value = "Something went wrong!";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (!client.IsConnected)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, 0].Value = "Not connected to the database!";
                    return;
                }
                List<string> result = client.Ask(textBox2.Text);

                if (result != null)
                {
                    int count = 0;
                    foreach (var variable in result)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1[0, count].Value = variable;
                        count++;
                    }
                }
                else
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, 0].Value = "Something went wrong!";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();

            if (client.Connect())
            {
                dataGridView1[0, 0].Value = "Connection succesful!";
            }
            else
            {
                dataGridView1[0, 0].Value = "Couldn´t connect to the database!";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();

            if (client.Connect())
            {
                dataGridView1[0, 0].Value = "Connection succesful!";
            }
            else
            {
                dataGridView1[0, 0].Value = "Couldn´t connect to the database!";
            }
        }
    }
}
