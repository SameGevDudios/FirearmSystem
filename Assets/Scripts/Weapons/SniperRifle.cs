public class SniperRifle : Weapon
{
    private float _startRecoil;
    protected override void Start()
    {
        base.Start();
        _startRecoil = _recoil;
    }
    public override void Scope()
    {
        base.Scope();
        _recoil = _scoped ? 0 : _startRecoil;
    }
}
