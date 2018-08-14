using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ECHONET_Lite
{
    public class EchonetLite
    {
        private static readonly int Port = 3610;
        private static readonly string MulticastAddress = "224.0.23.0";
        private static readonly UInt16 Header = 0x1081;
        private static readonly UInt16 TransactionID = 0x0000;
        private static readonly byte ControllerGrpCd = 0x05;
        private static readonly byte ControllerClsCd = 0xFF;
        private static readonly byte ControllerInsCd = 0x01;

        /// <summary>
        /// サービスコード
        /// </summary>
        private enum enmESV
        {
            SETI = 0x60,        //セットして応答はいらない
            SETC = 0x61,        //セットしたことを返信してもらう ESV=0x71で返ってくる
            GET = 0x62,         //状態を返してもらう ESV=0x72で返ってくる
            SET_RES = 0x71,     //応答時にセットされるプロパティ0x61が来たとき
            GET_RES = 0x72,     //応答時にセットされるプロパティ0x62が来たとき
            INF = 0x73,         //自身の情報を同一ネットワーク内の全てのコントローラーに通知するときにつける。自発的に送信したとき
        }

        /// <summary>
        /// ECHONETオブジェクトのパラメータ
        /// </summary>
        public class EObject
        {
            public int Number = 0;
            public string IPAddress = "";
            public byte GroupCd = 0;
            public byte ClassCd = 0;
            public byte InstanceCd = 0;
        }

        /// <summary>
        /// ローカルマシンのIPアドレス
        /// </summary>
        public string LocalAddress { get; set; } = "127.0.0.1";

        public EchonetLite()
        {

        }

        /// <summary>
        /// ノード検索用フレームを生成します
        /// </summary>
        /// <returns></returns>
        private byte[] CreateSearchFrame()
        {
            byte[] frame = new byte[14];

            frame[0] = (byte)(Header >> 8);         //EHD1 ECHONETLiteヘッダ  1Byte   0x10固定
            frame[1] = (byte)(Header & 0xFF);       //EHD2 ECHONETLiteヘッダ  1Byte   0x81固定

            frame[2] = (byte)(TransactionID >> 8);  //TID  トランザクションID 2Byte   0～65535
            frame[3] = (byte)(TransactionID & 0xFF);//TID  トランザクションID 2Byte   0～65535

            frame[4] = ControllerGrpCd;             //SEOJ 送信元オブジェクト コントローラークラスグループコード
            frame[5] = ControllerClsCd;             //SEOJ 送信元オブジェクト コントローラークラスコード
            frame[6] = ControllerInsCd;             //SEOJ 送信元オブジェクト インスタンスコード

            frame[7] = (byte)0x0E;                  //DEOJ 送信先オブジェクト 一覧検索時のクラスグループコード
            frame[8] = (byte)0xF0;                  //DEOJ 送信先オブジェクト 一覧検索時のクラスコード
            frame[9] = (byte)0x01;                  //DEOJ 送信先オブジェクト インスタンスコード

            frame[10] = (byte)enmESV.GET;           //ESV  サービスコード     Get

            frame[11] = (byte)0x01;                 //OPC  プロパティ数
            frame[12] = (byte)0xD6;                 //EPC  プロパティ番号     ノードリストの検索
            frame[13] = (byte)0x00;                 //PDC  EDTのバイト数      EDTが無いので 0

            return frame;
        }

        /// <summary>
        /// オブジェクトを探します
        /// ただし、このプログラムは1つだけしか取得しません
        /// </summary>
        /// <returns></returns>
        public List<EObject> SearchObject(ref string err)
        {
            err = "";

            //ノード一覧を取得するためにマルチキャストします
            using (UdpClient udpClient = new UdpClient(AddressFamily.InterNetwork))
            {
                IPAddress address = IPAddress.Parse(MulticastAddress);
                IPEndPoint ipEndPoint = new IPEndPoint(address, Port);

                try
                {
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
                    udpClient.JoinMulticastGroup(address, IPAddress.Parse(LocalAddress));
                    udpClient.MulticastLoopback = true;

                    byte[] frame = CreateSearchFrame();
                    udpClient.Send(frame, frame.Length, ipEndPoint);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    Debug.WriteLine(e.Message);
                    err = e.Message;
                }
            }
            //udpClient.Close();

            //マルチキャストの返信がユニキャストで来るので取得します
            byte[] recvBytes = null;
            IPEndPoint remoteEP = null;
            using (UdpClient udp = new UdpClient(Port))
            {
                try
                {
                    udp.Client.ReceiveTimeout = 5000;   //5secでタイムアウト
                    recvBytes = udp.Receive(ref remoteEP);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    Debug.WriteLine(e.Message);
                    err = e.Message;
                }
            }
            //udp.Close();

            if (recvBytes == null || remoteEP == null)
            {
                //うまく取得できませんでした
                return null;
            }

            for (int i = 0; i < recvBytes.Length; i++)
            {
                Debug.Write(recvBytes[i].ToString("X2") + " ");
            }
            Debug.WriteLine("");


            List<EObject> list = new List<EObject>();
            string ipaddress = remoteEP.Address.ToString();

            if (recvBytes.Length < 14)
            {
                //データがおかしい
                return list;
            }

            int len = recvBytes[13];
            int nodeCnt = recvBytes[14];

            if (nodeCnt * 3 + 1 != len)
            {
                //データがおかしい
                return list;
            }

            //取得したノードをlistに入れます
            for (int i = 0; i < nodeCnt; i++)
            {
                EObject eo = new EObject();
                eo.Number = i + 1;
                eo.IPAddress = ipaddress;
                eo.GroupCd = recvBytes[15 + i * 3];
                eo.ClassCd = recvBytes[15 + i * 3 + 1];
                eo.InstanceCd = recvBytes[15 + i * 3 + 2];
                list.Add(eo);
            }
            return list;
        }

        /// <summary>
        /// スイッチ用のフレームを生成します
        /// </summary>
        /// <returns></returns>
        private byte[] CreateSwitchFrame(EObject eo, bool sw)
        {
            byte[] frame = new byte[15];

            frame[0] = (byte)(Header >> 8);         //EHD1 ECHONETLiteヘッダ  1Byte   0x10固定
            frame[1] = (byte)(Header & 0xFF);       //EHD2 ECHONETLiteヘッダ  1Byte   0x81固定

            frame[2] = (byte)(TransactionID >> 8);  //TID  トランザクションID 2Byte   0～65535
            frame[3] = (byte)(TransactionID & 0xFF);//TID  トランザクションID 2Byte   0～65535

            frame[4] = ControllerGrpCd;             //SEOJ 送信元オブジェクト コントローラークラスグループコード
            frame[5] = ControllerClsCd;             //SEOJ 送信元オブジェクト コントローラークラスコード
            frame[6] = ControllerInsCd;             //SEOJ 送信元オブジェクト インスタンスコード

            frame[7] = eo.GroupCd;                  //DEOJ 送信先オブジェクト グループコード
            frame[8] = eo.ClassCd;                  //DEOJ 送信先オブジェクト ラスコード
            frame[9] = eo.InstanceCd;               //DEOJ 送信先オブジェクト インスタンスコード

            frame[10] = (byte)enmESV.SETC;           //ESV  サービスコード     SETC

            frame[11] = (byte)0x01;                 //OPC  プロパティ数
            frame[12] = (byte)0x80;                 //EPC  プロパティ番号     スイッチのオンオフ
            frame[13] = (byte)0x01;                 //PDC  EDTのバイト数
            frame[14] = sw ? (byte)0x30 : (byte)0x31;   //0x30がスイッチオン 0x31がスイッチオフ

            return frame;
        }

        public byte[] SwitchSet(EObject eo, bool sw)
        {
            byte[] sendFrame = CreateSwitchFrame(eo, sw);   //スイッチ用のフレームを取得します
            byte[] recvBytes = null;

            IPEndPoint remoteEP = null;
            using (UdpClient udp = new UdpClient(Port))
            {
                udp.Client.ReceiveTimeout = 5000;   //5secでタイムアウト
                try
                {
                    udp.Send(sendFrame, sendFrame.Length, eo.IPAddress, Port);

                    recvBytes = udp.Receive(ref remoteEP);
                }
                catch (Exception ee)
                {
                    Debug.WriteLine(ee.StackTrace);
                    Debug.WriteLine(ee.Message);
                }
            }
            //udp.Close();

            //受信したデータと送信者の情報を表示する
            Debug.WriteLine("送信元アドレス:{0}/ポート番号:{1}", remoteEP.Address, remoteEP.Port);
            for (int j = 0; j < recvBytes.Length; j++)
            {
                Debug.Write(recvBytes[j].ToString("X2") + " ");
            }
            Debug.WriteLine("");

            return recvBytes;
        }
    }
}
 