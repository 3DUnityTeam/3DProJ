using UnityEngine;

public class BlueberryPoz : MonoBehaviour
{

    private void Update()
    {
        Destroy(this.gameObject, 1.8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            Destroy(this.gameObject, 0.2f);
        }
    }
}
