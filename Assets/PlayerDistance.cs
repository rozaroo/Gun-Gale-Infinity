using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistance : MonoBehaviour
{
    public Shader mucusWall;
    private Material material;

    public bool playerIsNear;

    private void Start()
    {
        playerIsNear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerIsNear = true;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (playerIsNear)
        {
            Graphics.Blit(source, destination, material);
        }
    }
}
