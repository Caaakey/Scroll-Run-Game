using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterModule : MonoBehaviour
{
    public Animator animator = null;
    public Rigidbody2D rigid = null;
    public Image hpBar = null;
    public float maxHp = 8f;
    public float speed = 1f;
    public float jumpPower = 1.5f;

    private float currentHp;
    private bool isLeft = false;
    private bool isMove = false;

    public int Hit { get; set; } = 0;

    private void Start()
    {
        currentHp = maxHp;
        hpBar.fillAmount = 1f;
    }

    private void Update()
    {
        if (Hit == 2) return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!isLeft) Rotate();
            if (!isMove) Move();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isLeft) Rotate();
            if (!isMove) Move();
        }
        else
        {
            isMove = false;

            if (!animator.GetBool("isJump"))
                animator.Play("Idle", 0);
        }

        if (Input.GetKeyDown(KeyCode.Z) &&
            !animator.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            animator.Play("Jump", 0);
            animator.SetBool("isJump", true);
        }
    }
    
    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, isLeft ? 0 : 180, 0);
        isLeft = !isLeft;
    }

    private void Move()
    {
        isMove = true;

        if (!animator.GetBool("isJump"))
            animator.Play("Move", 0);
    }


    private void FixedUpdate()
    {
        if (Hit == 2) return;

        if (isMove)
            transform.Translate(new Vector3(speed * Time.fixedDeltaTime, 0, 0));

        if (rigid.velocity.y == 0)
        {
            if (animator.GetBool("isJump"))
            {
                animator.SetBool("isJump", false);
                if (isMove) animator.Play("Move", 0);
            }
        }
        else if (rigid.velocity.y <= -0.1f)
        {
            if (animator.GetBool("isJump")) return;

            animator.SetBool("isJump", true);
            animator.Play("JumpDownLoop", 0);
        }
    }

    private const float Damage = 2.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Hit > 0) return;
        if (collision.CompareTag("Bomb"))
        {
            Hit = 2;
            animator.Play("Hit");

            StartCoroutine(UpdateCurrentHp(Damage));

            Vector3 dir = (transform.position - collision.transform.position).normalized;
            rigid.AddForce(dir * 2.5f, ForceMode2D.Impulse);
        }
    }

    public void StartRecover()
        => StartCoroutine(UpdateRecoverTime());

    private const float UpdateHpTime = .33f;
    private IEnumerator UpdateCurrentHp(float damage)
    {
        float prevCurrentHp = currentHp;
        float prevPer = currentHp / maxHp;
        float per = (currentHp - damage) / maxHp;

        float fixedTime = Time.time;
        while (Time.time <= fixedTime + UpdateHpTime)
        {
            currentHp = Mathf.Lerp(
                prevPer,
                per,
                (Time.time - fixedTime) / UpdateHpTime);

            hpBar.fillAmount = currentHp;
            hpBar.color = new Color(hpBar.color.r, currentHp, hpBar.color.b);

            yield return null;
        }

        currentHp = prevCurrentHp - damage;
        yield break;
    }

    private const float RecorverTime = 2f;
    private IEnumerator UpdateRecoverTime()
    {
        Hit = 1;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 1f;

        bool isReverse = false;
        float fixedTime = Time.time;
        while (Time.time <= fixedTime + RecorverTime)
        {
            renderer.color =
                new Color(color.r, color.g, color.b, isReverse ? 0.8f : 0.2f);

            isReverse = !isReverse;
            yield return null;
        }

        renderer.color = color;
        Hit = 0;
        yield break;
    }
}
