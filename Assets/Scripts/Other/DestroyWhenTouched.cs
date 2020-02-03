using UnityEngine;

public class DestroyWhenTouched : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

}
