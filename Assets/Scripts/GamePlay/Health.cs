using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    int actualHealth = 1;
    int currentHealth;

    GamePlayManager GPM;

    Animator anime;
    Rigidbody2D rb2d;

    SpriteRenderer body;

    void Start()
    {
        body = gameObject.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
        GPM = GameObject.Find(Constants.GameObject.STAGE_ESSENTIAL).GetComponent<GamePlayManager>();
        SetHealth();
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            rb2d.velocity = Vector2.zero;
            anime.SetTrigger(Constants.Trigger.KILLED);
        }
        else
        {
            InvokeRepeating(Constants.Event.BLINK, 0, 0.3f);
            StartCoroutine(CancelBlink());
        }
    }
    public void SetHealth()
    {
        currentHealth = actualHealth;
    }
    public void SetInvincible()
    {
        currentHealth = 1000;
    }
    void Death()
    {
        if (gameObject.CompareTag(Constants.Tag.PLAYER))
        {
            GPM.SpawnPlayer();
        }
        else
        {
            // TODO: if enemy die - do something
        }
        Destroy(gameObject);
    }

    private void Blink()
    {
        if (body.color == Color.white) body.color = Color.Lerp(Color.white, Color.red, 0.4f);
        else body.color = Color.white;
    }

    private IEnumerator CancelBlink()
    {
        yield return new WaitForSeconds(1);
        body.color = Color.white;
        CancelInvoke(Constants.Event.BLINK);
    }
}