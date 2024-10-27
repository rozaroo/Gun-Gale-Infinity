using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    SpaceShipController Player;
    public Image crosshairImage;
    void Start()
    {
        Player = FindObjectOfType<SpaceShipController>();
    }
    void Update()
    {
        if (Player != null) 
        {
            Vector3 mousePosition = Input.mousePosition;
            crosshairImage.transform.position = mousePosition;
        }
        else crosshairImage.SetActive(false);
    }
}

