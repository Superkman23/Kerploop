/*
 * BulletRay.cs
 * Created by: Ambroisa
 * Created on: 5/2/2020 (dd/mm/yy)
 * Created for: Drawing bullet rays
 */

using System.Collections;
using UnityEngine;

public class BulletRay : MonoBehaviour
{
	LineRenderer _LineRenderer;

	private void Awake() 
	{
		_LineRenderer = GetComponent<LineRenderer>();
		_LineRenderer.enabled = false;
		_LineRenderer.useWorldSpace = true;	
	}

	public void SetRendererPosition(Vector3 position)
	{
        Debug.Log(position);
		_LineRenderer.enabled = true;
		_LineRenderer.SetPosition(0, transform.position);
		_LineRenderer.SetPosition(1, position);
	}

	public IEnumerator WaitThenDestroy(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
}
