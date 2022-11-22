using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlaveBase : MonoBehaviour{
    public static event MarketTriggers idleEnable;
    public static event MarketTriggers idleDisable;
    public Animator slaveController;
    public AudioSource slaveVoice;
    [SerializeField]
    internal Voicelines voiceLines;
    bool Waving;
    bool Walking;
    bool Talking;
    bool Bowing;
    internal virtual void Start() {
        if(slaveController == null)
        slaveController = VarManager.Singleton.possessedDemon;
        if(slaveController.GetComponent<AudioSource>() != null)
        slaveVoice = slaveController.GetComponent<AudioSource>();
        else
        slaveVoice = slaveController.gameObject.AddComponent<AudioSource>();
    }
    internal void Wave(){
        if(!Waving){
        slaveController.SetBool("Waving", true);
        Waving = true;
        }
        else{
        slaveController.SetBool("Waving", false);
        Waving = false;
        }
    }
    internal void Walk(){
        if(!Walking){
        slaveController.SetBool("Walking", true);
        Walking = true;
        }
        else{
        slaveController.SetBool("Walking", false);
        Walking = false;
        }
    }
    internal void Talk(){
        if(!Talking){
        slaveController.SetBool("Talking", true);
        Talking = true;
        }
        else{
        slaveController.SetBool("Talking", false);
        Talking = false;
        }
    }
    internal void Bow(){
        if(!Bowing){
        slaveController.SetBool("Bowing", true);
        Bowing = true;
        }
        else{
        slaveController.SetBool("Bowing", false);
        Bowing = false;
        }
    }
    internal IEnumerator Whalecome(){
        Idle.slaveReady = false;
        idleDisable.Invoke();
        yield return new WaitForSeconds(1);
        slaveVoice.clip = voiceLines.Welcome;
        slaveVoice.Play();
        Wave();
        yield return new WaitForSeconds(2);
        Wave();
        Talk();
        yield return new WaitForSeconds(7);
        Talk();
        Idle.slaveReady = true;
        yield return null;
        idleEnable.Invoke();
    }
    internal IEnumerator Question(){
        idleDisable.Invoke();
        Idle.slaveReady = false;
        slaveVoice.clip = voiceLines.Question;
        slaveVoice.Play();
        Talk();
        yield return new WaitForSeconds(voiceLines.Question.length + 1);
        Debug.Log("Question finished");
        Idle.slaveReady = true;
        yield return null;
        idleEnable.Invoke();
    }
    internal IEnumerator Respond(){
        Idle.slaveReady = false;
        idleDisable.Invoke();
        Talk();
        yield return new WaitForSeconds(1);
        Talk();
        slaveVoice.clip = voiceLines.Respond;
        slaveVoice.Play();
        yield return new WaitForSeconds(slaveVoice.clip.length);
        Idle.slaveReady = true;
        Talk();
        idleEnable.Invoke();
        yield return null;
    }
    internal IEnumerator IceSuggestion(){
        Idle.slaveReady = false;
        idleDisable.Invoke();
        slaveVoice.clip = voiceLines.iceSuggestion;
        slaveVoice.Play();
        Talk();
        yield return new WaitForSeconds(slaveVoice.clip.length + 3);
        Talk();
        Idle.slaveReady = true;
        idleEnable.Invoke();
        yield return null;
    }
    internal IEnumerator Suggestion(){
        Idle.slaveReady = false;
        idleDisable.Invoke();
        slaveVoice.clip = voiceLines.Suggestion;
        slaveVoice.Play();
        Talk();
        yield return new WaitForSeconds(slaveVoice.clip.length + 3);
        Talk();
        Idle.slaveReady = true;
        idleEnable.Invoke();
        yield return null;
    }
    internal IEnumerator Goodbye(){
        Idle.slaveReady = false;
        idleDisable.Invoke();
        Bow();
        slaveVoice.clip = voiceLines.Bye;
        slaveVoice.Play();
        yield return new WaitForSeconds(2);
        Bow();
        Wave();
        Idle.slaveReady = true;
        idleEnable.Invoke();
        yield return null;
    }
}
[Serializable]
public struct Voicelines{
    public AudioClip Welcome;
    public AudioClip Question;
    public AudioClip Respond;
    public AudioClip iceSuggestion;
    public AudioClip Suggestion;
    public AudioClip Bye;
}