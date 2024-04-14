using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Fan Layout", 154)]
public class FanLayout : LayoutGroup
{
    [SerializeField] protected float m_minTilt = 5.0f;
    [SerializeField] protected float m_maxTilt = -5.0f;
    [SerializeField] protected float m_minPivot = 0.0f;
    [SerializeField] protected float m_maxHeightBump = 0.0f;

    public float MinTilt { get => m_minTilt; set => SetProperty(ref m_minTilt, value); }
    public float MaxTilt { get => m_maxTilt; set => SetProperty(ref m_maxTilt, value); }
    public float MinPivot { get => m_minPivot; set => SetProperty(ref m_minPivot, value); }
    public float MaxHeightBump { get => m_maxHeightBump; set => SetProperty(ref m_maxHeightBump, value); }

    public AnimationCurve heightCurve = new(
        new Keyframe(0.0f, 0.0f),
        new Keyframe(0.5f, 1.0f),
        new Keyframe(1.0f, 0.0f));

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    }
#endif

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal() { }

    public override void SetLayoutVertical()
    {
        SetChildren();
    }

    private void SetChildren()
    {
        if (rectChildren.Count > 1)
        {
            for (int i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                if (child.TryGetComponent<LayoutElement>(out var layoutElement))
                {
                    if (layoutElement.ignoreLayout)
                    {
                        continue;
                    }
                }
                float time = i / (float)(transform.childCount - 1);

                child.anchoredPosition = new Vector2(
                    child.anchoredPosition.x,
                    Mathf.Lerp(child.anchoredPosition.y, child.anchoredPosition.y + MaxHeightBump, heightCurve.Evaluate(time)));

                child.eulerAngles = new Vector3(
                    child.eulerAngles.x,
                    child.eulerAngles.y,
                    Mathf.Lerp(MinTilt, MaxTilt, time));
            }
        }
    }
}
