using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        crosshairImage.transform.position = mousePosition;
    }
   
}

