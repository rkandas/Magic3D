using UnityEngine;

namespace Thoughtworks.xr.Real3d.Core.Interaction
{
    public class FreeRotate : MonoBehaviour
    {
        [SerializeField] private float rotationFactor = 0.25f;
        [SerializeField] private bool autoCorrectAxis = true;
        [SerializeField] private float autoCorrectThreshold = 0.1f;
        [SerializeField] private bool basicRotation;
        private TouchPhase currentPhase = TouchPhase.Ended;
        private Vector2 previousTouchPoint;
        private Vector2 currentTouchPoint;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            CheckTouchDrag();
#else
            CheckMouseDrag();
#endif
            if (basicRotation)
                RotateBasic();
            else 
                Rotate();
        }

        private void CheckMouseDrag()
        {
            previousTouchPoint = currentTouchPoint;
            if (Input.GetMouseButton(0))
            {
                currentPhase = currentPhase == TouchPhase.Ended ? TouchPhase.Began : TouchPhase.Moved;
            }
            else
            {
                currentPhase = TouchPhase.Ended;
            }

            currentTouchPoint = Input.mousePosition;
        }

        private void CheckTouchDrag()
        {
            previousTouchPoint = currentTouchPoint;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                currentPhase = touch.phase;
                currentTouchPoint = touch.position;
            }
            else
            {
                currentPhase = TouchPhase.Ended;
            }
        }

        private void Rotate()
        {
            if (currentPhase != TouchPhase.Moved || !mainCamera) return;

            float distance = Vector3.Distance(transform.position, mainCamera.transform.position) / 2f;

            Vector2 delta = currentTouchPoint - previousTouchPoint;
            Ray previousRay = mainCamera.ScreenPointToRay(previousTouchPoint);
            Ray currentRay = mainCamera.ScreenPointToRay(currentTouchPoint);

            Vector3 previousPoint = previousRay.GetPoint(distance);
            Vector3 currentPoint = currentRay.GetPoint(distance);

            Vector3 axis = Vector3.Cross(transform.position - previousPoint, transform.position - currentPoint);

            if (autoCorrectAxis)
            {
                axis = AutoCorrectAxis(axis);
            }

            // Debug.DrawLine(previousRay.origin, previousRay.origin + previousRay.direction * 5f, Color.magenta, 0.5f);
            // Debug.DrawLine(currentRay.origin, currentRay.origin + currentRay.direction * 5f, Color.cyan, 0.5f);
            // Debug.DrawLine(transform.position, transform.position + axis * 25f, Color.green, 2.5f);
            // Debug.LogError(delta);

            transform.Rotate(axis, rotationFactor * delta.magnitude, Space.World);
        }

        private void RotateBasic()
        {
            if (currentPhase != TouchPhase.Moved || !mainCamera) return;
            
            Vector2 delta = currentTouchPoint - previousTouchPoint;
            Ray previousRay = mainCamera.ScreenPointToRay(previousTouchPoint);
            Ray currentRay = mainCamera.ScreenPointToRay(currentTouchPoint);

            Vector3 axis = Vector3.Cross(previousRay.direction, currentRay.direction);

            if (autoCorrectAxis)
            {
                axis = AutoCorrectAxis(axis);
            }

            // Debug.DrawLine(previousRay.origin, previousRay.origin + previousRay.direction * 5f, Color.magenta, 0.5f);
            // Debug.DrawLine(currentRay.origin, currentRay.origin + currentRay.direction * 5f, Color.cyan, 0.5f);
            // Debug.DrawLine(transform.position, transform.position + axis * 25f, Color.green, 2.5f);
            // Debug.LogError(delta);

            transform.Rotate(axis, -rotationFactor * delta.magnitude, Space.World);
        }

        private Vector3 AutoCorrectAxis(Vector3 axis)
        {
            Vector3 normalized = axis.normalized;
            if (Mathf.Abs(normalized.x) < autoCorrectThreshold)
            {
                normalized.x = 0;
            }
            if (Mathf.Abs(normalized.y) < autoCorrectThreshold)
            {
                normalized.y = 0;
            }
            if (Mathf.Abs(normalized.z) < autoCorrectThreshold)
            {
                normalized.z = 0;
            }

            return normalized.normalized * axis.magnitude;
        }
    }
}