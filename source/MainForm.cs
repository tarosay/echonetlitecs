using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECHONET_Lite
{
    public partial class MainForm : Form
    {
        private static List<EchonetLite.EObject> EObjects = null;
        private static string LocalIP = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            nudNumber.Enabled = false;
            btnON.Enabled = false;
            btnOFF.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox2.Enabled = false;
            nudNumber.Enabled = false;
            btnON.Enabled = false;
            btnOFF.Enabled = false;
            textBox2.Text = "";

            if (LocalIP == "")
            {
                textBox1.AppendText("DHCPで取得した自分のIPアドレスを読み込み中\r\n");
                LocalIP = GetLocalIPV4Address("DHCP");
                textBox1.AppendText("ローカルIPアドレス : " + LocalIP + "\r\n");
            }

            EchonetLite echonetLite = new EchonetLite();
            echonetLite.LocalAddress = LocalIP;

            string errmsg = "";

            EObjects = echonetLite.SearchObject(ref errmsg);

            textBox1.AppendText(errmsg + "\r\n");

            if (EObjects != null)
            {
                textBox1.AppendText("検索結果\r\n");

                if (EObjects.Count > 0)
                {
                    textBox1.AppendText("応答 IPアドレス: " + EObjects[0].IPAddress + "\r\n");
                    textBox2.Text = EObjects[0].GroupCd.ToString("X2") + EObjects[0].ClassCd.ToString("X2");
                }

                string name = "ノード : ";
                string str = "";
                int min = 0;
                int max = 0;

                for (int i = 0; i < EObjects.Count; i++)
                {
                    str = EObjects[i].GroupCd.ToString("X2") + EObjects[i].ClassCd.ToString("X2");
                    str += EObjects[i].InstanceCd.ToString("X2") + "\r\n";
                    textBox1.AppendText(name + str);

                    max = max < EObjects[i].InstanceCd ? EObjects[i].InstanceCd : max;
                    min = min > EObjects[i].InstanceCd ? EObjects[i].InstanceCd : min;
                }

                textBox2.Enabled = true;
                nudNumber.Enabled = true;
                nudNumber.Maximum = max;
                nudNumber.Minimum = min;
                btnON.Enabled = true;
                btnOFF.Enabled = true;
                nudNumber.Focus();
            }

            button1.Enabled = true;
        }

        private void OnOff(bool sw)
        {
            EchonetLite.EObject eo = new EchonetLite.EObject();

            foreach (EchonetLite.EObject item in EObjects)
            {
                if (item.InstanceCd == (byte)nudNumber.Value)
                {
                    eo.Number = item.Number;
                    eo.IPAddress = item.IPAddress;
                    eo.GroupCd = item.GroupCd;
                    eo.ClassCd = item.ClassCd;
                    eo.InstanceCd = item.InstanceCd;
                    break;
                }
            }

            EchonetLite echonetLite = new EchonetLite();

            echonetLite.SwitchSet(eo, sw);
        }

        private void btnON_Click(object sender, EventArgs e)
        {
            btnON.Enabled = false;
            OnOff(true);
            btnON.Enabled = true;
            btnOFF.Focus();
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            btnOFF.Enabled = false;
            OnOff(false);
            btnOFF.Enabled = true;
            btnON.Focus();
        }

        /// <summary>
        /// DHCPからもらったIPV4のIPアドレスを取得します
        /// </summary>
        /// <returns></returns>
        private string GetLocalIPV4Address(string type)
        {
            IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipentry.AddressList)
            {
                //IPV4のアドレスか？
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    if (type == "DHCP")
                    {
                        //IPHostEntryオブジェクトを取得
                        IPHostEntry ipHost = Dns.GetHostEntry(ip.ToString());

                        if (ipHost.HostName.IndexOf(".") > 0)
                        {
                            //.○○.○○.○○とあったらDHCPからもらったアドレスだと思われる
                            return ip.ToString();
                        }
                    }
                    else
                    {
                        return ip.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}
