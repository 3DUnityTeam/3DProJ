using UnityEngine;

public class RollingFire : MonoBehaviour
{
    public float fireDmg;  //ƽ�� ������

    private void Start()
    {
        Destroy(this.gameObject, 2.3f);
    }
}
