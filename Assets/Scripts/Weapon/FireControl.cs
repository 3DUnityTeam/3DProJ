using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public AimManager aimManager;
    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;
    private new AudioSource audio;
    public MeshRenderer muzzelFlash;
    public float cooltime = 0.7f;
    private float ctime = 0.7f;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        aimManager = GameManager.instance.AimManager;
        ctime = cooltime;
        //audio = GetComponent<AudioSource>();
        //muzzelFlash.enabled = false;
    }

    private void Update()
    {
        ctime -= Time.deltaTime;
        if (ctime <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
                ctime = cooltime;
                
            }
        }
        
    }
    // Update is called once per frame
    void Fire()
    {
        GameObject firedbullet = Instantiate(bullet, firePos.position, firePos.rotation);
        if (aimManager.aimingTarget != null)
        {
            //Vector3 direction = aimManager.aimingTarget.transform.position - firePos.position;
            //Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            //firedbullet.transform.rotation = lookRotation;
            firedbullet.transform.LookAt(aimManager.aimingTarget.transform.position);

        }
        //audio.PlayOneShot(fireSfx, 1.0f);
        //StartCoroutine(ShowMuzzleFlash());
    }

    /*IEnumerator ShowMuzzleFlash()
    {
        Vector2 offset = new Vector3(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        float angle = Random.Range(0, 360);
        muzzelFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        float scale = Random.Range(1.0f, 2.0f);
        muzzelFlash.transform.localScale = Vector3.one * scale;

        muzzelFlash.enabled = true;

        yield return new WaitForSeconds(0.009f);
        muzzelFlash.enabled = false;
    }*/
}
