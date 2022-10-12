using Thoughtworks.xr.Real3d.Core.GazeProviders;
using UnityEngine;

namespace Thoughtworks.xr.Real3d.Core.Projection
{
    [RequireComponent(typeof(Eye))]
    public class BasicEyeMovement : MonoBehaviour
    {
        public VirtualGazeProvider gazeProvider;

        private Eye virtualEye;
        private Vector3 initialLocalPosition;
        private Quaternion initialLocalRotation;

        void Start()
        {
            virtualEye = GetComponent<Eye>();
            initialLocalPosition = virtualEye.transform.localPosition;
            initialLocalRotation = virtualEye.transform.localRotation;
        }

        void Update()
        {
            if (gazeProvider == null)
                return;

            if(gazeProvider.IsTracking)
            {
                virtualEye.transform.localPosition = initialLocalPosition + gazeProvider.Translation;
                //virtualEye.transform.rotation = gazeProvider.Rotation;
            }
        }
    }
}
