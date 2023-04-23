using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Tags;
using HMUI;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace BSBBChat.BSMLTags
{
    // I'll stash this class for the time being
    // I think I spent too much time on this and I might not even use it in the end.
    // Will see, might eventually even scrap this entire thing
    public class TextInputField : BSMLTag
    {
        public override string[] Aliases => new string[] { "text-input" };

        private InputFieldView _fieldViewPrefab;
        public InputFieldView FieldViewPrefab
        {
            get
            {
                if (!_fieldViewPrefab)
                {
                    FieldInfo placeholderTextField = typeof(InputFieldView).GetField("_placeholderText", BindingFlags.NonPublic | BindingFlags.Instance);
                    _fieldViewPrefab = Resources.FindObjectsOfTypeAll<InputFieldView>().FirstOrDefault(x => x.name == "InputField" && x.useGlobalKeyboard);
                    Console.WriteLine("------------------------");
                    Console.WriteLine(_fieldViewPrefab.name);
                    Console.WriteLine(_fieldViewPrefab.useGlobalKeyboard);
                    GameObject placeholderTextGameObject = (GameObject)placeholderTextField.GetValue(_fieldViewPrefab);
                    Console.WriteLine(placeholderTextGameObject.GetComponent<TextMeshProUGUI>().text);
                    Console.WriteLine("------------------------");
                    var prefabs = Resources.FindObjectsOfTypeAll<InputFieldView>();
                    foreach (var prefab in prefabs)
                    {
                        Console.WriteLine("------------------------");
                        Console.WriteLine(prefab.name);
                        Console.WriteLine(prefab.useGlobalKeyboard);
                        placeholderTextGameObject = (GameObject)placeholderTextField.GetValue(prefab);
                        Console.WriteLine(placeholderTextGameObject.GetComponent<TextMeshProUGUI>().text);
                        Console.WriteLine("------------------------");
                    }
                }
                return _fieldViewPrefab;
            }
        }

        public override GameObject CreateObject(Transform parent)
        {
            GameObject gameObject = Object.Instantiate(FieldViewPrefab.gameObject, parent, false);
            gameObject.name = "TextInputField";
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = 90.0f;
            layoutElement.preferredHeight = 8.0f;
            var transform = gameObject.transform as RectTransform;
            transform.anchoredPosition = new Vector2(0, 0);

            var fieldView = gameObject.GetComponent<InputFieldView>();

            // Use reflection to set the _placeholderText field of the InputFieldView
            // Since its protected in this case. Its really only meant to be set from the unity editor...
            FieldInfo placeholderTextField = typeof(InputFieldView).GetField("_placeholderText", BindingFlags.NonPublic | BindingFlags.Instance);
            GameObject placeholderTextGameObject = (GameObject)placeholderTextField.GetValue(fieldView);
            placeholderTextGameObject.GetComponent<TextMeshProUGUI>().text = "Type message here...";

            // Set _useGlobalKeyboard true
            FieldInfo useGlobalKeyboardField = typeof(InputFieldView).GetField("_useGlobalKeyboard", BindingFlags.NonPublic | BindingFlags.Instance);
            useGlobalKeyboardField.SetValue(fieldView, true);

            var text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            text.alignment = TMPro.TextAlignmentOptions.Midline;
            text.enableWordWrapping = false;

            return gameObject;
        }
    }
}
