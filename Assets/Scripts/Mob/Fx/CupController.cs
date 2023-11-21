using UnityEngine;

public class CupController : MonoBehaviour
{
    public float damage=20;
    public GameObject exp;
    public Transform expPoz;
    bool once = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land") && !once)
        {
            once = true;
            Instantiate(exp, expPoz.position, expPoz.rotation);
            Destroy(this.gameObject, 0.5f);
        }
        else if(collision.gameObject.CompareTag("Player") && !once)
        {
            once = true;
            GameManager.instance.player.GetHitDamage(damage);
            Instantiate(exp, expPoz.position, expPoz.rotation);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
