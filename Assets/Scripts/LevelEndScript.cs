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
        Vector3 OFFET = startPosition + new Vector3(0f, -64f, 260f);
        Transform unit = Instantiate(generalUnit, OFFET, Quaternion.Euler(0f, 180f, 0), parent);
        length += unit.position.z + GetZLength(unit) * 0.5f;
        Obstacle obstacleComponent = unit.gameObject.GetComponent<Obstacle>();
        obstacleComponent.SetSize(1);
        unit.position = new Vector3(0f, unit.position.y + GetYLength(unit), length);
        length += 50f;
        for (int i = 6; i < 20; i++)
        {
            unit = Instantiate(generalUnit, OFFET, Quaternion.Euler(0f, 180f, 0), parent);
            unit.localScale = Vector3.one * i;
            length += GetZLength(unit) * 0.5f;
            length += 50f;
            unit.position = new Vector3(0f, unit.position.y + GetYLength(unit), length);
            obstacleComponent = unit.gameObject.GetComponent<Obstacle>();
            obstacleComponent.SetSize(i-4);
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
