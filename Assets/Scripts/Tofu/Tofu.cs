using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : MonoBehaviour
{
    //HP
    public float maxHP = 600f;
    public float MaxHP { get { return this.maxHP; } }
    private float hp;
    public float HP { get { return this.hp; } set { this.hp = value; } }
    public float timeSpeed;
    public float TimeSpeed { get { return this.timeSpeed; } set { this.timeSpeed = value; } }

    public float attackCount;

    bool isRevive = false;
    GameManager manager = GameManager.instance;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        timeSpeed = 1;
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
    void FixedUpdate()
    {
        if (GameManager.instance.UIManager.ResultUIDefeat.activeSelf == true || GameManager.instance.UIManager.ResultUIVictory.activeSelf == true)
            return;
        HP -= Time.fixedDeltaTime*(timeSpeed);
    }
    
    IEnumerator RevivePly(float revivetime)
    {
        GameManager.instance.UIManager.Dead(revivetime);
        yield return new WaitForSeconds(revivetime);
        HP = HP - (MaxHP * 0.12f);
        manager.player.gameObject.SetActive(true);
        isRevive = false;
    }
}
