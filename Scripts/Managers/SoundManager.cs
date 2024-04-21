using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{



    [SerializeField] private AudioSource audio;
    [SerializeField]AudioClip[] MusicClips;
    [SerializeField]AudioClip[] BatClips;
    [SerializeField]AudioClip[] BigBearClips;
    [SerializeField]AudioClip[] SoundsClips;
    [SerializeField]AudioClip[] FrogClips;
    [SerializeField]AudioClip[] PlayerClips;
    [SerializeField] AudioClip[] MenuClips;







    private void Awake ()
    {

        audio = GetComponent<AudioSource>();



    }

    #region BatSounds
    public void BatDeathSound ()
    {
        foreach (AudioClip clip in BatClips)
        {
            if (clip.name == "BAT_DEATH")
            {
                audio.PlayOneShot(clip);

            }

        }

    }
    public void BatAttackSound ()
    {
        foreach (AudioClip clip in BatClips)
        {
            if (clip.name == "BAT_ATTACK")
            {
                audio.PlayOneShot(clip);

            }

        }

    }
    #endregion


    #region HUGE_BEAR
    public void HugeBearReliseSound ()
    {
        foreach (AudioClip clip in BigBearClips)
        {
            if (clip.name == "BIG_BEAR_RELEASE")
            {
                audio.PlayOneShot(clip);

            }

        }

    }
    public void HugeBearDeathSound ()
    {
        foreach (AudioClip clip in BigBearClips)
        {
            if (clip.name == "ENEMY_DEATH_sfx")
            {
                audio.PlayOneShot(clip);

            }

        }

    }





    #endregion


    #region FROG_DEATH
    public void FrogDeathSound ()
    {
        foreach (AudioClip clip in FrogClips)
        {
            if (clip.name == "FROG_DEATH")
            {
                audio.PlayOneShot(clip);

            }

        }

    }


    #endregion


    #region PLAYER_SOUNDS

    public void PlayerGetHitSound ()
    {
        foreach (AudioClip clip in PlayerClips)
        {
            if (clip.name == "GETTIN_HIT")
            {
                audio.PlayOneShot(clip);

            }

        }

    }
    public void PlayerDeathSound ()
    {
        foreach (AudioClip clip in PlayerClips)
        {
            if (clip.name == "DEATH")
            {
                audio.PlayOneShot(clip);

            }

        }

    }
    public void PlayerAttackSound ()
    {
        foreach (AudioClip clip in PlayerClips)
        {
            if (clip.name == "SCISSORS")
            {
                audio.PlayOneShot(clip);

            }

        }

    }



    #endregion



    #region MUSIC

    public void StartBossFight ()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }



    #endregion


    #region STOPA_ALL_AND_RESUME
    public void StopMusicAndSound ()
    {

        AudioListener.pause = true;



    }
    public void StartMusicAfterPause ()
    {

        AudioListener.pause = false;

    }

    #endregion

    #region MENU_SOUNDS

    public void MenuClickSound ()
    {
        foreach (AudioClip clip in MenuClips)
        {
            if (clip.name == "click")
            {
                audio.PlayOneShot(clip);

            }

        }



    }
    public void MenuReturnSound ()
    {
        foreach (AudioClip clip in MenuClips)
        {
            if (clip.name == "return")
            {
                audio.PlayOneShot(clip);

            }

        }



    }



    #endregion
}