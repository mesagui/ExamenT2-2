using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
	public float velocityX = 10f;
	private Rigidbody2D rb;
	private RedHatBoyController redBoyPlayerController;


	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		redBoyPlayerController = FindObjectOfType<RedHatBoyController>();
		Destroy(gameObject, 3);// personaje lanza bala y se destruye despues de 3
	}


	void Update()
	{
		rb.velocity = Vector2.right * velocityX;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "Enemy")
		{
			Destroy(other.gameObject);
			Destroy(this.gameObject);

			//redBoyPlayerController.IncrementarPuntajeEn10();
		}
	}
}
