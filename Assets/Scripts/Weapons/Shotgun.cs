using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int _pelletsPerShot;
    public override void Shoot()
    {
        for (int i = 0; i < _pelletsPerShot; i++)  base.Shoot();
    }
}
