// KK
using UnityEngine;
using System.Collections;
using System;
using System.Timers;
using Pomelo.DotNetClient;
using SimpleJson;
using UnityEngine.Internal;

public class HeartBeat
{
	int interval;
	public int timeout;
	Timer timer;
	DateTime lastTime;
	bool isConnected;

	int lastTime2;

	PomeloClient pc;

	float nowTime;

	public bool IsConnected
	{
		get {
			return isConnected;
		}
	}

	public int Interval
	{
		get {
			return interval;
		}
	}

	public HeartBeat(int interval, PomeloClient net){
		this.interval = interval*1000;
		isConnected = true;
		pc = net;

		Start (Time.time);
	}
	
	public void ResetTimeout2(){
		this.timeout = 0;
		lastTime = DateTime.Now;
	}

	public void ResetTimeout(){
		this.timeout = 0;
		lastTime2 = GetGameTime();
	}
	
	public void SendHeartBeat(object source, ElapsedEventArgs e){
		//check timeout
		if(timeout > interval*3){
			UnityEngine.Debug.Log("======heart beat time out.======");
			//protocol.getPomeloClient().disconnect();
			isConnected = false;
			Stop ();
			return;
		}
		
		TimeSpan span = DateTime.Now - lastTime;
		timeout += (int)span.TotalMilliseconds;

		//Send heart beat
		//Debug.Log("===SendHeartBeat===");
		//pc.request("data.dataHandler.heartbeat", (ret)=>{
		pc.request("connector.entryHandler.heartbeat", (ret)=>{
			//Debug.Log("SendHeartBeat ret=>"+ret.ToString());
			if (ret.ContainsKey("status"))
			{
				int status = Convert.ToInt32 (ret["status"]);
				if( status == 0 ) {
					ResetTimeout();
				}
			}
		});
	}

	public void Tick2()
	{
		UpdateNowTime(Time.time);

		//check timeout
		if(timeout > interval*3){
			UnityEngine.Debug.Log("======heart beat time out.======");
			//protocol.getPomeloClient().disconnect();
			isConnected = false;
			//Stop ();
			return;
		}
		
		TimeSpan span = DateTime.Now - lastTime;
		timeout += (int)span.TotalMilliseconds;

		if( pc == null ) 
			return;
		
		//Send heart beat
		//Debug.Log("===SendHeartBeat===");
		//pc.request("data.dataHandler.heartbeat", (ret)=>{
		pc.request("connector.entryHandler.heartbeat", (ret)=>{
			//Debug.Log("SendHeartBeat ret=>"+ret.ToString());
			if (ret.ContainsKey("status"))
			{
				int status = Convert.ToInt32 (ret["status"]);
				if( status == 0 ) {
					ResetTimeout();
				}
			}
		});
	}

	public void Tick()
	{
		UpdateNowTime(Time.time);

		//check timeout
		if(timeout > interval*3){
			UnityEngine.Debug.Log("======heart beat time out.======");
			//protocol.getPomeloClient().disconnect();
			isConnected = false;
			//Stop ();
			return;
		}
		
		timeout += (GetGameTime() - lastTime2);
		
		//Send heart beat
		//Debug.Log("===SendHeartBeat===");
		//pc.request("data.dataHandler.heartbeat", (ret)=>{
		pc.request("connector.entryHandler.heartbeat", (ret)=>{
			//Debug.Log("SendHeartBeat ret=>"+ret.ToString());
			if (ret.ContainsKey("status"))
			{
				int status = Convert.ToInt32 (ret["status"]);
				if( status == 0 ) {
					ResetTimeout();
				}
			}
		});
	}

	public void UpdateNowTime(float now)
	{
		nowTime = now;
	}
	public int GetGameTime()
	{
		return (int)(nowTime * 1000f);
	}
	
	public void Start(float now){
		if(interval < 1000) return;
		
		//start hearbeat
//		this.timer = new Timer();
//		timer.Interval = interval;
//		timer.Elapsed += new ElapsedEventHandler(SendHeartBeat);
//		timer.Enabled = true;
		
		//Set timeout
		timeout = 0;
		lastTime = DateTime.Now;

		UpdateNowTime(now);
		lastTime2 = GetGameTime();

		isConnected = true;

		Debug.Log("===================HeartBeat Starting===================");
	}
	
	public void Stop(){
		if(this.timer != null) {
			this.timer.Enabled = false;
			this.timer.Dispose();
		}
		isConnected = false;

		Debug.Log("===================HeartBeat Stoped===================");
	}

}
