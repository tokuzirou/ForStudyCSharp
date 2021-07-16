using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ForStudySocketClient
{
    class Program
    {
        static async Task Main()
        {
            //IP入力待ち
            string a = Console.ReadLine();
            IPEndPoint ipep = default;
            try
            {
                //string型→IPAddress型に変換
                if (IPAddress.TryParse(a, out IPAddress iPAddress))
                {
                    //IPEndPointオブジェクトをインスタンス化
                    ipep = new IPEndPoint(iPAddress, 9999);
                }
                else
                {
                    throw new Exception("IPに誤りがあります");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //tryの中身をメソッド化したのを再起処理
                //メソッドの中でエラー処理すれば、良い
            }

            //クライアントソケットインスタンスを生成
            //アドレスファミリ：Ipv4のアドレス
            //ソケットタイプ：ストリーム
            //プロトコルタイプ：TCP
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //サーバーに接続
            client.Connect(ipep);

            Console.WriteLine("接続完了(ポート9999)");
            //接続後処理
            try
            {
                //RemoteEndpointをIPEndPointにアップキャスト可能な場合
                if (client.RemoteEndPoint is IPEndPoint serverEndPoint)
                {
                    Console.WriteLine($"接続状況{Environment.NewLine}" +
                                        $"Address:{serverEndPoint.Address}{Environment.NewLine}" +
                                        $"Port:{serverEndPoint.Port}");
                }
                else
                {
                    throw new Exception("型が異なります");
                }

                try
                {
                    //サーバーからのメッセージを待つ
                    while (true)
                    {
                        //メッセージ受信処理
                        //サーバーから送られてくるデータのバッファー
                        byte[] listenBuffer = new byte[1024];
                        //Recieveはブロッキングメソッドのため、別ワーカースレッドで処理
                        await Task.Run(() =>
                        {
                            client.Receive(listenBuffer);
                        });
                        //受信バッファーを文字列情報に変換
                        string msga = Encoding.Default.GetString(listenBuffer);
                        //最後の受信メッセージが>で終了した場合
                        Console.Write(">" + msga + "\r\n>");

                        //メッセージ送信処理
                        //サーバーに送信するメッセージ入力待機待ち(止まるはず)
                        string msgaa = Console.ReadLine();
                        //exitが入力された場合
                        if (msgaa == "exit")
                        {
                            client.Disconnect(false);
                            break;
                        }
                        //string型→byte[]にエンコード
                        byte[] ba = Encoding.ASCII.GetBytes(msgaa);
                        //サーバーにバイト列メッセージを送信
                        await Task.Run(() =>
                        {
                            client.Send(ba);
                        });
                    }
                }
                //サーバー側から強制切断があった場合
                catch (SocketException sockex)
                {
                    Console.WriteLine(sockex.Message);
                }
                finally
                {
                    Console.WriteLine("接続終了");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
