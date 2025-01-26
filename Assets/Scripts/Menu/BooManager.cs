using System.Collections;
using UnityEngine;


public class BooManager : MonoBehaviour
{
    public float minDelay = 10f;  // Минимальное время до старта парения
    public float maxDelay = 16f;  // Максимальное время до старта парения
    public float floatHeight = 100f; // Максимальная высота, на которую будет подниматься снеговик
    public float floatSpeed = 1f; // Скорость подъема
    public float floatSway = 0.10f; // Сила колебания в стороны

    private bool isFloating = false;

    private void Start()
    {
        // Задержка перед началом парения
        StartCoroutine(StartFloatingWithDelay());
    }

    private IEnumerator StartFloatingWithDelay()
    {
        // Генерируем случайную задержку
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);

        // Начинаем парение
        StartCoroutine(Float());
    }

    private IEnumerator Float()
    {
        isFloating = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * floatHeight;

        while (isFloating)
        {
            // Парим вверх и вниз
            float floatAmount = Mathf.Sin(Time.time * floatSpeed) * floatSway;
            transform.position = new Vector3(startPosition.x + floatAmount, Mathf.PingPong(Time.time * floatSpeed, floatHeight) + startPosition.y, startPosition.z);

            yield return null;
        }
    }
}
