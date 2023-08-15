using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveSongs : MonoBehaviour
{
    public AudioClip Song1;
    public AudioClip Song2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToSong1()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = Song1;
        GetComponent<AudioSource>().Play();
    }

    public void ChangeToSong2()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = Song2;
        GetComponent<AudioSource>().Play();
    }
}
