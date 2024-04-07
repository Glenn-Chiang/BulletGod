using UnityEngine;
public class PowerCell : Cell
{
    [SerializeField]
    private float _hitPoints = 200;
    public override float HitPoints { get => _hitPoints; protected set => _hitPoints = value; }


}