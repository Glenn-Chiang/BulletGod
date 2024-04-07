using UnityEngine;
public class HealthCell : Cell
{
    [SerializeField]
    private float _hitPoints = 50;
    public override float HitPoints { get => _hitPoints; protected set => _hitPoints = value; }


}