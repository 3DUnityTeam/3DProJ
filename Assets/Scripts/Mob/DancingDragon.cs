using UnityEngine;

public class DancingDragon : MonoBehaviour
{
    public GameObject pm;
    Animator ani;

    private void Start()
    {

        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (pm.GetComponent<ProgressManager>().boss1Cleared ||
                    pm.GetComponent<ProgressManager>().boss2Cleared)
        {
            ani.SetTrigger("Anger");
        }        
    }
}
