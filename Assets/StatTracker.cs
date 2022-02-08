using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public IntVariable Food;
    public IntVariable Water;
    public BoolEvent DeathUIActivationEvent;

    public void CheckFoodAndWater()
    {
        if (Food.Value < 0 || Water.Value < 0)
        {
            DeathUIActivationEvent.Raise(true);
        }
    }
}
