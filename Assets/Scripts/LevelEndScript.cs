using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndScript : MonoBehaviour
{
    [SerializeField]
    private Transform tenUnit;
    [SerializeField]
    private Transform twentyUnit;
    [SerializeField]
    private Transform generalUnit;

    public void BuildEndOfLevel(Vector3 startPosition, Transform parent)
    {
        float length = 0;
        Vector3 OFFET = startPosition + new Vector3(0f, -62f, 260f);
        Transform unit = Instantiate(tenUnit, OFFET, Quaternion.identity, parent);
        length += unit.position.z + GetZLength(unit) * 0.5f;
        unit.position = new Vector3(0f, unit.position.y + (GetYLength(unit) * 0.5f), length);
        length += 100f;
        unit = Instantiate(twentyUnit, OFFET, Quaternion.identity, parent);
        length += GetZLength(unit) * 0.5f;
        unit.position = new Vector3(0f, unit.position.y + (GetYLength(unit) * 0.5f), length);
        length += 100f;
        for (int i = 3; i < 10; i++)
        {
            unit = Instantiate(generalUnit, OFFET, Quaternion.identity, parent);
            unit.localScale = Vector3.one * i;
            length += GetZLength(unit) * 0.5f;
            unit.position = new Vector3(0f, unit.position.y + (GetYLength(unit) * 0.5f), length);
            Obstacle obstacleComponent = unit.gameObject.AddComponent<Obstacle>();
            obstacleComponent.SetIsLevelEnd(true);
            obstacleComponent.SetSize(i);
            length += 100f;
        }
    }

    private float GetZLength(Transform t)
    {
        return t.GetComponent<MeshRenderer>().bounds.size.z;
    }

    private float GetYLength(Transform t)
    {
        return t.GetComponent<MeshRenderer>().bounds.size.y;
    }
}
