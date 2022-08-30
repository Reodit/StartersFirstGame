using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mJumpSound;
    public AudioClip mJumpClip;

    PlayerMovement mPlayer;

    private void Update()
    {
        JumpSoundPlay();
    }

    void JumpSoundPlay()
    {
        if(mPlayer.isJump == true)
        {
            mJumpSound.PlayOneShot(mJumpClip);
        }
    }

}
