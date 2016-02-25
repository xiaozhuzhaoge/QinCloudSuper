using UnityEngine;
using System.Collections;
using System;

public class SoundConfig : ConfigMode
{
	public int id;
	public int type;
	public int priority;
	public int wholisten;
	public int limit;
	public float volume;
	public string resId;
	public bool ifCircle;
	public float time;
	public float interval;

    #region IConfig implementation

	public SoundConfig ()
	{
	}

	public SoundConfig (SimpleJson.JsonObject o)
	{
		Init (o);
	}

	public override void Init (SimpleJson.JsonObject o)
	{
        base.Init(o);
        this.id = Convert.ToInt32(o["id"]);   
		this.type = Convert.ToInt32 (o ["type"]);        
        this.priority = Convert.ToInt32 (o ["priority"]);
		this.wholisten = Convert.ToInt32 (o ["wholisten"]);
		this.limit = Convert.ToInt32 (o ["limit"]);
		this.volume = Convert.ToInt32 (o ["volume"]) / 100f;
		this.resId = Convert.ToString (o ["resId"]);
		this.ifCircle = Convert.ToInt32 (o ["ifCircle"]) == 1;
		this.time = Convert.ToInt32 (o ["time"]) / 1000f;
		this.interval = Convert.ToInt32 (o ["interval"]) / 1000f;
	}

    #endregion


}
