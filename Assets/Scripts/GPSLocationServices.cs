using UnityEngine;
using System.Collections;

public class GPSLocationServices : MonoBehaviour {

    const float EARTH_RADIUS = 6371000; // meters


    protected float HaversineDistance (ref float lastLatitude, ref float lastLongitude) {

        float newLatitude = Input.location.lastData.latitude;
        float newLongitude = Input.location.lastData.longitude;

        float deltaLatitude = (newLatitude - lastLatitude) * Mathf.Deg2Rad;
        float deltaLongitude = (newLongitude - lastLongitude) * Mathf.Deg2Rad;

        float distance = 2 * EARTH_RADIUS * Mathf.Asin(Mathf.Sqrt(Mathf.Sin(deltaLatitude * 0.5f) * Mathf.Sin(deltaLatitude * 0.5f)
            + Mathf.Cos(lastLatitude * Mathf.Deg2Rad) * Mathf.Cos(newLatitude * Mathf.Deg2Rad) * Mathf.Sin(deltaLongitude * 0.5f) * Mathf.Sin(deltaLongitude * 0.5f)));

        lastLatitude = newLatitude;
        lastLongitude = newLongitude;

        return distance;
    }
}
