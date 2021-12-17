using System.Collections.Generic;
using UnityEngine;

namespace Cube
{
    public class FaceSelection : MonoBehaviour
    {
        private CubeState cubeState;
        private CubeRead cubeRead;
        private const int LayerMask = 1 << 8;
        private const float RayMaxDistance = 50f;
        private void Start()
        {
            cubeRead = FindObjectOfType<CubeRead>();
            cubeState = FindObjectOfType<CubeState>();
        }
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (CubeState.m_AutoRotating) return;
            cubeRead.ReadState();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, RayMaxDistance, LayerMask)) return;
            var face = hit.collider.gameObject;
            var cubeSides = new List<List<GameObject>>() {cubeState.m_Up, cubeState.m_Down, cubeState.m_Left, cubeState.m_Right, cubeState.m_Front, cubeState.m_Back};
            foreach (var cubeSide in cubeSides)
            {
                if (!cubeSide.Contains(face)) return;
                cubeState.PickUp(cubeSide);
                cubeSide[4].transform.parent.GetComponent<FaceRotation>().Rotate(cubeSide);
            }
        }
    }
}