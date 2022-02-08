using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    public IntVariable Food;
    public IntVariable Water;
    public IntVariable Days;

    public void ResetScene()
    {
        SceneManager.LoadScene(0);

        Food.Value = Food.DefaultValue;
        Water.Value = Water.DefaultValue;
        Days.Value = Days.DefaultValue;
    }
}
