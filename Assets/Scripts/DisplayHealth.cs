using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    public GameObject healthSource;
    public GameObject heartPrefab;
    public List<GameObject> heartlist;

    public Sprite heartFull;
    public Sprite heartEmpty;

    public float heartDisplayLength = 4;

    public float sizeMulti = 1;
    // Start is called before the first frame update
    void Start()
    {
        

        int maxHealth = healthSource.gameObject.GetComponent<Health>().MaxHealth;
        heartDisplayLength = (float)(0.25 * maxHealth * sizeMulti);

        for (int i = 0; i < maxHealth; i++)
        {
            float heartPosition = (heartDisplayLength/2)*-1;
            float offset = heartDisplayLength / (maxHealth - 1)*i;
            GameObject heartClone = Instantiate(heartPrefab, new Vector2(heartPosition + offset, 50), new Quaternion(0f, 0f, 0f, 0f), transform.parent = gameObject.transform);
            heartClone.transform.localScale = new Vector2(0.004f * sizeMulti, 0.004f * sizeMulti);
            heartlist.Add(heartClone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < heartlist.Count; i++)
        {
            GameObject currentHeart = heartlist[i];
            Image currentHealthSprite = currentHeart.GetComponent<Image>();

            if ((i+1) <= healthSource.gameObject.GetComponent<Health>().CurrentHealth)
                currentHealthSprite.sprite = heartFull;
            else
                currentHealthSprite.sprite = heartEmpty;
        }
    }
}
