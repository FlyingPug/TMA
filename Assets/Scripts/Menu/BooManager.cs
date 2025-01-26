using System.Collections;
using UnityEngine;


public class BooManager : MonoBehaviour
{
    public float minDelay = 10f;  // ����������� ����� �� ������ �������
    public float maxDelay = 16f;  // ������������ ����� �� ������ �������
    public float floatHeight = 100f; // ������������ ������, �� ������� ����� ����������� ��������
    public float floatSpeed = 1f; // �������� �������
    public float floatSway = 0.10f; // ���� ��������� � �������

    private bool isFloating = false;

    private void Start()
    {
        // �������� ����� ������� �������
        StartCoroutine(StartFloatingWithDelay());
    }

    private IEnumerator StartFloatingWithDelay()
    {
        // ���������� ��������� ��������
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);

        // �������� �������
        StartCoroutine(Float());
    }

    private IEnumerator Float()
    {
        isFloating = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * floatHeight;

        while (isFloating)
        {
            // ����� ����� � ����
            float floatAmount = Mathf.Sin(Time.time * floatSpeed) * floatSway;
            transform.position = new Vector3(startPosition.x + floatAmount, Mathf.PingPong(Time.time * floatSpeed, floatHeight) + startPosition.y, startPosition.z);

            yield return null;
        }
    }
}
