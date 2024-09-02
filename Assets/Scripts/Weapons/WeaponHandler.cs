using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapon;
    private Weapon _currentWeapon;
    private void Update()
    {
        CheckWeaponChange();
        if (_currentWeapon != null && _currentWeapon.gameObject.activeSelf)
        {
            CheckShoot();
            CheckReload();
            CheckAutoChange();
            CheckScope();
        }
    }
    private void CheckShoot()
    {
        if (Input.GetMouseButton(0)) _currentWeapon.TryShoot();
    }
    private void CheckReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) _currentWeapon.TryReload();
    }
    private void CheckAutoChange()
    {
        if (Input.GetKeyDown(KeyCode.X)) _currentWeapon.ChangeAuto();
    }
    private void CheckScope()
    {
        if (Input.GetMouseButtonDown(1)) _currentWeapon.Scope();
    }

    private void CheckWeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeWeapon(3);
    }

    public void ChangeWeapon(int index)
    {
        if (_currentWeapon != null) _currentWeapon.gameObject.SetActive(false);
        _currentWeapon = _weapon[index];
        _currentWeapon.gameObject.SetActive(true);
    }
}
