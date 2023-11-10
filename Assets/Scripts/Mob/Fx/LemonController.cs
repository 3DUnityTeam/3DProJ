using UnityEngine;

public class LemonController : MonoBehaviour
{
    float firePower = 800f;
    Rigidbody rigid_;

    void Start()
    {
        rigid_ = GetComponent<Rigidbody>();
        rigid_.AddRelativeForce(Vector3.forward * firePower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Land"))
        {
            Destroy(this.gameObject, 1.5f);
        }
    }
}
