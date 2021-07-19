using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace MainScene.ChatScene.LoginPage
{
    public class UserProfilePage : SerializedMonoBehaviour
    {
        public enum DataType
        {
            UserName,
            Uid,
            UserPassword,
            DatabasePassword,
            HashID,
            AppID,
            AppSecret,
            RegBtn,
            LoginBtn,
            LostPasswordBtn,
            DeleteBtn
        }

        [Serializable]
        public class Row
        {
            public TMP_Text txtPrompt;
            public string promptStr { get; set; }
            public TMP_InputField input;
        }

        [OdinSerialize]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<DataType, Row> data = new Dictionary<DataType, Row>();

        [OdinSerialize]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<DataType, Button> buttons = new Dictionary<DataType, Button>();

        void Start()
        {
            buttons[DataType.RegBtn].onClick.AddListener(OnRegister);
            buttons[DataType.LoginBtn].onClick.AddListener(OnLogin);
            buttons[DataType.LostPasswordBtn].onClick.AddListener(OnLostPassword);
            buttons[DataType.DeleteBtn].onClick.AddListener(OnDeleteUser);
            data.ForEach(kv => kv.Value.promptStr = kv.Value.txtPrompt.text);
        }

        void Clear()
        {
            data.ForEach(t => t.Value.input.text = string.Empty);
        }

        void OnRegister() { }
        void OnLogin() { }
        void OnLostPassword() { }
        void OnDeleteUser() { }
    }
}