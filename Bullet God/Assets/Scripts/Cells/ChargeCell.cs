using UnityEngine;
public class ChargeCell : Cell
{
    [SerializeField] private float _hitPoints = 100;
    public override float HitPoints { get => _hitPoints; protected set => _hitPoints = value; }

    protected override int NumOrbs => 2;
}