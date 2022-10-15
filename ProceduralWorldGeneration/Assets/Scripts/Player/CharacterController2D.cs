using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	#region Variables
	[SerializeField] private float m_JumpForce = 2000f;
	[Range(0, 10f)] [SerializeField] private float HP = 10f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] public bool m_AirControl = false;
	[SerializeField] private LayerMask platformLayerMask;

	private bool m_Grounded;
	private bool m_FacingRight = true;
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	new private SpriteRenderer renderer;
	private BoxCollider2D BoxCollider2D;
	bool isInvincible = false;

	#endregion

	#region Awake; Update; FixedUpdate
	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		BoxCollider2D = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		if (HP == 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void FixedUpdate()
	{
		m_Grounded = false;

		float extra = .05f;
		RaycastHit2D raycast = Physics2D.BoxCast(BoxCollider2D.bounds.center, BoxCollider2D.bounds.size, 0f, Vector2.down, extra, platformLayerMask);

		if (raycast.collider != null)
			m_Grounded = true;

	}
	#endregion

	#region Move; Flip; CreateCrumbs; Dodge
	public void Move(float move, bool jump)
	{
		if (m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if (move > 0 && !m_FacingRight)
				Flip();
			else if (move < 0 && m_FacingRight)
				Flip();
		}


		if (m_Grounded && jump)
		{
			m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
		}
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	#endregion

	#region OnCollisionEnter2D
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies") && !isInvincible)
		{
			HP--;
			isInvincible = true;
			Invoke("resetInvulnerability", 2);
			renderer.color = new Color(1f, 1f, 1f, 0.5f);
		}
	}

	void resetInvulnerability()
	{
		renderer.color = new Color(1f, 1f, 1f, 1f);
		isInvincible = false;
	}
	#endregion
}

