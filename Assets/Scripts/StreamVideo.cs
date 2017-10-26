using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
 

    public RawImage image;

    public VideoClip videoToPlay;
    public VideoClip staticTransition;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;

    public VideoClip[] channels = new VideoClip[6];
    public static int currentChannel;

    public static bool button;
    public static bool stopVid;
    // Use this for initialization
    IEnumerator co;
    void Start()
    {
        button = false;
        stopVid = false;
        Application.runInBackground = true;
        currentChannel = 0; // first channel
                            //StartCoroutine(playVideo());
        

        co = playVideo(); // create an IEnumerator object
        StartCoroutine(co); // start the coroutine
        
    }

    public static void nextChannel()
    {
        if (currentChannel <= 4)
        {
            currentChannel += 1;
            Debug.Log("next channel");
        }
        else if (currentChannel == 5)
        {
            currentChannel = 0;
            Debug.Log("firstchannel");
        }
        else
        {
            Debug.Log("Something Went Horribly Wrong ! ! !");
        }
    }

    public static void pressedButton()
    {
        button = true;
        Debug.Log("Pressed Button");
    }

    IEnumerator playVideo()
    {
        for (; ; ) { 



        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //We want to play from video clip not from url

        videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        //videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        /* ADD CHANNELS */
        videoPlayer.clip = channels[currentChannel];
        videoPlayer.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();
        //videoPlayer.Stop();


        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            if (stopVid == true) // I think this is a bug glitch pls fix
            {
                videoPlayer.time = videoPlayer.frameCount / videoPlayer.frameRate;



                /*audioSource.Stop();
                videoPlayer.Stop();*/


                print("I MADE IT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");



            }
            yield return null;
        }
        stopVid = false;

        Debug.Log("Done Playing Video");
            nextChannel();

    }

    }

    public void Update()
    {
        
        if (button == true)
        {
            stopVid = true;
            
            Debug.Log("Stopped " + Time.time);
            //nextChannel();
            button = false;

            

        }
        
    }
}