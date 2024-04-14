using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldCardZone : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;

    public RenderTexture Rendered { get; private set; }

    private void Awake()
    {
    }

    public void Render(CardTemplate cardTemplate)
    {
        var rt = renderCamera.targetTexture;
        Rendered = new RenderTexture(rt);
        renderCamera.targetTexture = Rendered;

        Rendered.filterMode = FilterMode.Point;

        var clone = Instantiate(cardTemplate.graphics, transform);
        clone.transform.localPosition = Vector3.zero;

        SetGameLayerRecursive(clone, 7);

        if (renderCamera.TryGetComponent<ObliqueFrustum>(out var obliqueFrustum))
        {
            obliqueFrustum.UpdateMatrix();
        }
        renderCamera.Render();

        renderCamera.gameObject.SetActive(false);
    }

    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WorldCardZone))]
public class WorldCardZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var previewRect = GUILayoutUtility.GetRect(64.0f * 4, 128.0f * 4, GUILayout.ExpandWidth(false));

        var targetZone = (WorldCardZone)target;
        if (targetZone.Rendered != null)
        {
            EditorGUI.DrawTextureTransparent(previewRect, targetZone.Rendered);
        }
        else
        {
            EditorGUI.DrawRect(previewRect, new Color(0.2f, 0.2f, 0.2f, 1.0f));
        }
    }
}
#endif
