using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedHatBoyController : MonoBehaviour
{

	private SpriteRenderer sr;
	private Animator _animator;
	private Rigidbody2D rb2d;


	public float speed = 5;
	public float upSpeed = 25;
	private bool puedoSaltar = true;
	private int contadorSaltos = 0;


	public Text scoreTexto;
	public int Score = 0;


	public Text coinsTexto;
	public int coinsScore = 0;


	public Text vidaTexto;
	public int vidaScore = 3;


	//sonido
	public List<AudioClip> AudioClips;
	private AudioSource audioSource;



	//escala
	private Vector2 temp;
	private bool crece = false;

	public GameObject rightBullet; //bala derecha
	public GameObject leftBullet; // bala izquierda



	//parpadea
	private bool esIntangible = false; //
	private float maxIntangibleTime = 0.5f; //maximo
	private float intangibleTime = 0; //timepo


	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();

	}


	void Update()
	{
		scoreTexto.text = "PUNTAJE: " + Score;
		coinsTexto.text = "MONEDAS: " + coinsScore;
		vidaTexto.text = "VIDAS: " + vidaScore;

		setIdleAnimation();


		if (Input.GetKeyDown(KeyCode.A))
		{
			if (!sr.flipX)
			{
				var position = new Vector2(transform.position.x + 1, transform.position.y);
				Instantiate(rightBullet, position, rightBullet.transform.rotation);
			}
			else
			{
				var position = new Vector2(transform.position.x - 2, transform.position.y);
				Instantiate(leftBullet, position, leftBullet.transform.rotation);
			}
			audioSource.PlayOneShot(AudioClips[5]);
		}


		if (Input.GetKey(KeyCode.RightArrow))
		{
			sr.flipX = false;
			setRumAnimation();
			rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			sr.flipX = true;
			setRumAnimation();
			rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
		}

		if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
		{
			setJumpAnimation();
			rb2d.velocity = Vector2.up * upSpeed;

			if (contadorSaltos == 0 && puedoSaltar == true)
			{
				puedoSaltar = true;
				contadorSaltos++;
			}
			else
			{
				puedoSaltar = false;
			}

			audioSource.PlayOneShot(AudioClips[4]);

		}

		if (Input.GetKey(KeyCode.C))
		{
			//sr.flipX = false;

			if (!sr.flipX)
			{
				setSlideAnimation();

				rb2d.velocity = new Vector2(speed, rb2d.velocity.y);

			}
			else
			{
				setSlideAnimation();

				rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
			}
		}

		if (vidaScore == 0)
		{
			setDeadAnimation();
			audioSource.PlayOneShot(AudioClips[1]);
		}

		//Parpadea
		if (esIntangible && intangibleTime < maxIntangibleTime)
		{
			intangibleTime += Time.deltaTime;
			Parpadear();
			DeshabilitarColisionConEnemigo();
			sr.color = Color.red;

		}

		if (intangibleTime >= maxIntangibleTime)
		{
			HabilitarColisionConEnemigo();
			intangibleTime = 0;
			esIntangible = false;
			sr.enabled = true;
			sr.color = Color.white;
		}


	}

	private void DeshabilitarColisionConEnemigo()
	{
		Physics2D.IgnoreLayerCollision(6, 3, true);
	}

	private void HabilitarColisionConEnemigo()
	{
		Physics2D.IgnoreLayerCollision(6, 3, false);
	}

	private void Parpadear()
	{
		sr.enabled = !sr.enabled;
	}


	//Aumenta Tamaño
	private void Crece()
	{
		sr.color = Color.red;
		temp = transform.localScale;
		temp.x += .180f;
		temp.y += .180f;
		transform.localScale = temp;
		crece = true;
	}

	private void RecuperaTamaño()
	{
		sr.color = Color.white;
		temp = transform.localScale;
		temp.x -= .180f;
		temp.y -= .180f;
		transform.localScale = temp;
		crece = false;

	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == 8)
		{
			puedoSaltar = true;
			contadorSaltos = 0;
		}

		if (other.gameObject.tag == "Enemy")
		{
			audioSource.PlayOneShot(AudioClips[2]);
			vidaScore--;

			esIntangible = true;
		}


		if (other.gameObject.tag == "SilverCoin")
		{
			Incrementar5();
			Destroy(other.gameObject);
			audioSource.PlayOneShot(AudioClips[3]);
		}

		if (other.gameObject.tag == "BronzeCoin")
		{
			Incrementar10();
			Destroy(other.gameObject);
			audioSource.PlayOneShot(AudioClips[3]);
		}

		if (other.gameObject.tag == "GoldCoin")
		{
			Incrementar15();
			Destroy(other.gameObject);
			audioSource.PlayOneShot(AudioClips[3]);
		}

		if (other.gameObject.tag == "Estrella")
		{
			Crece();
			Destroy(other.gameObject);

			if (crece == true)
			{
				Invoke("RecuperaTamaño", 5.0f);
			}
		}



	}

	private void Incrementar15()
	{
		Score += 15;
		coinsScore += 1;
	}

	private void Incrementar10()
	{
		Score += 10;
		coinsScore += 1;
	}

	private void Incrementar5()
	{
		Score += 5;
		coinsScore += 1;
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "EnemyJump")
		{
			IncrementarPuntajeEn5();
		}

	}

	private void IncrementarPuntajeEn5()
	{
		Score += 5;
	}



	private void setIdleAnimation()
	{
		_animator.SetInteger("Estado", 0);
	}

	private void setRumAnimation()
	{
		_animator.SetInteger("Estado", 1);
	}

	private void setJumpAnimation()
	{
		_animator.SetInteger("Estado", 2);
	}

	private void setSlideAnimation()
	{
		_animator.SetInteger("Estado", 3);
	}

	private void setDeadAnimation()
	{
		_animator.SetInteger("Estado", 4);
	}

	public void IncrementaPuntajeEn10()
	{
		Score += 10;
	}

	//detecta cuando desaparecemos de la escena
	/*public void OnBecameInvisible()
	{
		transform.position = new Vector3(-1, 0, 0);
	}
	*/
}
