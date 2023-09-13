
using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserSensor : MonoBehaviour
{
    public event Action<Laser> onLaserAdded;
    public event Action<Laser> onLaserRemoved;

	public event Action onStruck;
	public event Action onUnstruck;

    List<Laser> strikingLasers;

	private void Awake()
	{
		strikingLasers = new List<Laser>();
	}

	public static void HandleLaser(Laser laser, LaserSensor prev, LaserSensor current)
    {
        if (prev != null)
            prev.RemoveLaser(laser);

		if (current != null)
			current.AddLaser(laser);
	}

    void AddLaser(Laser strikingLaser)
    {
        strikingLasers.Add(strikingLaser);
        onLaserAdded?.Invoke(strikingLaser);
		if (strikingLasers.Count == 1)
			onStruck?.Invoke();
	}

	void RemoveLaser(Laser unstrikingLaser)
	{
		strikingLasers.Remove(unstrikingLaser);
		onLaserRemoved?.Invoke(unstrikingLaser);
		if (strikingLasers.Count == 0)
			onUnstruck?.Invoke();
	}
}
