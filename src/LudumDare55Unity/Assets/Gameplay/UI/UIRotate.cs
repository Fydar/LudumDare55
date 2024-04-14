using System.Diagnostics;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/Rotate", 18)]
    public class UIRotate : BaseMeshEffect
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SerializeField] private Vector2 rotation = new(30.0f, 0.0f);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SerializeField] private float fov = 90.0f;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SerializeField] private float distance = 60.0f;

        public Vector2 Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                if (graphic != null)
                {
                    graphic.SetVerticesDirty();
                }
            }
        }

        public float Fov
        {
            get => fov;
            set
            {
                fov = value;
                if (graphic != null)
                {
                    graphic.SetVerticesDirty();
                }
            }
        }

        public float Distance
        {
            get => distance;
            set
            {
                distance = value;
                if (graphic != null)
                {
                    graphic.SetVerticesDirty();
                }
            }
        }

        protected UIRotate()
        {
        }

        protected override void OnValidate()
        {
            graphic.SetVerticesDirty();
            base.OnValidate();
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var matrix = Matrix4x4.Perspective(Mathf.Deg2Rad * Fov, 1.0f, 0.5f, 100.0f)
                * Matrix4x4.Translate(new Vector3(0.0f, 0.0f, distance))
                * Matrix4x4.Rotate(Quaternion.Euler(Rotation.x, Rotation.y, 0.0f));

            UIVertex v0 = default;
            UIVertex v1 = default;
            UIVertex v2 = default;
            UIVertex v3 = default;

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref v0, i);
                v0.position = matrix.MultiplyPoint(v0.position);
                vh.SetUIVertex(v0, i);
            }

            for (int i = 0; i < vh.currentVertCount; i += 4)
            {
                vh.PopulateUIVertex(ref v0, i);
                vh.PopulateUIVertex(ref v1, i + 1);
                vh.PopulateUIVertex(ref v2, i + 2);
                vh.PopulateUIVertex(ref v3, i + 3);

                float ratio1 = Vector3.Distance(v0.position, v3.position) / Vector3.Distance(v2.position, v1.position);
                float ratio2 = Vector3.Distance(v0.position, v1.position) / Vector3.Distance(v3.position, v2.position);

                v0.uv0 = new Vector4(v0.uv0.x * ratio1, v0.uv0.y * ratio1 * ratio2, (ratio1 * ratio2) - 1.0f, 0.0f);
                v1.uv0 = new Vector4(v1.uv0.x * ratio2, v1.uv0.y * ratio2, ratio2 - 1.0f, 0.0f);
                v2.uv0 = new Vector4(v2.uv0.x * 1, v2.uv0.y * 1, 0.0f, 0.0f);
                v3.uv0 = new Vector4(v3.uv0.x * ratio1, v3.uv0.y * ratio1, ratio1 - 1.0f, 0.0f);

                vh.SetUIVertex(v0, i);
                vh.SetUIVertex(v1, i + 1);
                vh.SetUIVertex(v2, i + 2);
                vh.SetUIVertex(v3, i + 3);
            }
        }
    }
}
