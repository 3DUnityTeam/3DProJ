using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : MonoBehaviour
{
    //HP
    private float maxHP = 50000f;
    public float MaxHP { get { return this.maxHP; } }
    private float hp = 50000f;
    public float HP { get { return this.hp; } set { this.hp = value; } }
    bool isRevive = false;
    GameManager manager = GameManager.instance;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        manager = GameManager.instance;
        if (manager.player.HP <= 0 && !isRevive)
        {
            GameObject deadeffect = GameManager.instance.effectPoolManger.Get(7 - 1);
            deadeffect.transform.position = manager.player.gameObject.transform.position;
            manager.player.gameObject.SetActive(false);
            isRevive = true;
            StartCoroutine(RevivePly(5));
        }
    }
    IEnumerator RevivePly(float revivetime)
    {
        GameManager.instance.UIManager.Dead(revivetime);
        yield return new WaitForSeconds(revivetime);
        HP = HP - (MaxHP * 0.12f);
        manager.player.gameObject.SetActive(true);
        manager.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        manager.player.HP = manager.player.MaxHP;
        isRevive = false;
    }
}
