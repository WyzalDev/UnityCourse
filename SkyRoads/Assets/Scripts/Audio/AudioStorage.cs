// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "DefaultAudioStorage", menuName = "Scriptables/AudioStorage", order = 1)]
    public class AudioStorage : ScriptableObject
    {
        [SerializeField] private List<Sound> sounds;

        public bool TryGetSoundByName(string soundName, out Sound sound)
        {
            sound = sounds.Find(s => s.Name == soundName);
            return sounds.Exists(s => s.Name == soundName);
        }
    }
}