using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();
        if (player != null) 
        {
           int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;                 SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
