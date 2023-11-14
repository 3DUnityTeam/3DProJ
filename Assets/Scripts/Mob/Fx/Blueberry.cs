using UnityEngine;

public class Blueberry : MonoBehaviour
{
    public GameObject fx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poz"))
        {
            Destroy(this.gameObject, 1.2f);
            fx.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            Destroy(this.gameObject, 1.2f);
            fx.SetActive(true);
        }
    }
}