using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoz;

    Transform trans_;
    Rigidbody rigid_;
    Animator ani_;

    float jumpPower = 25;
    float turnSpeed = 0;

    int jump = 2;
    int stage = 1;

    bool flag = false;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
        rigid_ = GetComponent<Rigidbody>();
        ani_ = GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        turnSpeed = 1500;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxis("Mouse X");
        rigid_.AddForce(new Vector3(-h, 0, -v), ForceMode.Impulse);
        trans_.Rotate(Vector3.up * turnSpeed * Time.deltaTime * x);


        if (h != 0 || v != 0)
        {
            ani_.SetBool("Walk", true);
        }
        else
        {
            ani_.SetBool("Walk", false);
        }

        if (Input.GetMouseButton(0))
        {
            StartCoroutine("FireBullet");
        }
    }

    private void Update()
    {
        if (trans_.position.y < -3)
        {
            SceneManager.LoadScene("Stage" + stage);
        }

        if (Input.GetButtonDown("Jump") && jump > 0)
        {
            jump--;
            ani_.SetBool("Jump", true);
            rigid_.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    IEnumerator FireBullet()
    {
        if (!flag)
        {
            flag = true;
            Instantiate(bullet, firePoz.position, firePoz.rotation);
            ani_.SetTrigger("Atk");
            yield return new WaitForSeconds(0.5f);
            flag = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            jump = 2;
            ani_.SetBool("Jump", false);
        }
    }
}
