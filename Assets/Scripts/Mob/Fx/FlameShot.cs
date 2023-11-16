using UnityEngine;

public class FlameShot : MonoBehaviour
{
    Player player;

    public float flameDmg = 0.3f;  //Æ½´ç µ¥¹ÌÁö
    bool flag = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (!player)
            Debug.Log("Player is missing");
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            player.HP -= flameDmg;
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
