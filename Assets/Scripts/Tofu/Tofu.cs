using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : MonoBehaviour
{
    //HP
    private float maxHP = 100f;
    public float MaxHP { get { return this.maxHP; } }
    private float hp = 100f;
    public float HP { get { return this.hp; } set { this.hp = value; } }
    GameManager manager = GameManager.instance;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        manager = GameManager.instance;
        if (manager.player.HP <= 0)
        {
            StartCoroutine(RevivePly());
        }
    }
    IEnumerator RevivePly()
    {
        yield return new WaitForSeconds(3);
        manager.player.gameObject.SetActive(true);
        manager.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        manager.player.HP = manager.player.MaxHP;

    }
}
