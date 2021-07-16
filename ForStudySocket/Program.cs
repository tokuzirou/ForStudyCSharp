using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ForStudySocket
{
    class Program
    {
        static async Task Main()
        {
            //ネットワーク情報を設定(任意のIPでポート9999)←今回はlocalhostしか使えないので、127.0.0.1になる
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9999);
            //ソケットインスタンスを生成
            //アドレスファミリ：Ipv4のアドレス
            //ソケットタイプ：ストリーム
            //プロトコルタイプ：TCP
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //サーバーソケットをサーバーが開くIPとポートに紐づける
            server.Bind(ipep);
            //サーバーソケットをクライアントからのConnectに備えて、待機する
            server.Listen(20);
            //この時点から、サーバーが立ち上がる
            Console.WriteLine("サーバースタート(ポート9999)");
            try
            {
                //サーバーが通信切断するまで、無限ループ
                while (true)
                {
                    //クライアントソケットを接続要求があれば、別のワーカースレッドでインスタンス化する
                    //接続要求ごとにインスタンス化できる
                    //接続が確認できるまで待つ
                    Socket client = await Task.Run(() =>
                    {
                        return server.Accept();
                    });

                    //上のTaskのコレクションを作成して、Palarell処理でもよさそう
                    //別のワーカースレッドでクライアントソケットの接続後処理をする
                    //１回の接続処理が終わるまで待つ
                    try
                    {
                        //RemoteEndpointをIPEndPointにアップキャスト可能な場合
                        if (client.RemoteEndPoint is IPEndPoint clientEndPoint)
                        {
                            //接続先表示
                            Console.WriteLine($"Client: (From {clientEndPoint.Address}:{clientEndPoint.Port}, ConnectionTime: {DateTime.Now})");
                        }
                        else
                        {
                            throw new Exception("型が異なります");
                        }

                        try
                        {
                            //ローディングメッセージ
                            //\0は改行
                            string msg = "Welcome";
                            //string型→byte[]型にエンコード
                            byte[] b = Encoding.ASCII.GetBytes(msg);
                            //クライアントにバイト列メッセージ送信
                            await Task.Run(() =>
                            {
                                client.Send(b);
                            });

                            //体裁
                            Console.Write(">");
                            //クライアントとの接続が維持されている間
                            while (client.Connected)
                            {
                                //クライアントから送られてくるデータのバッファー
                                byte[] listenBuffer = new byte[1024];
                                //データ受信
                                await Task.Run(() =>
                                {
                                    client.Receive(listenBuffer);
                                });
                                //受信バッファーを文字列情報に変換
                                string msga = Encoding.Default.GetString(listenBuffer);
                                //サーバーコンソールに表示
                                Console.Write(msga + "\r\n>");

                                //データ送信
                                byte[] baa = Encoding.Default.GetBytes(msga);
                                //受信バッファをクライアントに送信
                                await Task.Run(() =>
                                {
                                    client.Send(baa);
                                });
                            }
                        }
                        //Client側から強制切断があった場合
                        catch (SocketException sockex)
                        {
                            Console.WriteLine(sockex.Message);
                        }
                        //クライアント接続解除
                        finally
                        {
                            client.Disconnect(false);
                            Console.WriteLine("クライアント接続終了");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            //サーバーに何か異常がある場合
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //サーバーの終了
            finally
            {
                Console.WriteLine("サーバーを閉じます");
                server.Disconnect(false);
            }
        }
    }
}
