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
        private static List<SoundEffect> _sounds = new List<SoundEffect>();

        public static void LoadContent(Game game)
        {
            SoundEffect s1 = game.Content.Load<SoundEffect>("Squelch 1");
            SoundEffect s2 = game.Content.Load<SoundEffect>("Squelch 2");
            SoundEffect s3 = game.Content.Load<SoundEffect>("Squelch 3");

            _sounds.Add(s1);
            _sounds.Add(s2);
            _sounds.Add(s3);
        }

        public static void PlaySquelch()
        {
            Random rand = new Random();
            _sounds[rand.Next(0, 2)].Play();
        }
    }
}
