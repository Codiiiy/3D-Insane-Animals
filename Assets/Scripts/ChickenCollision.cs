using UnityEngine;

public class ChickenCollision : MonoBehaviour
{
    public Chicken_movement chicken;
    public Wolf_Chase chaser;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!chicken.isJumping || chicken.isRailJump)
        {
            if (other.CompareTag("Seed"))
            {
                Game_Manager.instance.AddPoints(500);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Obstacle"))
            {
                Destroy(other.gameObject);
                HandleObstacleCollision();
            }
            else if (other.CompareTag("PowerUp_DoublePoints"))
            {
                Game_Manager.instance.ActivateDoublePoints();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("PowerUp_SlowTime"))
            {
                Game_Manager.instance.ActivateSlowTime();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("1life"))
            {
                LifeUp();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("EndTrigger"))
            {

                chicken.moveSpeed = 0;
            }
        }
    }

    private void HandleObstacleCollision()
    {
        chaser.OnHit();
        Game_Manager.instance.ObstacleCollision();
    }
    private void LifeUp()
    {
        chaser.LifeUp();
        Game_Manager.instance.LifeUp();
    }
}
