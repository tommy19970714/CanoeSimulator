using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPServer : MonoBehaviour
{
	public UdpClient udp;
	public RealTimeDataManager realtimedata;
	public RealTimeController realtimeControl;

	private Thread thread;
	public string localIpString = "192.168.1.240";
    public int localPort = 22227;
	public bool serverEnabled = true;
	public object lockobject;

	void Start()
	{
		//localIpString = PlayerPrefs.GetString ("realtimeUdpIp", "192.168.1.240");
		//localPort = PlayerPrefs.GetInt ("realtimeUdpPort", 22227);
		server();
		lockobject = new object();
	}

	public void server()
	{
		//バインドするローカルIPとポート番号
		IPAddress localAddress = IPAddress.Parse(localIpString);
		IPEndPoint localEP = new IPEndPoint(localAddress, localPort);
		thread = new Thread(new ThreadStart(ThreadMethod));
		try {
			udp = new UdpClient(localEP);
			thread.Start();
			serverEnabled = true;
            //UdpClientを作成し、ローカルエンドポイントにバインドする
        } catch {
			Debug.LogError("udpのポートを開放できません。");
		}
	}

	public void threadrestart()
	{
		serverEnabled = true;
		IPAddress localAddress = IPAddress.Parse(localIpString);
		IPEndPoint localEP = new IPEndPoint(localAddress, localPort);
		thread = new Thread(new ThreadStart(ThreadMethod));
		try {
			udp = new UdpClient(localEP);
			thread.Start();
			//UdpClientを作成し、ローカルエンドポイントにバインドする
		} catch {
			Debug.LogError("udpのポートを開放できません。");
		}
	}

	public void threadstop()
	{
		serverEnabled = false;
		if (thread.IsAlive)
		{
			thread.Abort();
		}
		try
		{
			udp.Close();
		}
		catch
		{
			Debug.LogError("udpのポートを閉じることができません。");
		}

		realtimedata.resetdata();
	}

	void OnApplicationQuit()
	{
		if (thread.IsAlive)
		{
			thread.Abort();
		}
		try {
			udp.Close();
		} catch {
			Debug.LogError("udpのポートを閉じることができません。");
		}
	}

	private void ThreadMethod()
	{
		/*一回目の受信したデータは壊れている可能性が高いため、捨てる*/
		IPEndPoint remoteEP = null;
		udp.Receive(ref remoteEP);
		udp.Receive(ref remoteEP);
		Debug.Log ("udp start");
		while(serverEnabled == true)
		{
			//データを受信する
			remoteEP = null;
			byte[] rcvBytes = udp.Receive(ref remoteEP);
			//データを文字列に変換する
			string rcvMsg = Encoding.UTF8.GetString(rcvBytes);
			if (rcvMsg == ("exit") )
			{
				Debug.Log ("exit");
				realtimeControl.started = false;
				break;
			}
			lock(lockobject)
			{
				realtimedata.getdata_realtime(rcvMsg); //受信したデータをキューに格納する
			}
			realtimeControl.started = true;
			//"exit"を受信したら終了
		}
	}
}