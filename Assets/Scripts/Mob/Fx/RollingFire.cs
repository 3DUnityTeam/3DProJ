using UnityEngine;

public class RollingFire : MonoBehaviour
{
    public float fireDmg;  //틱당 데미지

    private void Start()
    {
        Destroy(this.gameObject, 2.3f);
    }
}
