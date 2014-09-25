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

            SoundEffectInstance si1 = s1.CreateInstance();
            SoundEffectInstance si2 = s2.CreateInstance();
            SoundEffectInstance si3 = s3.CreateInstance();

            _sounds.Add(si1);
            _sounds.Add(si2);
            _sounds.Add(si3);
        }

        public static void PlaySquelch()
        {
            Random rand = new Random();
            _sounds[rand.Next(0, 2)].Play();
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
