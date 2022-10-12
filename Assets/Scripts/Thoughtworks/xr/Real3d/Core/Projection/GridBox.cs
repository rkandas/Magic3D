using UnityEngine;

namespace Thoughtworks.xr.Real3d.Core.Projection
{
    public class GridBox : MonoBehaviour
    {
        private GameObject alignmentCube;

        public float alignmentDepth = 5f;
        
        [SerializeField] private GameObject back;
        [SerializeField] private GameObject left;
        [SerializeField] private GameObject right;
        [SerializeField] private GameObject top;
        [SerializeField] private GameObject bottom;

        public void Setup(Transform parent, bool show,  Vector2 size)
        {
            Size = size;
            
            if(Application.isPlaying)
            {
                alignmentCube = new GameObject("AlignmentCube");
                alignmentCube.transform.SetParent(parent, false);

                alignmentCube.transform.localPosition = Vector3.zero;
                alignmentCube.transform.rotation = parent.rotation;

                if (back == null || left == null || right == null || top == null || bottom == null)
                {
                    Debug.LogError("Wall Game Objects are not set. Illusion may not be perceived.");
                }
                alignmentCube.SetActive(show);
            }
        }
        private GameObject CreateAlignmentQuad(Material surfaceMaterial)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.parent = alignmentCube.transform;
            quad.GetComponent<Renderer>().material = surfaceMaterial;
            return quad;
        }
        private void UpdateAlignmentQuad(Transform t, Vector3 pos, Vector3 scale, Quaternion rotation)
        {
            t.localPosition = pos;
            t.localScale = scale;
            t.localRotation = rotation;
        }
        public void UpdateAlignmentCube()
        {
            if (alignmentCube.activeInHierarchy)
            {
                //Debug.Log("Real3D: Alignment Cube active in hierarchy.");
                Vector2 halfSize = Size * 0.5f;
                UpdateAlignmentQuad(back.transform, new Vector3(0, 0, alignmentDepth), new Vector3(Size.x, Size.y),
                    Quaternion.identity);
                UpdateAlignmentQuad(left.transform,
                    new Vector3(-halfSize.x, 0, alignmentDepth * 0.5f),
                    new Vector3(alignmentDepth, Size.y, 0.1f),
                    Quaternion.Euler(0, -90, 0));
                UpdateAlignmentQuad(right.transform,
                    new Vector3(halfSize.x, 0, alignmentDepth * 0.5f),
                    new Vector3(alignmentDepth, Size.y, 0.1f),
                    Quaternion.Euler(0, 90, 0));
                UpdateAlignmentQuad(top.transform,
                    new Vector3(0, halfSize.y, alignmentDepth * 0.5f),
                    new Vector3(Size.x, alignmentDepth, 0.1f),
                    Quaternion.Euler(-90, 0, 0));

                UpdateAlignmentQuad(bottom.transform,
                    new Vector3(0, -halfSize.y, alignmentDepth * 0.5f),
                    new Vector3(Size.x, alignmentDepth, 0.1f),
                    Quaternion.Euler(90, 0, 0));
            }
        }
        public Vector2 Size { get; set; }
        public void Destroy()
        {
            GameObject.DestroyImmediate(alignmentCube);
        }
    }
}