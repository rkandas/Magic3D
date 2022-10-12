using System;
using UnityEngine;
using UnityEngine.Events;

namespace Thoughtworks.xr.Real3d.Core.Interaction
{
    public class SwipeInteraction : MonoBehaviour
    {
        public float swipeThreshold = 30f;
        public float timeThreshold = 0.3f;

        public UnityEvent OnSwipeLeft;
        public UnityEvent OnSwipeRight;
        public UnityEvent OnSwipeUp;
        public UnityEvent OnSwipeDown;

        private Vector2 fingerDown;
        private DateTime fingerDownTime;
        private Vector2 fingerUp;
        private DateTime fingerUpTime;

        private void Update ()
        {
            CheckMouseForSwipe();
            CheckFingerTouchForSwipe();
        }

        private void CheckFingerTouchForSwipe()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    this.fingerDown = touch.position;
                    this.fingerUp = touch.position;
                    this.fingerDownTime = DateTime.Now;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    this.fingerDown = touch.position;
                    this.fingerUpTime = DateTime.Now;
                    this.DetectSwipeDirectionAndRaiseEvent();
                }
            }
        }

        private void CheckMouseForSwipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.fingerDown = Input.mousePosition;
                this.fingerUp = Input.mousePosition;
                this.fingerDownTime = DateTime.Now;
            }

            if (Input.GetMouseButtonUp(0))
            {
                this.fingerDown = Input.mousePosition;
                this.fingerUpTime = DateTime.Now;
                this.DetectSwipeDirectionAndRaiseEvent();
            }
        }

        private void DetectSwipeDirectionAndRaiseEvent() {
            float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
            if (duration > this.timeThreshold) return;

            DetectHorizontalSwipes();

            DetectVerticalSwipes();

            this.fingerUp = this.fingerDown;
        }

        private void DetectVerticalSwipes()
        {
            float deltaY = fingerDown.y - fingerUp.y;
            if (Mathf.Abs(deltaY) > this.swipeThreshold)
            {
                if (deltaY > 0)
                {
                    this.OnSwipeUp.Invoke();
                }
                else if (deltaY < 0)
                {
                    this.OnSwipeDown.Invoke();
                }
            }
        }

        private void DetectHorizontalSwipes()
        {
            float deltaX = this.fingerDown.x - this.fingerUp.x;
            if (Mathf.Abs(deltaX) > this.swipeThreshold)
            {
                if (deltaX > 0)
                {
                    this.OnSwipeRight.Invoke();
                }
                else if (deltaX < 0)
                {
                    this.OnSwipeLeft.Invoke();
                }
            }
        }
    }
}