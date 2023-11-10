using System.Collections;
using UnityEngine;

public class Blueberry : MonoBehaviour
{
    Transform trans_;
    public GameObject fx;
    GameObject spawnPoz;

    bool go = false;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
        string[] thisName = this.gameObject.name.Split();
        spawnPoz = GameObject.Find("MeteoPoz " + thisName[1]);
    }


    private void Update()
    {
        go = GameObject.Find("Dragon").GetComponent<DragonController>().meteoAll;
        if (go)
        {
            trans_.Translate(spawnPoz.transform.position * 3f * Time.deltaTime);
        }
        else
        {
            trans_.Translate((spawnPoz.transform.position + new Vector3(0, 10, 0))
                * 3f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poz"))
        {
            Destroy(this.gameObject, 1.2f);
            fx.SetActive(true);
        }
    }
}
