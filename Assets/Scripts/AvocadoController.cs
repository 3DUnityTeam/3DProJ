using UnityEngine;

public class AvocadoController : MonoBehaviour
{
    float firePower = 1200f;
    Rigidbody rigid_;

    void Start()
    {
        rigid_ = GetComponent<Rigidbody>();
        rigid_.AddRelativeForce(Vector3.forward * firePower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(this.gameObject, 1.5f);
        }
    }
}
