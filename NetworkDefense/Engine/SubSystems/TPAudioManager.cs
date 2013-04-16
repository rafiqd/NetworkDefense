using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Engine.SubSystems
{

    public class TPAudioManager
    {
        Dictionary<string, SoundEffect> m_SFX = new Dictionary<string, SoundEffect>();
        Dictionary<string, SoundEffectInstance> m_Music = new Dictionary<string, SoundEffectInstance>();
        TPGame m_GameRef;
        public bool Mute { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public TPAudioManager(TPGame game)
        {
            m_GameRef = game;
            Mute = false;
        }

        public void LoadSong(string key, string path)
        {
            SoundEffect music = m_GameRef.Content.Load<SoundEffect>(path);
            if (!m_Music.ContainsKey(key))
            {
                m_Music.Add(key, music.CreateInstance());
            }
        }

        public void PlaySong(string key, bool loop)
        {
            if (!Mute)
            {
                if (m_Music.ContainsKey(key))
                {
                    if (loop)
                    {
                        m_Music[key].IsLooped = true;
                    }
                    m_Music[key].Play();
                }
            }
        }

        public void SetMasterVolume(float vol)
        {
            SoundEffect.MasterVolume = vol;
        }

        public void StopAllMusic()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> e in m_Music)
            {
                e.Value.Stop();
                e.Value.Dispose();
            }
            m_Music.Clear();
        }

        public void LoadSFX(string path)
        {
            if (m_SFX.ContainsKey(path))
            {
                return;
            }
            m_SFX.Add(path, m_GameRef.Content.Load<SoundEffect>(path));
        }

        public void PlaySFX(string path)
        {
            if (!Mute)
            {
                m_SFX[path].Play();
            }
        }

        public void PlaySFX(string path, float volume, float pitch, float pan)
        {
            if (!Mute)
            {
                if (m_SFX.ContainsKey(path))
                {
                    m_SFX[path].Play(volume, pitch, pan);

                }
            }
        }
    }

}
