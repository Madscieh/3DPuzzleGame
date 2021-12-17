using UnityEngine;

namespace Cube
{
    public class CubeRotation : MonoBehaviour
    {
        private Vector2 clickBegin;
        private Vector2 clickEnd;
        private Vector2 swipe;
        private Vector3 mousePositionPrevious;
        private Vector3 mouseDelta;
        private const float RotationReducer = 0.1f;
        private const float Speed = 200f;
        public Transform m_Target;    
        private void Update()
        {
            Drag();
            Swipe();
        }
        private void Drag()
        {
            if (Input.GetMouseButton(0))
            {
                // while the mouse is held down the cube can be moved around its central axis to provide visual feedback
                mouseDelta = (Input.mousePosition - mousePositionPrevious) * RotationReducer;
                transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
            }
            else
            {
                if (transform.rotation == m_Target.transform.rotation) return;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_Target.transform.rotation, Speed * Time.deltaTime);
            }
            mousePositionPrevious = Input.mousePosition;
        }
        private void Swipe()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            clickBegin = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (!Input.GetMouseButtonUp(0)) return;
            clickEnd = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            swipe = new Vector2(clickEnd.x - clickBegin.x, clickEnd.y - clickBegin.y).normalized;
            //SwipeLeft
            if (swipe.x < 0 && Mathf.Abs(swipe.y) < 2f) m_Target.Rotate(0, 90, 0, Space.World);
            //SwipeRight
            else if (swipe.x > 0 && Mathf.Abs(swipe.y) < 2f) m_Target.Rotate(0, -90, 0, Space.World);
            //SwipeUpLeft
            else if (swipe.y > 0 && swipe.x < 0f) m_Target.Rotate(90, 0, 0, Space.World);
            //SwipeUpRight
            else if (swipe.y > 0 && swipe.x > 0f) m_Target.Rotate(0, 0, -90, Space.World);
            //SwipeDownLeft
            else if (swipe.y < 0 && swipe.x < 0f) m_Target.Rotate(0, 0, 90, Space.World);
            //SwipeDownRight
            else if (swipe.y < 0 && swipe.x > 0f) m_Target.Rotate(-90, 0, 0, Space.World);
        }
    }
}