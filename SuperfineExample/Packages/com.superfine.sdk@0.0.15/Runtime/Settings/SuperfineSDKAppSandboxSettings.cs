using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAppSandboxSettings : ISerializationCallbackReceiver
    {
        public enum FileAccess
        {
            [InspectorName("None")]
            NONE = 0,

            [InspectorName("Read-Only")]
            READ_ONLY = 1,

            [InspectorName("Read-Write")]
            READ_WRITE = 2
        }

        [SerializeField, HideInInspector] private bool _serialized = false;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (_serialized == false)
            {
                _serialized = true;
                outgoingConnections = true;
            }
        }

        [Space(5)]
        [InfoBox("Network", EInfoBoxType.Normal)]

        [Label("Incoming Connections (Server)")]
        public bool incomingConnections = false;
        [Label("Outgoing Connections (Client)")]
        public bool outgoingConnections = true;

        [Space(10)]
        [InfoBox("Hardware", EInfoBoxType.Normal)]

        public bool camera = false;
        public bool audioInput = false;
        [Label("USB")]
        public bool usb = false;
        public bool printing = false;
        public bool bluetooth = false;

        [Space(10)]
        [InfoBox("App Data", EInfoBoxType.Normal)]

        public bool contacts = false;
        public bool location = false;
        public bool calendar = false;

        [Space(10)]
        [InfoBox("File Access", EInfoBoxType.Normal)]

        public FileAccess userSelectedFile = FileAccess.NONE;
        public FileAccess downloadsFolder = FileAccess.NONE;
        public FileAccess picturesFolder = FileAccess.NONE;
        public FileAccess musicFolder = FileAccess.NONE;
        public FileAccess moviesFolder = FileAccess.NONE;
    }
}
