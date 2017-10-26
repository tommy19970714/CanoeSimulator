using UnityEngine;
using System.IO.Ports;
using System.Text;

public class Serial : MonoBehaviour
{
	void Update() { }

	public Vector3 input;
	public SerialPort port;

	public string com;
	public int baudrate = 38400;

	public string message;
	public bool done = false;
	public Vector3 h = new Vector3(0, 0, 0);

	// Use this for initialization
	//シリアル通信の開始、シリアルからの出力を取得
	void Awake()
	{
        com = "COM4";//PlayerPrefs.GetString("serialCom");
		//baudrate = PlayerPrefs.GetInt("serialBoadrate");

		port = new SerialPort(com, baudrate, Parity.None, 8, StopBits.One);
		try
		{
			port.Open();
		}
		catch
		{
			Debug.Log("ポートを開けません");
		}
		done = true;
	}

	//16進数に変換関数
	//引数の制御値を16進数に変換し返す
	string hexconverter(Vector3 temp)
	{
		string[] output = new string[3];
		StringBuilder sb = new StringBuilder();
		output[0] = ((int)temp.x).ToString("X4");
		output[1] = ((int)temp.y).ToString("X4");
		output[2] = ((int)temp.z).ToString("X4");
		//appendははやい
		for (int i = 0; i < 3; i++) sb.Append(output[i]);
		sb.Append("\r\n");
		return sb.ToString();
	}

	//hexconverter()を用いてマイコンに制御値を出力する
	public void dumphex(Vector3 temp)
	{
		checkSend(hexconverter(temp));
	}

	//初期位置に戻す
	public void dumpinitialize()
	{
		Debug.LogWarning("Intiration");
		for (int i = 0; i<10; i++) checkSend("I");
	}

	public void dumpUp()
	{
		Debug.LogWarning("Up");
		checkSend("U");
	}

	public void dumpoparation()
	{
		Debug.LogWarning("Operation");
		for (int i = 0; i < 10; i++) checkSend("O");
	}

	//機械の動作を停止させる
	public void dumpstop()
	{
		Debug.LogWarning("Stop");
		for (int i = 0; i < 10; i++) checkSend("S");
	}

	public void kattun()
	{
		//かっつんモードに変更する(調整用)
		checkSend("K");
	}

	//機械の動作を停止させる
	public void space()
	{
		checkSend(" ");
	}

	public void dumpKattuncontrol(int motor, int duty)
	{
		if (motor != 0 && motor != 1 && motor != 2)
		{
			Debug.Log("motor番号が間違っています");
			return;
		}
		if (duty < -200 || duty > 100)
		{
			Debug.Log("デュティー比の値が間違っています。");
			return;
		}

		string sendStr = motor.ToString() + duty.ToString() + "\r\n";
		checkSend(sendStr);
		Debug.Log(sendStr);
		Debug.Log(motor.ToString() + duty.ToString() + "\r\n");
	}

	public void test()
	{
		string testest = hexconverter(input);

		Debug.Log("(" + input.x + "," + input.y + "," + input.z + ")");
		Debug.Log(testest);
		return;
	}

	//マイコンからの入力を読み取る
	public void read()
	{
		Debug.Log("Read1");
		if (done && port != null && port.IsOpen)
		{
			Debug.Log("Read2");
			try
			{
				message = port.ReadLine();
				Debug.Log(message);
				//4096 = 16^3
				h.x = (float)(int.Parse(message.Substring(0, 4), System.Globalization.NumberStyles.HexNumber)) / 4096.00f * 100;
				h.y = (float)(int.Parse(message.Substring(4, 4), System.Globalization.NumberStyles.HexNumber)) / 4096.00f * 100;
				h.z = (float)(int.Parse(message.Substring(8, 4), System.Globalization.NumberStyles.HexNumber)) / 4096.00f * 100;
				Debug.Log(h);
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}
	}

	public void close()
	{
		checkClose ();
	}

	void OnApplicationQuit()
	{
		checkSend ("S");
		checkClose ();
	}

	void OnDestroy()
	{
		checkClose ();
	}

	void checkSend(string sendStr)
	{
		if (port.IsOpen)
		{
			port.Write(sendStr);
		}
		else
		{
			try
			{
				port.Open();
				//再度信号を送る
				port.Write(sendStr);
			}
			catch { Debug.Log("serialのポートを開けません"); }
		}
	}

	void checkClose()
	{
		if (port.IsOpen)
		{
			try
			{
				Debug.Log("serialのポートを閉じました。");
				port.Close(); 
			}
			catch { Debug.Log("serialのポートを閉じることができません"); }
		}
	}
}