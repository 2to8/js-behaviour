using System.Linq;
using GameEngine.Extensions;
using MoreTags;
using MoreTags.Attributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Tetris.Main
{
    public class IconCamera : Manager<IconCamera>
    {
        [SerializeField]
        RawImage image;

        [SerializeField]
        Vector2 StartPoint;

        [ShowInInspector]
        [PropertyRange(0f, 600f)]
        float x {
            get => StartPoint.x;
            set => StartPoint = new Vector2(value, StartPoint.y);
        }

        [ShowInInspector]
        [PropertyRange(0f, 600f)]
        float y {
            get => StartPoint.y;
            set => StartPoint = new Vector2(StartPoint.x, value);
        }

        [SerializeField, Tags(Id.CraftCamera)]
        Camera camera;

        bool drawed;

        void Update()
        {
            if (!drawed) {
                RTImage();
                drawed = true;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Tests/Tags/Find Tag", false, 100)]
        static void TestFindTag()
        {
            var tagName = Id.BlockCamera;// "id.blockCamera";
            Debug.Log(TagSystem.refs.Select(tk => tk.Key).Join());
            Debug.Log(TagSystem.query.tags(tagName).withTypes(typeof(Camera)).result.Select(go => go.name).Join());
            Debug.Log(TagSystem.Find<Camera>(tagName)?.gameObject.name);
        }
#endif

        // Take a "screenshot" of a camera's Render Texture.
        [Button]
        public void RTImage()
        {
            // The Render Texture in RenderTexture.active is the one
            // that will be read by ReadPixels.
            var currentRT = RenderTexture.active;
            if (camera == null) {
                camera = TagSystem.Find<Camera>(Id.CraftCamera);
                Assert.IsNotNull(camera, $"{Id.CraftCamera} Not Found");
            }

            RenderTexture.active = camera.targetTexture;

            // Render the camera's view.
            camera.Render();

            // Make a new texture and read the active Render Texture into it.
            var tmp = new Texture2D(100, 100);
            tmp.ReadPixels(new Rect(StartPoint.x, StartPoint.y, 100, 100), 0, 0);
            tmp.Apply();

            // Replace the original active Render Texture.
            RenderTexture.active = currentRT;
            image.texture = tmp;
        }
    }
}