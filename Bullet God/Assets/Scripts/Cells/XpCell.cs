using UnityEngine;
public class XpCell : Cell
{
    [SerializeField]
    private float _hitPoints = 40;
    public override float HitPoints { get => _hitPoints; protected set => _hitPoints = value; }


}