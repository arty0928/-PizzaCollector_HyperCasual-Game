using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    //audio
    public AudioSource[] sfxPlayer;
    int sfxCursor;
    public AudioClip[] sfxClip;
    public enum Sfx { coin, gameOver, levelUp };

    private void Awake()
    {
        I = this;
    }

    public void SfxPlay(Sfx type)
    {
        switch (type)
        {
            case Sfx.coin:
                sfxPlayer[sfxCursor].clip = sfxClip[0];
                break;

            case Sfx.gameOver:
                sfxPlayer[sfxCursor].clip = sfxClip[1];
                break;

            case Sfx.levelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[2];
                break;

        }

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }

}

