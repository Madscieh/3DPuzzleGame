using System.Collections.Generic;
using UnityEngine;

namespace Cube
{
    public class CubeRead : MonoBehaviour
    {
        public Transform m_TFront;
        public Transform m_TBack;
        public Transform m_TUp;
        public Transform m_TDown;
        public Transform m_TLeft;
        public Transform m_TRight;
        private List<GameObject> raysFront = new List<GameObject>();
        private List<GameObject> raysBack = new List<GameObject>();
        private List<GameObject> raysUp = new List<GameObject>();
        private List<GameObject> raysDown = new List<GameObject>();
        private List<GameObject> raysLeft = new List<GameObject>();
        private List<GameObject> raysRight = new List<GameObject>();   
        private const int LayerMask = 1 << 8; // this layerMask is for the faces of the cube only
        private CubeState cubeState;
        public GameObject m_Empty;
        private void Start()
        {
            SetRayTransforms();
            ReadState();
            cubeState = FindObjectOfType<CubeState>();
            CubeState.m_Started = true;
        }
        public void ReadState()
        {
            cubeState = FindObjectOfType<CubeState>();
            // set the state of each position in the list of sides so we know
            // what color is in what position
            cubeState.m_Front = ReadFace(raysFront, m_TFront);
            cubeState.m_Back = ReadFace(raysBack, m_TBack);
            cubeState.m_Up = ReadFace(raysUp, m_TUp);
            cubeState.m_Down = ReadFace(raysDown, m_TDown);
            cubeState.m_Left = ReadFace(raysLeft, m_TLeft);
            cubeState.m_Right = ReadFace(raysRight, m_TRight);
        }
        private void SetRayTransforms()
        {
            // populate the ray listFrontraycasts emanating from the transform, angled towards the cube.
            raysFront = BuildRays(m_TFront, new Vector3(0, 90, 0));
            raysBack = BuildRays(m_TBack, new Vector3(0, -90, 0));
            raysUp = BuildRays(m_TUp, new Vector3(90, 90, 0));
            raysDown = BuildRays(m_TDown, new Vector3(-90, 90, 0));
            raysLeft = BuildRays(m_TLeft, new Vector3(0, 180, 0));
            raysRight = BuildRays(m_TRight, new Vector3(0, 0, 0));
        }
        private List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
        {
            // The ray count is used to name the rays so we can be sure they are in the right order.
            var rayCount = 0;
            var rays = new List<GameObject>();
            // Creates 9 rays in the following shape:
            // |0|1|2|
            // |3|4|5|
            // |6|7|8|
            for (var y = 4; y > -5; y -= 4)
            {
                for (var x = -4; x < 5; x += 4)
                {
                    var startPos = new Vector3(rayTransform.localPosition.x + x, rayTransform.localPosition.y + y, rayTransform.localPosition.z);
                    var rayStart = Instantiate(m_Empty, startPos, Quaternion.identity, rayTransform);
                    rayStart.name = rayCount.ToString();
                    rays.Add(rayStart);
                    rayCount++;
                }
            }
            rayTransform.localRotation = Quaternion.Euler(direction);
            return rays;
        }
        public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
        {
            var facesHit = new List<GameObject>();
            foreach (var rayStart in rayStarts)
            {
                var ray = rayStart.transform.position;
                // Does the ray intersect any objects in the layerMask?
                if (Physics.Raycast(ray, rayTransform.forward, out var hit, Mathf.Infinity, LayerMask))
                {
                    Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                    facesHit.Add(hit.collider.gameObject);
                }
                else
                {
                    Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
                }
            }
            return facesHit;
        }
    }
}