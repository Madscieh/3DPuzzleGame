using System.Collections.Generic;
using UnityEngine;

namespace Cube
{
    public class FaceRotation : MonoBehaviour
    {
        private List<GameObject> activeSide;
        private Vector3 mouseRef;
        private Vector3 localForward;
        private bool dragging = false;
        private bool autoRotating = false;
        private const float Sensitivity = 0.4f;
        private const float Speed = 300f;
        private Vector3 rotation;
        private Quaternion targetQuaternion;
        private CubeRead cubeRead;
        private CubeState cubeState;
        private void Start()
        {
            cubeRead = FindObjectOfType<CubeRead>();
            cubeState = FindObjectOfType<CubeState>();
        }
        private void LateUpdate()
        {
            if (dragging && !autoRotating)
            {
                SpinSide(activeSide);
                if (Input.GetMouseButtonUp(0))
                {
                    dragging = false;
                    RotateToRightAngle();
                }
            }
            if (autoRotating) AutoRotate();
        }
        private void SpinSide(List<GameObject> side)
        {
            // Reset the rotation
            rotation = Vector3.zero;
            // current mouse position minus the last mouse position
            var mouseOffset = (Input.mousePosition - mouseRef);
            if (side == cubeState.m_Front) rotation.x = (mouseOffset.x + mouseOffset.y) * Sensitivity * -1;
            if (side == cubeState.m_Back) rotation.x = (mouseOffset.x + mouseOffset.y) * Sensitivity * 1;
            if (side == cubeState.m_Up) rotation.y = (mouseOffset.x + mouseOffset.y) * Sensitivity * 1;
            if (side == cubeState.m_Down) rotation.y = (mouseOffset.x + mouseOffset.y) * Sensitivity * -1;
            if (side == cubeState.m_Left) rotation.z = (mouseOffset.x + mouseOffset.y) * Sensitivity * 1;
            if (side == cubeState.m_Right) rotation.z = (mouseOffset.x + mouseOffset.y) * Sensitivity * -1;
            // rotate
            transform.Rotate(rotation, Space.Self);
            // store mouse
            mouseRef = Input.mousePosition;
        }
        public void Rotate(List<GameObject> side)
        {
            activeSide = side;
            mouseRef = Input.mousePosition;
            dragging = true;
            // Create a vector to rotate around
            localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        }
        public void StartAutoRotate(List<GameObject> side, float angle)
        {
            cubeState.PickUp(side);
            var localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
            targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
            activeSide = side;
            autoRotating = true;
        }
        public void RotateToRightAngle()
        {
            var vec = transform.localEulerAngles;
            // round vec to nearest 90 degrees
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            targetQuaternion.eulerAngles = vec;
            autoRotating = true;
        }
        private void AutoRotate()
        {
            dragging = false;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, Speed * Time.deltaTime);
            // if within one degree, set angle to target angle and end the rotation
            if (Quaternion.Angle(transform.localRotation, targetQuaternion) > 1) return;
            transform.localRotation = targetQuaternion;
            cubeState.PutDown(activeSide, transform.parent);
            cubeRead.ReadState();
            CubeState.m_AutoRotating = false;
            autoRotating = false;
            dragging = false;                                                          
        }         
    }
}