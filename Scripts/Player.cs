using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using System.Security.Cryptography;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    public Transform GroundCheck;
    bool isGrounded;
    Animator anim;
    int curHp;
    [SerializeField] private int MaxHp = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool canTP = true;
    public bool inWater = false;
    bool isClimbing = false;
    public bool canHit = true;
    public GameObject BlueGem, Key;
    int gemCount = 0;
    float hitTimer = 0f;
    public Image PlayerCountDown;
    public SoundEffector soundEffector;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = MaxHp;
    }

    void Update()
    {
        if (transform.position.y < -50)
            Lose();

        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = false;
            if (Input.GetAxis("Horizontal") != 0)
                Flip();                                                                                           
        }
        else
        {
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && (!isClimbing))
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
        }

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing && !inWater)       
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);
    }

    public void RecountHp(int deltaHp)
    {
        if (deltaHp < 0 && canHit)
        {
            curHp = curHp + deltaHp;
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        else if (deltaHp > 0)
        {
            if (curHp >= MaxHp)
                curHp = MaxHp;

            else
                curHp = curHp + deltaHp;
        }
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);

        if (GetComponent<SpriteRenderer>().color.g >= 1f)
        {
            canHit = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            yield break;
        }
        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
            gemCount++;
            Key.SetActive(true);
            CheckGems(Key);
            Key.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            CheckGems(BlueGem);
        }

        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);

                canTP = false;
                StartCoroutine(TPwait());
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            RecountHp(1);
        }

        if (collision.gameObject.tag == "MushRoom")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }

        if (collision.gameObject.tag == "BlueGem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(NoHit());
        }
    }
    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.gravityScale = 0;
            if(Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
                anim.SetInteger("State", 6);
            }
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }
        if (collision.gameObject.tag == "Water" && canHit)
        {
            inWater = true;
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0;
                PlayerCountDown.fillAmount = 1;
                RecountHp(-1);
            }
            else
                PlayerCountDown.fillAmount = 1 - (hitTimer / 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimbing = false;
            rb.gravityScale = 1;
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 7)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }


        if (collision.gameObject.tag == "Water")
        {
            inWater = false;
            hitTimer = 0f;
            PlayerCountDown.fillAmount = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));

        if (collision.gameObject.tag == "QuickSand")
        {
            speed *= 0.25f;
            rb.mass *= 100;
        }
    }

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }

    IEnumerator NoHit()
    {
        StopCoroutine(OnHit());
        gemCount++;
        BlueGem.SetActive(true);
        CheckGems(BlueGem);
        canHit = false;
        BlueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        float t = 5f;
        while (t > 0f)
        {
            canHit = false;
            t -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(Invise(BlueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;
        gemCount--;
        BlueGem.SetActive(false);
        CheckGems(Key);
    }
    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 1.1f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            BlueGem.transform.localPosition = new Vector3(-0.5f, 1.1f, BlueGem.transform.localPosition.z);
            Key.transform.localPosition = new Vector3(0.5f, 1.1f, Key.transform.localPosition.z);
        }
    }

    IEnumerator Invise(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invise(spr, time));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "QuickSand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    public int GetHp()
    {
        return curHp;
    }
}