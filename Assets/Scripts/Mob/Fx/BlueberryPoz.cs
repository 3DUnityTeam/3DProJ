using UnityEngine;

public class BlueberryPoz : MonoBehaviour
{
    bool isAll;

    private void Update()
    {
        isAll = GameObject.Find("Dragon").GetComponent<DragonController>().meteoAll;
        if (isAll)
            Destroy(this.gameObject, 0.8f);
    }


}
