using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndScript : MonoBehaviour
{
    [SerializeField]
    private Transform generalUnit;

    public void BuildEndOfLevel(Vector3 startPosition, Transform parent)
    {
        float length = 0;
        Vector3 OFFET = startPosition + new Vector3(0f, -62.8f, 260f);
        Transform unit = Instantiate(generalUnit, OFFET, Quaternion.Euler(0f, 180f, 0), parent);
        length += unit.position.z + GetZLength(unit) * 0.5f;
        Obstacle obstacleComponent = unit.gameObject.GetComponent<Obstacle>();
        unit.localScale = Vector3.one * 6f;
        obstacleComponent.SetSize(6);
        unit.position = new Vector3(0f, unit.position.y + GetYLength(unit) * 0.5f, length);
        length += 50f;
        for (int i = 8; i <= 22; i+=2)
        {
            unit = Instantiate(generalUnit, OFFET, Quaternion.Euler(0f, 180f, 0), parent);
            unit.localScale = Vector3.one * i * 0.75f;
            unit.position = new Vector3(0f, unit.position.y + GetYLength(unit) * 0.5f, length);
            length += 50f + GetZLength(unit) * 0.5f;
            obstacleComponent = unit.gameObject.GetComponent<Obstacle>();
            obstacleComponent.SetSize(i);
        }
    }

    private float GetZLength(Transform t)
    {
        return t.GetComponentInChildren<MeshRenderer>().bounds.size.z;
    }

    private float GetYLength(Transform t)
    {
        return t.GetComponentInChildren<MeshRenderer>().bounds.size.y;
    }
}
