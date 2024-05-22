using UnityEngine;

public class RotateManager : MonoBehaviour
{



    private void FixedUpdate()
    {


        this.gameObject.transform.LookAt(Camera.main.transform);


    }
}
