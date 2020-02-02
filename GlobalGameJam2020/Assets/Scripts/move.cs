using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

    [SerializeField]
	private Vector3 velocity;
	private bool moving;

	private void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.name.Equals("Player"))
        {
			moving = true;
			col.collider.transform.SetParent(transform);
        }

    }

	private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
			col.collider.transform.SetParent(null);
        }
    }

	private void FixedUpdate()
    {
        if (moving)
        {
            transform.position += (velocity * Time.deltaTime);
        }
    }


}
