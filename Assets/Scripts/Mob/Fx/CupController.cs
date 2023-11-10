using UnityEngine;

public class CupController : MonoBehaviour
{
    public GameObject exp;
    public Transform expPoz;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land") || 
            collision.gameObject.CompareTag("Player"))
        {
            Instantiate(exp, expPoz.position, expPoz.rotation);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
