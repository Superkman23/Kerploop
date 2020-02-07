/*
 * ScalePreserver.cs
 * Created by: Kaelan Bartlett
 * Created on: 6/1/2020 (dd/mm/yy)
 * Created for: Preventing children from scaling
 */

using UnityEngine;

public class ScalePreserver : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
    }
}
