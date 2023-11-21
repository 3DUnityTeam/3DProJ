using UnityEngine;

public class LemonController : MonoBehaviour
{
    float firePower = 1600f;
    Rigidbody rigid_;
    public float damage = 5;

    void Start()
    {
        rigid_ = GetComponent<Rigidbody>();
        rigid_.AddRelativeForce(Vector3.forward * firePower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            GameManager.instance.player.GetHitDamage(damage);
            Destroy(this.gameObject);
        }else if(collision.gameObject.CompareTag("Land"))
        {
            Destroy(this.gameObject, 0.3f);
        }
    }
}
