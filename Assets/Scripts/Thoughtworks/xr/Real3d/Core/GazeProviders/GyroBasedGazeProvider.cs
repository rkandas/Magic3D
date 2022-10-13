using System.Collections;
using UnityEngine;

namespace Thoughtworks.xr.Real3d.Core.GazeProviders
{
    public class GyroBasedGazeProvider : VirtualGazeProvider
    {
        [SerializeField] private float movementScale = 2f;
        [SerializeField] private float calibrationDelay = 1f;
        [SerializeField] private Quaternion correctionQuaternion = Quaternion.Euler(90f, -90f, 0f);
        [SerializeField] private Transform testObject;
        private Gyroscope phoneGyro;
        private Quaternion startCalibration = Quaternion.identity;

        private Quaternion GyroRotation => GyroToUnity(phoneGyro.attitude);

        private void OnEnable()
        {
            IsTracking = true;
            phoneGyro = Input.gyro;
            phoneGyro.enabled = true;
            StartCoroutine(CalibrateAfterDelay());
        }

        private void OnDisable()
        {
            IsTracking = false;
        }

        private void FixedUpdate()
        {
            if (IsTracking)
            {
                Quaternion rotation = startCalibration * GyroRotation;
                translation = rotation * Vector3.forward * movementScale;
                if (testObject)
                    testObject.transform.rotation = rotation;
            }
        }

        private IEnumerator CalibrateAfterDelay()
        {
            yield return new WaitForSeconds(calibrationDelay);
            Calibrate();
        }
        
        public void Calibrate()
        {
            startCalibration = Quaternion.Inverse(GyroRotation);
        }

        private Quaternion GyroToUnity(Quaternion q)
        {
            return correctionQuaternion * new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }
}