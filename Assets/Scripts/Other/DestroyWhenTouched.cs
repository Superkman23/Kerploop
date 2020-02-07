/*
 * DestroyWhenTouched.cs
 * Created by: Kaelan Bartlett
 * Created on: 2/2/20 (dd/mm/yy)
 * Created for: An easy way to destroy all objects that hit this object
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyWhenTouched : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        Destroy(collision.gameObject);
    }

}
