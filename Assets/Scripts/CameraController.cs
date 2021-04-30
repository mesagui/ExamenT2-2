using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject redHatBoyPlayer;


	void Update()
	{

		MovientoCamaraPlayer();
	}


	public void MovientoCamaraPlayer()
	{
		var x = redHatBoyPlayer.transform.position.x + 5;

		transform.position = new Vector3(x, 0, transform.position.z);
	}
}
