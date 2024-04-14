using UnityEngine;
using UnityEngine.UI;

public class FloatingCard : MonoBehaviour
{
    [Header("Follow")]
    public RectTransform FollowTarget;
    public float PositionDampening = 0.1f;
    public float RotationDampening = 0.1f;

    [Header("Card Rendering")]
    public UICardRenderer UICardRenderer;
    public UIRotate TiltGraphic;
    public float TiltScale = 0.1f;
    public float TiltDampening = 1.0f;

    public Vector3 movementVelocity;


    private void Update()
    {
        if (FollowTarget != null)
        {
            var oldPosition = transform.position;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                FollowTarget.transform.position,
                ref movementVelocity,
                (1.0f / PositionDampening));


            transform.rotation = Quaternion.Slerp(transform.rotation, FollowTarget.transform.rotation, (1.0f / RotationDampening) * Time.deltaTime);

            var velocity = transform.position - oldPosition;
            var targetTilt = new Vector2(velocity.y, -velocity.x) * TiltScale;
            targetTilt = Rotate(targetTilt, transform.eulerAngles.z);

            TiltGraphic.Rotation = Vector2.Lerp(TiltGraphic.Rotation, targetTilt, (1.0f / TiltDampening) * Time.deltaTime);
        }
    }
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
