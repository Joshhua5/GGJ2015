﻿using UnityEngine;
using System.Collections;


public class Play : MonoBehaviour
{
    public ParticleSystem playEffect;
    public ParticleSystem basicEffect;
    public ParticleSystem smokeEffect;

    private bool _started;

    public void Start()
    {
        basicEffect.transform.position = new Vector3(0, -Screen.height, 0);
        basicEffect.transform.localScale = new Vector3(Screen.width * 3, 0, Screen.height / 3);
        basicEffect.emissionRate = 4 * Screen.width / 10;
        basicEffect.maxParticles = 4000;

        smokeEffect.transform.position = new Vector3(0, Screen.height / 2, -1);
        smokeEffect.transform.localScale = new Vector3(Screen.width * 3, 0, Screen.height * 2);
        smokeEffect.emissionRate = 0;
        smokeEffect.maxParticles = 4000;

        playEffect.transform.position = new Vector3(0, 0, 0);
        playEffect.transform.localScale = new Vector3(Screen.width * 3, 0, Screen.height * 2);
        playEffect.maxParticles = 10000000;
    }

    public void play()
    {
        playEffect.emissionRate = 4 * Screen.width / 2;
        smokeEffect.emissionRate = 4 * Screen.width / 20;

        if (!_started)
        {
            StartCoroutine(LoadNextLevel());
            _started = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(5);

        Application.LoadLevel("Level1");
    }
}
