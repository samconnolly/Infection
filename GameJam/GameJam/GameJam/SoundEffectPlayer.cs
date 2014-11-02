using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public class SoundEffectPlayer
    {

        private static List<SoundEffectInstance> _sounds = new List<SoundEffectInstance>();
        public static float Volume = 1.0f;

        public static void LoadContent(Game game)
        {
            SoundEffect s1 = game.Content.Load<SoundEffect>("Squelch 1");
            SoundEffect s2 = game.Content.Load<SoundEffect>("Squelch 2");
            SoundEffect s3 = game.Content.Load<SoundEffect>("Squelch 3");

            SoundEffect v1 = game.Content.Load<SoundEffect>("voice_01");
            SoundEffect v2 = game.Content.Load<SoundEffect>("voice_02");
            SoundEffect v3 = game.Content.Load<SoundEffect>("voice_03");
            SoundEffect v4 = game.Content.Load<SoundEffect>("voice_04");
            SoundEffect v5 = game.Content.Load<SoundEffect>("voice_05");
            SoundEffect v6 = game.Content.Load<SoundEffect>("voice_06");
            SoundEffect v7 = game.Content.Load<SoundEffect>("voice_07");
            SoundEffect v8 = game.Content.Load<SoundEffect>("voice_08");
            SoundEffect v9 = game.Content.Load<SoundEffect>("voice_09");

            SoundEffect splash = game.Content.Load<SoundEffect>("splash");
            SoundEffect zap = game.Content.Load<SoundEffect>("splash2"); 

            SoundEffectInstance si1 = s1.CreateInstance();
            SoundEffectInstance si2 = s2.CreateInstance();
            SoundEffectInstance si3 = s3.CreateInstance();

            SoundEffectInstance vi1 = v1.CreateInstance();
            SoundEffectInstance vi2 = v2.CreateInstance();
            SoundEffectInstance vi3 = v3.CreateInstance();
            SoundEffectInstance vi4 = v4.CreateInstance();
            SoundEffectInstance vi5 = v5.CreateInstance();
            SoundEffectInstance vi6 = v6.CreateInstance();
            SoundEffectInstance vi7 = v7.CreateInstance();
            SoundEffectInstance vi8 = v8.CreateInstance();
            SoundEffectInstance vi9 = v9.CreateInstance();

            SoundEffectInstance splashi = splash.CreateInstance();
            SoundEffectInstance zapi = zap.CreateInstance();

            _sounds.Add(si1);
            _sounds.Add(si2);
            _sounds.Add(si3);

            _sounds.Add(vi1);
            _sounds.Add(vi2);
            _sounds.Add(vi3);
            _sounds.Add(vi4);
            _sounds.Add(vi5);
            _sounds.Add(vi6);
            _sounds.Add(vi7);
            _sounds.Add(vi8);
            _sounds.Add(vi9);

            _sounds.Add(splashi);
            _sounds.Add(zapi);
        }

        public static void PlaySquelch()
        {
            Random rand = new Random();
            _sounds[rand.Next(0, 2)].Play();
        }

        public static void PlayVoice(int n)
        {
            Random rand = new Random();
            //_sounds[n+2].Play();
            _sounds[0].Play();
        }

        public static void PlaySplash()
        {
            Random rand = new Random();
            _sounds[rand.Next(12, 14)].Play();
        }

         public static void PlayZap()
        {
            _sounds[13].Play();
        }

        public static void AdjustVolume(float volume)
        {
            foreach (SoundEffectInstance se in _sounds)
            {
                se.Volume = volume;
                Volume = volume;
            }
            _sounds[0].Play();
        }
    }
}
