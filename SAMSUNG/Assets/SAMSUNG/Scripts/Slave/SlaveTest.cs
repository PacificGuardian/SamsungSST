using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SlaveTest : SlaveBase {
    int Stage = 0;

    internal override void Start()
    {
        base.Start();
        StartCoroutine(Whalecome());
        Stage++;
    }
    private void Update() {
        if(Input.anyKeyDown){
            switch(Stage){
                case 1:
                StartCoroutine(Question());
                Debug.Log("Question");
                break;
                case 2:
                StartCoroutine(Respond());
                Debug.Log("Response");
                break;
                case 3:
                Talk();
                Debug.Log("Stopped Waiting");
                break;
                case 4:
                StartCoroutine(Suggestion());
                Debug.Log("Suggestion");
                break;
                case 5:
                StartCoroutine(Goodbye());
                Debug.Log("Bye");
                break;
            }
            Stage++;
        }
    }
}