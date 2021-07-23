using Consts;
using GameEngine.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Main.Cameras
{
    /// <summary>
    /// https://docs.unity3d.com/ScriptReference/Camera.Render.html
    /// </summary>
    [SceneBind(SceneName.Main)]
    public class BlockCamera : Manager<BlockCamera>
    {
        [SerializeField]
        Camera camera;

        // Take a "screenshot" of a camera's Render Texture.
        Texture2D RTImage(Camera camera)
        {
            // The Render Texture in RenderTexture.active is the one
            // that will be read by ReadPixels.
            var currentRT = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;

            // Render the camera's view.
            camera.Render();

            // Make a new texture and read the active Render Texture into it.
            var targetTexture = camera.targetTexture;
            var image = new Texture2D(targetTexture.width, targetTexture.height);
            image.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            image.Apply();

            // Replace the original active Render Texture.
            RenderTexture.active = currentRT;
            return image;
        }

        public BlockCamera SetRawImage(RawImage image)
        {
            image.texture = RTImage(camera);
            return this;
        }
    }
}