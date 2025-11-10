using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MoreMountains.Tools
{
    /// <summary>
    /// A test object to store data to test the MMSaveLoadManager class
    /// </summary>
    [System.Serializable]
    public class MMSaveLoadTestObject
    {
        public List<string> StringList;
    }

    [System.Serializable]
    public class SerializableUnlock
    {
        public int id;
        public bool unlocked = false;
        public SerializableUnlock()
        {
            id = 0;
            unlocked = false;
        }
        public SerializableUnlock(int _id, bool _unlocked)
        {
            id = _id;
            unlocked = _unlocked;
        }

        //public static implicit operator SerializableUnlock(Unlock unlock)
        //{
        //    return new SerializableUnlock(unlock.id, unlock.unlocked);
        //}

    }

    [System.Serializable]
    public class UnlockList
    {
        
        public SerializableUnlock[] unlocks;

        public void PushUnlock(SerializableUnlock unlock)
        {
            //create a temporary list
            List<SerializableUnlock> tempList = new List<SerializableUnlock>();
            //add all the unlocks to the temporary list
            foreach (SerializableUnlock item in unlocks)
            {
                tempList.Add(item);
            }
            //add the new unlock to the temporary list
            tempList.Add(unlock);
            //set the unlocks list to the temporary list
            unlocks = tempList.ToArray();
        }

        public void PushUnlock(int _id, bool _unlocked)
        {
            PushUnlock(new SerializableUnlock(_id, _unlocked));
        }

        public UnlockList()
        {
            unlocks = new SerializableUnlock[0];
        }

        //create an implicit constructor that takes a list of unlocks and converts it to a serializable unlock list
        //public static implicit operator UnlockList(List<SerializableUnlock> unlocks)
        //{
        //    UnlockList unlockList = new UnlockList();
        //    unlockList.unlocks = unlocks.ToArray();
        //    return unlockList;
        //}
        public int Count()
        {
            return unlocks.Length;
        }
        
    }

    [System.Serializable]
    public class SavedData
    {
        
        public float Gold;
        public float xp;
        public int level;

        public List<SerializableUnlock> weaponUnlock = new List<SerializableUnlock>();
        public List<SerializableUnlock> levelUnlock = new List<SerializableUnlock>();

        SavedData()
        {
            this.Gold = 0f;
            this.xp = 0f;
            this.level = 0;
            this.weaponUnlock = null;
            this.levelUnlock = null;
        }

        //create a constructor that updates all the parameters
        public SavedData(float gold, float xp, int level, List<SerializableUnlock> unlockList, List<SerializableUnlock> levelUnlockList)
        {
            this.Gold = gold;
            this.xp = xp;
            this.level = level;
            this.weaponUnlock = unlockList;
            this.levelUnlock = levelUnlockList;
        }

        public SavedData(SavedData data)
        {
            this.Gold = data.Gold;
            this.xp = data.xp;
            this.level = data.level;
            this.weaponUnlock = data.weaponUnlock;
            this.levelUnlock = data.levelUnlock;
        }
    }

    /// <summary>
    /// A simple class used in the MMSaveLoadTestScene to test the MMSaveLoadManager class
    /// </summary>
    public class MMSaveLoadTester : MonoBehaviour
    {
        [Header("Saved object")]
        /// a test object containing a list of strings to save and load
        public MMSaveLoadTestObject TestObject;

        [Header("Saved data")]
        public SavedData Data;
        
        [Header("Save settings")]
        /// the chosen save method (json, encrypted json, binary, encrypted binary)
        public MMSaveLoadManagerMethods SaveLoadMethod = MMSaveLoadManagerMethods.Binary;
        /// the name of the file to save
        public string FileName = "TestObject";
        /// the name of the destination folder
        public string FolderName = "MMTest/";
        /// the extension to use
        public string SaveFileExtension = ".testObject";
        /// the key to use to encrypt the file (if needed)
        public string EncryptionKey = "ThisIsTheKey";

        /// Test button
        [MMInspectorButton("Save")]
        public bool TestSaveButton;
        /// Test button
        [MMInspectorButton("Load")]
        public bool TestLoadButton;
        /// Test button
        [MMInspectorButton("Reset")]
        public bool TestResetButton;

        protected IMMSaveLoadManagerMethod _saveLoadManagerMethod;

        /// <summary>
        /// Saves the contents of the TestObject into a file
        /// </summary>
        protected virtual void Save()
        {
            InitializeSaveLoadMethod();
            MMSaveLoadManager.Save(TestObject, FileName+SaveFileExtension, FolderName);
        }

        /// <summary>
        /// Loads the saved data
        /// </summary>
        protected virtual void Load()
        {
            InitializeSaveLoadMethod();
            TestObject = (MMSaveLoadTestObject)MMSaveLoadManager.Load(typeof(MMSaveLoadTestObject), FileName + SaveFileExtension, FolderName);
        }

        /// <summary>
        /// Resets all saves by deleting the whole folder
        /// </summary>
        protected virtual void Reset()
        {
            MMSaveLoadManager.DeleteSaveFolder(FolderName);
        }

        /// <summary>
        /// Creates a new MMSaveLoadManagerMethod and passes it to the MMSaveLoadManager
        /// </summary>
        protected virtual void InitializeSaveLoadMethod()
        {
            switch(SaveLoadMethod)
            {
                case MMSaveLoadManagerMethods.Binary:
                    _saveLoadManagerMethod = new MMSaveLoadManagerMethodBinary();
                    break;
                case MMSaveLoadManagerMethods.BinaryEncrypted:
                    _saveLoadManagerMethod = new MMSaveLoadManagerMethodBinaryEncrypted();
                    (_saveLoadManagerMethod as MMSaveLoadManagerEncrypter).Key = EncryptionKey;
                    break;
                case MMSaveLoadManagerMethods.Json:
                    _saveLoadManagerMethod = new MMSaveLoadManagerMethodJson();
                    break;
                case MMSaveLoadManagerMethods.JsonEncrypted:
                    _saveLoadManagerMethod = new MMSaveLoadManagerMethodJsonEncrypted();
                    (_saveLoadManagerMethod as MMSaveLoadManagerEncrypter).Key = EncryptionKey;
                    break;
            }
            MMSaveLoadManager.saveLoadMethod = _saveLoadManagerMethod;
        }
    }
}
