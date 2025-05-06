using UnityEngine;

public class ChickenCollision : MonoBehaviour
{
    public Chicken_movement chicken;
    public Wolf_Chase chaser;

    [SerializeField] private AudioClip crunch2Clip;
    [SerializeField] private AudioClip slowtimeClip;
    [SerializeField] private AudioClip crunchClip;
    [SerializeField] private AudioClip doublepointsClip;
    [SerializeField] private AudioClip[] damageClips;

    private void OnTriggerEnter(Collider other)
    {
        if (!chicken.isJumping || chicken.isRailJump)
        {
            if (other.CompareTag("Seed"))
            {
                Game_Manager.instance.AddPoints(500);
                SoundFXManager.instance.PlaySoundFXClip(crunch2Clip, transform, 1f);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Obstacle"))
            {
                //Debug.Log("Collision");
                SoundFXManager.instance.PlayRandomSoundFXClip(damageClips, transform, 1f);
                Destroy(other.gameObject);
                HandleObstacleCollision();
            }
            else if (other.CompareTag("PowerUp_DoublePoints"))
            {
                Game_Manager.instance.ActivateDoublePoints();
                SoundFXManager.instance.PlaySoundFXClip(doublepointsClip, transform, 1f);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("PowerUp_SlowTime"))
            {
                Game_Manager.instance.ActivateSlowTime();
                SoundFXManager.instance.PlaySoundFXClip(slowtimeClip, transform, 1f);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("1life"))
            {
                SoundFXManager.instance.PlaySoundFXClip(crunchClip, transform, 1f);
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
