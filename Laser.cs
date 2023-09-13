
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
	[SerializeField] LaserRendererSettings laserRendererSettings;

    Vector3 sourcePosition;
    float farDistance = 1000f;
	List<Vector3> bouncePositions;
	int maxBounces = 100;

	LaserSensor prevStruckLaserSensor = null;


	void Awake()
    {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
        laserRendererSettings.Apply(lineRenderer);
	}

    
    void FixedUpdate()
    {
		sourcePosition = transform.position + transform.forward * 0.2501f;
		bouncePositions = new List<Vector3>() { sourcePosition };
		CastBeam(sourcePosition, transform.forward);

		lineRenderer.positionCount = bouncePositions.Count;
		lineRenderer.SetPositions(bouncePositions.ToArray());
	}


	public void CastBeam(Vector3 origin, Vector3 direction)
	{
		if (bouncePositions.Count > maxBounces)
			return;

		var ray = new Ray(origin, direction);

		bool didHit = Physics.Raycast(ray, out RaycastHit hitInfo, farDistance);

		if (!didHit)
		{
			var endPoint = origin + direction * farDistance;
			bouncePositions.Add(endPoint);
			if (prevStruckLaserSensor != null)
			{
				LaserSensor.HandleLaser(this, prevStruckLaserSensor, null);
				prevStruckLaserSensor = null;
			}
			return;
		}

		bouncePositions.Add(hitInfo.point);

		var reflectiveObject = hitInfo.collider.GetComponent<ILaserReflective>();			
		if (reflectiveObject != null)
			reflectiveObject.Reflect(this, ray, hitInfo);

		var currentLaserSensor = hitInfo.collider.GetComponent<LaserSensor>();
		if (currentLaserSensor != prevStruckLaserSensor)
		{
			LaserSensor.HandleLaser(this, prevStruckLaserSensor, currentLaserSensor);
			prevStruckLaserSensor = currentLaserSensor;
		}
	}
}
