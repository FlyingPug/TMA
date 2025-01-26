using UnityEngine;

public class ButtonController : MonoBehaviour, IUsable
{
    [Header("��������� ������")]
    public GameObject[] usableObjects;
    public bool isAirlock;

    private Animation anim; 

    private WeatherController weatherController;

    private void Start()
    {
        anim = GetComponent<Animation>();
        weatherController = GameObject.Find("GameController").GetComponent<WeatherController>();    
    }

    public void Use()
    {   
        if (isAirlock && weatherController.isIndoors)
        {
            weatherController.isIndoors = false;
            weatherController.OnValidate();
        }
        else if (isAirlock && !weatherController.isIndoors)
        {
            weatherController.isIndoors = true;
            weatherController.OnValidate();
        }

        if (anim != null)
        {
            anim.Play();
        }

        foreach (GameObject obj in usableObjects)
        {
            IUsable usable = obj.GetComponent<IUsable>();
            if (usable != null)
            {
                usable.Use();
            }
        }
    }
}