using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Items/Weapon")]
public class Weapon : Equipment
{
    [Header("Weapon")]
    public GameObject prefab;
    public float range;
    public float attackRate;
    public float bulletForce;
    public GameObject bulletPrefab;

    [Header("Sounds")]
    public AudioClip attackSound;
    public float pitchRandom;
    public float shotVolume;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}