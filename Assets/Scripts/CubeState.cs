using System.Collections.Generic;
using UnityEngine;

namespace Cube
{
    public class CubeState : MonoBehaviour
    {
        public List<GameObject> m_Front = new List<GameObject>();
        public List<GameObject> m_Back = new List<GameObject>();
        public List<GameObject> m_Up = new List<GameObject>();
        public List<GameObject> m_Down = new List<GameObject>();
        public List<GameObject> m_Left = new List<GameObject>();
        public List<GameObject> m_Right = new List<GameObject>();
        public static bool m_AutoRotating = false;
        public static bool m_Started = false;
        public void PickUp(List<GameObject> cubeSide)
        {
            // Attach the cubes' parents to the one in the middle of the face
            foreach (var face in cubeSide) if (face != cubeSide[4]) face.transform.parent.transform.parent = cubeSide[4].transform.parent;
        }    
        public void PutDown(List<GameObject> cubes, Transform pivot)
        {
            // Undo PickUp()
            foreach (var cube in cubes) if (cube != cubes[4]) cube.transform.parent.transform.parent = pivot;
        }
    }
}