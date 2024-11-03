using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera _cam;
    [SerializeField] protected Animator _animator;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private TMP_Text _ammoText;

    [Space(1)]
    [Header("Stats")]
    [SerializeField] protected int _ammoPocket, _ammoMax, _damage;
    protected int _ammoCurrent;
    [SerializeField] protected float _reloadDuration, _fireRate, _recoil, _range, _scopeStrength;
    private float _cooldown;
    [SerializeField] private bool _canChangeAuto;
    protected bool _auto, _canShoot, _scoped;
    [SerializeField] Transform _gunPoint, _scopePoint;
    Vector3 _startPosition;

    private void OnEnable() => 
        UpdateAmmoText();
    private void OnDisable()
    {
        // Exit scope when changing weapon
        if(_scoped) 
            Scope();
    }
    protected virtual void Start()
    {
        _ammoCurrent = _ammoMax;
        _cooldown = _fireRate;
        _startPosition = transform.position;
        _canShoot = true;
        UpdateAmmoText();
    }
    private void Update() =>
        ProcessCooldown();
    private void ProcessCooldown() => 
        _cooldown += Time.deltaTime;
    public void TryShoot()
    {
        if (Input.GetMouseButtonDown(0) || _auto)
        {
            if (_cooldown >= _fireRate)
            {
                if(_ammoCurrent > 0 && _canShoot)
                {
                    _ammoCurrent--;
                    Shoot();
                    AnimateShot();
                    UpdateAmmoText();
                }
                _cooldown = 0;
            }
        }
    }
    public void TryReload()
    {
        if (_ammoMax != _ammoCurrent && _ammoPocket > 0 && _canShoot)
        {
            AnimateReload();
            Invoke("Reload", _reloadDuration);
            _canShoot = false;
        }
    }
    public void ChangeAuto()
    {
        if (_canChangeAuto) 
            _auto = !_auto;
    }
    public virtual void Scope()
    {
        _scoped = !_scoped;
        transform.position = _scoped ? _scopePoint.position : _startPosition;
        _cam.fieldOfView += _scopeStrength * (_scoped ? -1 : 1);
    }
    public virtual void Shoot()
    {
        TrailFade trail = GetTrail();
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 drift = new Vector3(Random.Range(-_recoil, _recoil), Random.Range(-_recoil, _recoil), Random.Range(-_recoil, _recoil));
        Ray ray = _cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition + drift);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _range, _mask))
        {
            Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null) 
                enemy.GetDamage(_damage);

            trail.SetPositions(_gunPoint.position, hit.point);
        }
        else
        {
            trail.SetPositions(_gunPoint.position, transform.position + transform.forward * _range);
        }
    }

    private TrailFade GetTrail()
    {
        GameObject buffer = PoolManager.Instance.InstantiateFromPool("bulletTrail", Vector3.zero, Quaternion.identity);
        return buffer.GetComponent<TrailFade>();
    }

    protected virtual void Reload()
    {
        if (_ammoPocket > _ammoMax)
        {
            _ammoPocket -= _ammoMax - _ammoCurrent;
            _ammoCurrent = _ammoMax;
        }
        else
        {
            _ammoCurrent += _ammoPocket;
            _ammoPocket = 0;
        }
        UpdateAmmoText();
        _canShoot = true;
    }
    private void AnimateShot() => 
        _animator.SetTrigger("Shoot");
    private void AnimateReload() => 
        _animator.SetTrigger("Reload");
    protected void UpdateAmmoText() => 
        _ammoText.text = $"{_ammoCurrent}/{_ammoPocket}";
}