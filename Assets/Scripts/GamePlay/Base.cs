using UnityEngine;

public class Base : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tag.ENEMY_PROJECTILE) || collision.gameObject.CompareTag(Constants.Tag.PLAYER_PROJECTILE))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            var GPM = GameObject.Find(Constants.GameObject.STAGE_ESSENTIAL).GetComponent<GamePlayManager>();
            StartCoroutine(GPM.GameOver());
        }
    }
}
