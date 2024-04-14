using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class ObliqueFrustum : MonoBehaviour
{
    public Vector4 up = new(0, 1, 0, 0);

    private Camera cameraComponent;

    private void OnEnable()
    {
        if (Application.isEditor)
        {
            Camera.onPreCull += ScenePreCull;
            Camera.onPostRender += ScenePostRender;
        }
    }

    private void OnDisable()
    {
        if (Application.isEditor)
        {
            Camera.onPreCull -= ScenePreCull;
            Camera.onPostRender -= ScenePostRender;
        }

        GetComponent<Camera>().ResetWorldToCameraMatrix();
    }

    private void Update()
    {
        UpdateMatrix();
    }

    public void UpdateMatrix()
    {
        cameraComponent = GetComponent<Camera>();
        cameraComponent.transparencySortMode = TransparencySortMode.Orthographic;

        var matrix = cameraComponent.transform.worldToLocalMatrix;
        matrix.SetRow(2, -matrix.GetRow(2));
        matrix.SetColumn(2, (1e-3f * matrix.GetColumn(2)) - up);
        cameraComponent.worldToCameraMatrix = matrix;
    }

    public static Matrix4x4 ScreenToWorldMatrix(Camera cam)
    {
        var rect = cam.pixelRect;
        var viewportMatrix = Matrix4x4.Ortho(rect.xMin, rect.xMax, rect.yMin, rect.yMax, -1, 1);

        var vpMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;

        vpMatrix.SetColumn(2, new Vector4(0, 0, 1, 0));

        return vpMatrix.inverse * viewportMatrix;
    }

    public Vector2 ScreenToWorldPoint(Vector2 point)
    {
        return ScreenToWorldMatrix(cameraComponent).MultiplyPoint(point);
    }

    private void ScenePreCull(Camera cam)
    {
        if (cam.cameraType == CameraType.SceneView)
        {
            UpdateMatrix();
        }
    }

    private void ScenePostRender(Camera cam)
    {
        if (cam.cameraType == CameraType.SceneView)
        {
            cam.ResetWorldToCameraMatrix();
        }
    }
}
