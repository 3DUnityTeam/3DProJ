using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    [Header ("�ܺ� ������Ʈ")]
    public AimManager aimManager;
    public int bulletID;
    public Transform firePos;
    public AudioClip fireSfx;
    private new AudioSource audio;
    public MeshRenderer muzzelFlash;
    public GameObject muzzlefire;
    private Gunmove gunmove;

    [Header ("���� ��ġ")]
    public float rapidspeed = 0.7f;
    private float ctime = 0.7f;
    public float bulletspeed = 2000f;
    

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        gunmove = GetComponent<Gunmove>();
        aimManager = GameManager.instance.AimManager;
        ctime = rapidspeed;
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
                ctime = rapidspeed;
                
            }
        }
        
    }
    // Update is called once per frame
    void Fire()
    {
        GameObject firedbullet = GameManager.instance.bulletPoolManger.Get(bulletID - 1);
        Vector3 direction;

        if (firedbullet != null)
        {
            // ������Ʈ Ǯ���� ���� ������Ʈ�� �ʱ� ���� ����
            firedbullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            firedbullet.transform.position = firePos.transform.position;
            firedbullet.transform.rotation = firePos.transform.rotation;

            if (aimManager.aimingTarget != null)
            {
                // Ÿ���� ���ϵ��� ȸ�� ����
                firedbullet.transform.LookAt(aimManager.aimingTarget.transform.position);
                direction = (aimManager.aimingTarget.transform.position - firePos.transform.position).normalized;
            }
            else
            {
                // Ÿ���� ���� ��� �ʱ� ���� ����
                firedbullet.transform.forward = firePos.transform.forward;
                direction = (gunmove.basic.position - firePos.transform.position).normalized;
            }

            firedbullet.GetComponent<Rigidbody>().AddForce(direction * bulletspeed);
        }
        //audio.PlayOneShot(fireSfx, 1.0f);
        if(muzzelFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
        else if(muzzlefire != null)
        {
            muzzlefire.SetActive(true);
            StartCoroutine(ShowMuzzleFire());
        }
        
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector3 localscale = muzzelFlash.transform.localScale;
        Vector2 offset = new Vector3(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        float angle = Random.Range(0, 360);
        muzzelFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        float scale = Random.Range(0.5f, 1.2f);
        muzzelFlash.transform.localScale = localscale * scale;

        muzzelFlash.enabled = true;

        yield return new WaitForSeconds(rapidspeed/4);
        muzzelFlash.transform.localScale = localscale;
        muzzelFlash.enabled = false;
    }
    IEnumerator ShowMuzzleFire()
    {

        yield return new WaitForSeconds(rapidspeed - 0.2f);
        muzzlefire.SetActive(false);
    }
}
