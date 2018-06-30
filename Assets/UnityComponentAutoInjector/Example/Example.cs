using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
	[SerializeField, GetComponent]
	private Transform cachedTransform = null; 

	[SerializeField, GetComponent]
	private GameObject cachedGameObject = null;

	[SerializeField, GetComponentInParent]
	private Camera parent = null;

	[SerializeField, GetComponentInChildren]
	private Camera children = null;

	[SerializeField, GetComponentInChildrenOnly(false)]
	private Camera childrenOnly = null;

	[SerializeField, GetComponent]
	private Camera example = null;

	[SerializeField, FindGameObject("Directional Light")]
    private Light find = null;

	private void Awake()
	{
		CDebug.Log("(Example) Cached : ", cachedTransform, cachedGameObject);
	}
}