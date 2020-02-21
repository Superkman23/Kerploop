/*
 * OnTriggerResetScene.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerResetScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
