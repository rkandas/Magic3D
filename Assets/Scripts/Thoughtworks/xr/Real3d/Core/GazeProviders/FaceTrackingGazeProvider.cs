using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Thoughtworks.xr.Real3d.Core.GazeProviders
{
    public class FaceTrackingGazeProvider : VirtualGazeProvider
    {
        public ARFaceManager ARCoreFaceManager;
        private ARFace trackingFace;

        [SerializeField] private float movementScale = 2f;
        void Start()
        {
            IsTracking = true;
            if (ARCoreFaceManager == null)
            {
                Debug.LogError("ARCore face manager is not set in the scene. Face Tracking will not work.");
                return;
            }
            else
            {
                ARCoreFaceManager.facesChanged += ARCoreFaceManagerOnfacesChanged;
            }
        }

        private void ARCoreFaceManagerOnfacesChanged(ARFacesChangedEventArgs facesChangedEventArgs)
        {
            if (facesChangedEventArgs.added.Count > 0)
            {
                trackingFace = facesChangedEventArgs.added.First();
                Debug.Log("Face detected for tracking. Face ID:" + trackingFace.trackableId);
            }

            if (facesChangedEventArgs.removed.Contains(trackingFace))
            {
                Debug.LogWarning("Face removed. Face ID:" + trackingFace.trackableId);
                trackingFace = null;
            }


        }

        private void Update()
        {
            if (IsTracking)
            {
                if (ARCoreFaceManager == null || trackingFace == null)
                    return;

                translation = new Vector3(-trackingFace.transform.position.x, trackingFace.transform.position.y, trackingFace.transform.position.z) * movementScale;
                Debug.Log("Position:" + translation );

            }
        }
    }
}
