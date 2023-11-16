using UnityEngine;

public class RollingFire : MonoBehaviour
{
    Player player;

    public float fireDmg = 0.5f;  //Æ½´ç µ¥¹ÌÁö
    bool flag = false;

    private void Start()
    {
        Destroy(this.gameObject, 2.3f);
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (!player)
            Debug.Log("Player is missing");
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            player.HP -= fireDmg;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player is burning");
            flag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player is NOT burning");
            flag = false;
        }
    }
}
