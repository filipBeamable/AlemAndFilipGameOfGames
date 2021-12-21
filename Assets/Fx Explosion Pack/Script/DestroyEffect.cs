using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	float time;

	void Update ()
	{
		time += Time.deltaTime;
		if (time >= 3f)
			Destroy(gameObject);
	}
}
