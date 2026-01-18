using UnityEngine;

namespace Coop.Combat
{
    public class Weapon : MonoBehaviour
    {
        public float weaponSpeed = 5.0f;
        public float weaponLife = 3.0f;
        public float weaponCooldown = 1.0f;
        public int weaponAmmo = 15;

        public GameObject weaponBullet;
        public Transform weaponFirePosition;
    }
}