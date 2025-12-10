using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbitalKnifeController : MonoBehaviour
{
    public GameObject orbitalKnifePrefab, KnivesGroup;
    private GameObject player, knifeInstance;
    private AttributesSystem playerAttributes;
    private float radius = 3.4f, baseRotationSpeed = 90f;

    private List<GameObject> orbitalKnives = new();
    private float currentAngle = 0f;

    void Awake()
    {
        player = gameObject;
        playerAttributes = player.GetComponent<AttributesSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(orbitalKnives.Count == 0) return;

        float attackSpeed = playerAttributes.attackSpeed.FinalValue;

        float rotationSpeed = baseRotationSpeed * attackSpeed;
        currentAngle += rotationSpeed * Time.deltaTime;
        currentAngle %= 360f;

        int count = orbitalKnives.Count;
        for(int i = 0; i < count; i++)
        {
            float anglePerKnifer = 360f / count;
            float targetAngle = currentAngle + anglePerKnifer * i;
            float rad = targetAngle * Mathf.Deg2Rad;

            Vector3 localPos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
            orbitalKnives[i].transform.position = player.transform.position + localPos;
            orbitalKnives[i].transform.rotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
        }
    }

    public void AddKnives(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject newKnife = Instantiate(orbitalKnifePrefab, player.transform.position, Quaternion.identity, KnivesGroup.transform);
            newKnife.GetComponent<OrbitalKnifeBehaviour>().Setup(playerAttributes, DamageSource.PLAYER);
            orbitalKnives.Add(newKnife);
        }
    }

    public void RemoveKnives(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            if(orbitalKnives.Count == 0) return;

            GameObject knifeToRemove = orbitalKnives[^1];
            orbitalKnives.Remove(knifeToRemove);
            Destroy(knifeToRemove);
        }
    }

    public int GetKnifeCount()
    {
        return orbitalKnives.Count;
    }
}
