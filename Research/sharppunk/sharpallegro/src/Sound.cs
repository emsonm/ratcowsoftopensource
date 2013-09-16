using System;

namespace sharpallegro
{
    /// <summary>
    /// driver for playing digital sfx
    /// </summary>
    public class DIGI_DRIVER : ManagedPointer
    {
        public DIGI_DRIVER(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// driver ID code
        /// </summary>
        public int id
        {
            get
            {
                return ReadInt(0);
            }
        }

        /// <summary>
        /// driver name
        /// </summary>
        public string name
        {
            get
            {
                return ReadString(sizeof(Int32));
            }
        }

        /// <summary>
        /// description string
        /// </summary>
        public string desc
        {
            get
            {
                return ReadString(2 * sizeof(Int32));
            }
        }

        /// <summary>
        /// ASCII format name string
        /// </summary>
        public string ascii_name
        {
            get
            {
                return ReadString(3 * sizeof(Int32));
            }
        }

        /// <summary>
        /// available voices
        /// </summary>
        public int voices
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }
        }

        /// <summary>
        /// voice number offset
        /// </summary>
        public int basevoices
        {
            get
            {
                return ReadInt(5 * sizeof(Int32));
            }
        }

        /// <summary>
        /// maximum voices we can support
        /// </summary>
        public int max_voices
        {
            get
            {
                return ReadInt(6 * sizeof(Int32));
            }
        }

        /// <summary>
        /// default number of voices to use
        /// </summary>
        public int def_voices
        {
            get
            {
                return ReadInt(7 * sizeof(Int32));
            }
        }

        public int rec_cap_bits
        {
            get
            {
                return ReadInt(38 * sizeof(Int32));
            }
        }

        public int rec_cap_stereo
        {
            get
            {
                return ReadInt(39 * sizeof(Int32));
            }
        }

        public static implicit operator DIGI_DRIVER(IntPtr pointer)
        {
            return new DIGI_DRIVER(pointer);
        }
    }

    /// <summary>
    /// driver for playing midi music
    /// </summary>
    public class MIDI_DRIVER : ManagedPointer
    {
        public MIDI_DRIVER(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// driver ID code
        /// </summary>
        public int id
        {
            get
            {
                return ReadInt(0);
            }
        }

        /// <summary>
        /// driver name
        /// </summary>
        public string name
        {
            get
            {
                return ReadString(sizeof(Int32));
            }
        }

        /// <summary>
        /// description string
        /// </summary>
        public string desc
        {
            get
            {
                return ReadString(2 * sizeof(Int32));
            }
        }

        /// <summary>
        /// ASCII format name string
        /// </summary>
        public string ascii_name
        {
            get
            {
                return ReadString(3 * sizeof(Int32));
            }
        }

        /// <summary>
        /// available voices
        /// </summary>
        public int voices
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }
        }

        /// <summary>
        /// voice number offset
        /// </summary>
        public int basevoice
        {
            get
            {
                return ReadInt(5 * sizeof(Int32));
            }
        }

        /// <summary>
        /// maximum voices we can support
        /// </summary>
        public int max_voices
        {
            get
            {
                return ReadInt(6 * sizeof(Int32));
            }
        }

        /// <summary>
        /// default number of voices to use
        /// </summary>
        public int def_voices
        {
            get
            {
                return ReadInt(7 * sizeof(Int32));
            }
        }

        /// <summary>
        /// reserved voice range
        /// </summary>
        public int xmin
        {
            get
            {
                return ReadInt(8 * sizeof(Int32));
            }
        }

        /// <summary>
        /// reserved voice range
        /// </summary>
        public int xmax
        {
            get
            {
                return ReadInt(9 * sizeof(Int32));
            }
        }

        public static implicit operator MIDI_DRIVER(IntPtr pointer)
        {
            return new MIDI_DRIVER(pointer);
        }
    }

    /// <summary>
    /// a sample
    /// </summary>
    public class SAMPLE : ManagedPointer
    {
        SAMPLE(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// 8 or 16
        /// </summary>
        public int bits
        {
            get
            {
                return ReadInt(0);
            }
        }

        /// <summary>
        /// sample type flag
        /// </summary>
        public int stereo
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }

        /// <summary>
        /// sample frequency
        /// </summary>
        public int freq
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }
        }

        /// <summary>
        /// 0-255
        /// </summary>
        public int priority
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }
        }

        /// <summary>
        /// length (in samples)
        /// </summary>
        public ulong len
        {
            get
            {
                return (ulong)ReadInt(4 * sizeof(Int32));
            }
        }

        /// <summary>
        /// loop start position
        /// </summary>
        public ulong loop_start
        {
            get
            {
                return (ulong)ReadInt(5 * sizeof(Int32));
            }
        }

        /// <summary>
        /// loop finish position
        /// </summary>
        public ulong loop_end
        {
            get
            {
                return (ulong)ReadInt(6 * sizeof(Int32));
            }
        }

        /// <summary>
        /// for internal use by the driver
        /// </summary>
        public ulong param
        {
            get
            {
                return (ulong)ReadInt(7 * sizeof(Int32));
            }
        }

        /// <summary>
        /// sample data
        /// </summary>
        public IntPtr data
        {
            get
            {
                return ReadPointer(8 * sizeof(Int32));
            }
        }

        public static implicit operator SAMPLE(IntPtr pointer)
        {
            return new SAMPLE(pointer);
        }
    }

    public class MIDI_TRACK : ManagedPointer
    {
        public MIDI_TRACK(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// MIDI message stream
        /// </summary>
        public IntPtr data
        {
            get
            {
                return ReadPointer(0);
            }
        }

        /// <summary>
        /// length of the track data
        /// </summary>
        public int len
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }
    }

    /// <summary>
    /// a midi file
    /// </summary>
    public class MIDI : ManagedPointer
    {
        /* Theoretical maximums: */
        const int MIDI_VOICES = 64;       /* actual drivers may not be */
        const int MIDI_TRACKS = 32;       /* able to handle this many */

        public MIDI(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// number of ticks per quarter note
        /// </summary>
        public int divisions
        {
            get
            {
                return ReadInt(0);
            }
        }

        public MIDI_TRACK this[int index]
        {
            get
            {
                return new MIDI_TRACK(Offset(pointer, 2 * index * sizeof(Int32)));
            }
        }
        public static implicit operator MIDI(IntPtr pointer)
        {
            return new MIDI(pointer);
        }
    }

    public class AUDIOSTREAM : ManagedPointer
    {
        public AUDIOSTREAM(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// the voice we are playing on
        /// </summary>
        public int voice
        {
            get
            {
                return ReadInt(0);
            }
        }

        /// <summary>
        /// the sample we are using
        /// </summary>
        public IntPtr samp
        {
            get
            {
                return ReadPointer(sizeof(Int32));
            }
        }

        /// <summary>
        /// buffer length
        /// </summary>
        public int len
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }
        }

        /// <summary>
        /// number of buffers per sample half
        /// </summary>
        public int bufcount
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }
        }

        /// <summary>
        /// current refill buffer
        /// </summary>
        public int bufnum
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }
        }

        /// <summary>
        /// which half is currently playing
        /// </summary>
        public int active
        {
            get
            {
                return ReadInt(5 * sizeof(Int32));
            }
        }

        /// <summary>
        /// the locked buffer
        /// </summary>
        public IntPtr locked
        {
            get
            {
                return ReadPointer(6 * sizeof(Int32));
            }
        }
        public static implicit operator AUDIOSTREAM(IntPtr pointer)
        {
            return new AUDIOSTREAM(pointer);
        }
    }
}
