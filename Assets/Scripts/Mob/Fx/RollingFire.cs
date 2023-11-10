using UnityEngine;

public class RollingFire : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
