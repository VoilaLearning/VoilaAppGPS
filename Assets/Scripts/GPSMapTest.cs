using UnityEngine;
using System.Collections;

public class GPSMapTest : MonoBehaviour {

    Vector3 initialCameraPosition;
    Vector3 mapCameraPosition = new Vector3(0, 20, 0);
    Vector3 mapCameraRotation = new Vector3(90, 0, -180);
    float cameraMoveSpeed = 1f;
    float cameraRotationSpeed = 3f;
    bool moveCamera = false;

    void OnEnable () {

        moveCamera = true;
    }

    void Update () {

        if(moveCamera) {

            float remainingDistance = Vector3.Distance(Camera.main.transform.position, mapCameraPosition);

            if(remainingDistance > 0.1f) {

                Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, mapCameraPosition, cameraMoveSpeed * Time.deltaTime);
            }
            else {

                moveCamera = false;
            }

            float remainingAngle = Vector3.Angle(Camera.main.transform.localEulerAngles, mapCameraRotation);
            Quaternion mapCameraQuaternion = Quaternion.Euler(mapCameraRotation);
            if(remainingAngle > 1) {

                Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, mapCameraQuaternion, cameraRotationSpeed * Time.deltaTime);
            }
        }
    }
}
