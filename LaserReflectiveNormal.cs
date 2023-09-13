using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflectiveNormal : MonoBehaviour, ILaserReflective
{
	public void Reflect(Laser laser, Ray incomingRay, RaycastHit hitInfo)
	{
		var outgoingDirection = hitInfo.normal;
        laser.CastBeam(hitInfo.point, outgoingDirection);
	}
}
