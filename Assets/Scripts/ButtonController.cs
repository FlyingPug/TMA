using UnityEngine;

using UnityEngine;

public class ButtonController : MonoBehaviour, IUsable
{
    [Header("Настройки кнопки")]
    public GameObject[] usableObjects;
    private Animation anim; 

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void Use()
    {
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