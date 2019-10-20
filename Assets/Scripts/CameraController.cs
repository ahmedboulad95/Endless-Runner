using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private Transform lookAt;
    private Vector3 startOffset;
    private Vector3 moveVector;

	void Start () 
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position;
	}
	
	void Update () 
    {
        moveVector = lookAt.position + startOffset;

        moveVector.y = Mathf.Clamp(moveVector.y, -1, 2);

		transform.position = moveVector;
	}
}
