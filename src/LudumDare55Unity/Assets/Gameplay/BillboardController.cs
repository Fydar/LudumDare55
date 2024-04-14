using UnityEngine;

public class BillboardController : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
