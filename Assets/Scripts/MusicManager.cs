using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class MusicManager : MonoBehaviour {

    [FMODUnity.EventRef]
    public string MusicLevel1 = "event:/Level1/MusicLevel";

    public static FMOD.Studio.EventInstance CurrentInstance;

    void Start()
    {
        PlayMusic( MusicLevel1 );
    }

    public static void PlayMusic( string eventName, bool isPrio = true )
    {
        if ( CurrentInstance == null )
        {
            CurrentInstance = FMODUnity.RuntimeManager.CreateInstance( eventName );
            CurrentInstance.start();
            Debug.Log( "Instance Created and played" );
            return;
        }

        PLAYBACK_STATE state;
        CurrentInstance.getPlaybackState( out state );

        if ( state.Equals( PLAYBACK_STATE.STOPPED ) || isPrio )
        {
            CurrentInstance.stop( STOP_MODE.ALLOWFADEOUT );
            CurrentInstance = FMODUnity.RuntimeManager.CreateInstance( eventName );
            CurrentInstance.start();
            Debug.Log( "Instance Played" );
        }
    }
}
