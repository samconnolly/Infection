using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
//using Microsoft.DirectX.AudioVideoPlayback;


namespace GameJam
{
    public class SoundEffectPlayer
    {
        
        // xact
        private static AudioEngine engine;
        private static SoundBank soundbank;
        private static WaveBank wavebank;
        private static AudioCategory soundCategory;
        private static float _volume;

        public static void LoadContent(Game game)
        {

            // xact
            engine = new AudioEngine("Content\\infectionSound.xgs");
            soundbank = new SoundBank(engine, "Content\\Sound Bank.xsb");
            wavebank = new WaveBank(engine, "Content\\Wave Bank.xwb");
            engine.Update();
            soundCategory = engine.GetCategory("Sound");
        }

        public static float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public static void PlaySquelch()
        {
            Cue cue = soundbank.GetCue("squelch");
            cue.Play();
        }

        public static void PlayVoice(int n)
        {
            Cue cue = soundbank.GetCue("squelch");
            cue.Play();            
        }

        public static void PlaySplash()
        {
            Cue cue = soundbank.GetCue("splash");
            cue.Play();
        }

        public static void AdjustVolume(float volume,bool silent=false)
        {
            soundCategory.SetVolume(volume);
            _volume = volume;
            if (silent == false)
            {
                Cue cue = soundbank.GetCue("squelch");
                cue.Play();
            }
        }
    }
}
