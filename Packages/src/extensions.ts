
import { System, ParadoxNotion, GameEngine, SearchPRO, DG, TMPro, UnityEngine, RSG, Unity, MoreTags, Tetris, NodeCanvas, NUnit, Base, MainScene, Common, Puerts, PuertsStaticWrap, SqlCipher4Unity3D, SQLiteNetExtensions, PuertsTest } from 'csharp';
import { $extension } from 'puerts';
export default function() {

    console.log("init extensions");

    $extension(System.String, ParadoxNotion.StringUtils)
    $extension(System.String, GameEngine.Extensions.Strings)
    $extension(System.String, GameEngine.Extensions.Transforms)
    $extension(System.String, GameEngine.Extensions.Files)
    $extension(System.String, SearchPRO.GUIUtils)
    $extension(System.String, DG.DemiEditor.StringExtensions)
    $extension(System.String, TMPro.TMPro_ExtensionMethods)
    $extension(UnityEngine.Renderer, UnityEngine.RendererExtensions)
    $extension(UnityEngine.Texture2D, UnityEngine.ImageConversion)
    $extension(UnityEngine.Texture2D, DG.DemiEditor.TextureExtensions)
    $extension(UnityEngine.ParticleSystem, UnityEngine.ParticlePhysicsExtensions)
    $extension(UnityEngine.Animator, UnityEngine.Animations.AnimatorJobExtensions)
    $extension(UnityEngine.Animator, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.Sprite, UnityEngine.U2D.SpriteDataAccessExtensions)
    $extension(UnityEngine.SpriteRenderer, UnityEngine.U2D.SpriteRendererDataAccessExtensions)
    $extension(UnityEngine.SpriteRenderer, DG.Tweening.DOTweenModuleSprite)
    $extension(UnityEngine.Terrain, UnityEngine.TerrainExtensions)
    $extension(UnityEngine.UIElements.VisualElement, UnityEngine.UIElements.UQueryExtensions)
    $extension(UnityEngine.UIElements.VisualElement, UnityEngine.UIElements.VisualElementExtensions)
    $extension(UnityEngine.WWW, UnityEngine.WWWAudioExtensions)
    $extension(UnityEngine.Video.VideoPlayer, UnityEngine.Experimental.Video.VideoPlayerExtensions)
    $extension(UnityEngine.Material, DG.Tweening.DOTweenModuleUnityVersion)
    $extension(UnityEngine.Material, DG.Tweening.ShortcutExtensions)
    $extension(UnityEngine.Camera, UnityEngine.Rendering.Universal.CameraExtensions)
    $extension(UnityEngine.Camera, DG.Tweening.ShortcutExtensions)
    $extension(UnityEngine.GameObject, UnityEngine.UI.Extensions.MenuExtensions)
    $extension(UnityEngine.GameObject, RSG.Scene.Query.GameObjectExts)
    $extension(UnityEngine.GameObject, ParadoxNotion.ObjectUtils)
    $extension(UnityEngine.GameObject, Unity.Linq.GameObjectExtensions)
    $extension(UnityEngine.GameObject, GameEngine.Extensions.Prefabs)
    $extension(UnityEngine.GameObject, GameEngine.Extensions.Transforms)
    $extension(UnityEngine.GameObject, GameEngine.Extensions.GameObjectExt)
    $extension(UnityEngine.GameObject, GameEngine.Extensions.ReplaceComponent)
    $extension(UnityEngine.GameObject, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.GameObject, MoreTags.Extensions)
    $extension(UnityEngine.GameObject, MoreTags.TagSystem)
    $extension(UnityEngine.GameObject, Tetris.TetrisUtil)
    $extension(UnityEngine.UI.ScrollRect, UnityEngine.UI.Extensions.ScrollRectExtensions)
    $extension(UnityEngine.UI.ScrollRect, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.RectTransform, UnityEngine.UI.Extensions.UIExtensionMethods)
    $extension(UnityEngine.RectTransform, UnityEngine.UI.Extensions.RectTransformExtension)
    $extension(UnityEngine.RectTransform, GameEngine.Extensions.Transforms)
    $extension(UnityEngine.RectTransform, GameEngine.Extensions.RectTransformExtension)
    $extension(UnityEngine.RectTransform, GameEngine.Extensions.UIExtend)
    $extension(UnityEngine.RectTransform, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.RectTransform, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.Canvas, UnityEngine.UI.Extensions.UIExtensionMethods)
    $extension(NodeCanvas.Framework.Node, NodeCanvas.BehaviourTrees.BehaviourTreeExtensions)
    $extension(NodeCanvas.Framework.Node, MoreTags.Extensions)
    $extension(NodeCanvas.Framework.Node, MoreTags.TagSystem)
    $extension(UnityEngine.GUIStyle, ParadoxNotion.GUIStyleUtils)
    $extension(UnityEngine.GUIStyle, DG.DemiEditor.GUIStyleExtensions)
    $extension(UnityEngine.Component, ParadoxNotion.ObjectUtils)
    $extension(UnityEngine.Component, GameEngine.Extensions.Transforms)
    $extension(UnityEngine.Component, GameEngine.Extensions.GameObjectExt)
    $extension(UnityEngine.Component, GameEngine.Extensions.MonoBehaviourExt)
    $extension(UnityEngine.Component, GameEngine.Extensions.ReplaceComponent)
    $extension(UnityEngine.Component, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.Component, Tetris.TetrisUtil)
    $extension(UnityEngine.Component, DG.Tweening.ShortcutExtensions)
    $extension(System.Object, ParadoxNotion.StringUtils)
    $extension(System.Object, GameEngine.Extensions.Strings)
    $extension(System.Object, NUnit.Framework.AssertEx)
    $extension(UnityEngine.Object, GameEngine.Extensions.Strings)
    $extension(UnityEngine.Object, GameEngine.Extensions.Transforms)
    $extension(UnityEngine.Object, GameEngine.Extensions.Files)
    $extension(UnityEngine.Object, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.Object, GameEngine.Extensions.UnityEngineObjectExtention)
    $extension(UnityEngine.Object, Base.Runtime.UnityEngineObjectExt)
    $extension(UnityEngine.Transform, GameEngine.Extensions.Transforms)
    $extension(UnityEngine.Transform, GameEngine.Extensions.GameObjectExt)
    $extension(UnityEngine.Transform, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.Transform, Tetris.TetrisUtil)
    $extension(UnityEngine.Transform, DG.Tweening.DOTweenProShortcuts)
    $extension(UnityEngine.Transform, DG.Tweening.ShortcutExtensions)
    $extension(UnityEngine.Events.UnityEvent, GameEngine.Extensions.AddressablesExtesion)
    $extension(UnityEngine.MonoBehaviour, GameEngine.Extensions.GameObjectExt)
    $extension(UnityEngine.MonoBehaviour, GameEngine.Extensions.MonoBehaviourExt)
    $extension(UnityEngine.MonoBehaviour, GameEngine.Extensions.MonoBehaviourExtension)
    $extension(UnityEngine.MonoBehaviour, Tetris.TimerExtention)
    $extension(UnityEngine.ScriptableObject, GameEngine.Extensions.ScritableExt)
    $extension(System.IO.FileInfo, GameEngine.Extensions.Files)
    $extension(UnityEngine.UI.Image, GameEngine.Extensions.UIExtend)
    $extension(UnityEngine.UI.Image, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.AsyncOperation, GameEngine.Extensions.UnityAsyncOperationAwaiter)
    $extension(UnityEngine.AnimationCurve, GameEngine.Extensions.UnityComponentExtention)
    $extension(UnityEngine.Networking.UnityWebRequestAsyncOperation, GameEngine.Extensions.ExtensionMethods)
    $extension(UnityEngine.Networking.UnityWebRequestAsyncOperation, MainScene.BootScene.Utils.ExtensionMethods)
    $extension(UnityEngine.PolygonCollider2D, Common.PolygonCollider2DExt)
    $extension(Puerts.JsEnv, Tetris.TetrisUtil)
    $extension(Puerts.JsEnv, PuertsStaticWrap.AutoStaticUsing)
    $extension(GameEngine.Models.Contracts.TableBase, Tetris.TetrisUtil)
    $extension(UnityEngine.AudioSource, DG.Tweening.DOTweenModuleAudio)
    $extension(UnityEngine.Audio.AudioMixer, DG.Tweening.DOTweenModuleAudio)
    $extension(UnityEngine.Rigidbody, DG.Tweening.DOTweenModulePhysics)
    $extension(UnityEngine.Rigidbody, DG.Tweening.DOTweenProShortcuts)
    $extension(UnityEngine.Rigidbody2D, DG.Tweening.DOTweenModulePhysics2D)
    $extension(UnityEngine.CanvasGroup, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.UI.LayoutElement, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.UI.Outline, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.UI.Slider, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.UI.Text, DG.Tweening.DOTweenModuleUI)
    $extension(UnityEngine.AndroidJavaObject, UnityEngine.Monetization.AndroidJavaObjectExtensions)
    $extension(NUnit.Framework.Constraints.ConstraintExpression, UnityEngine.TestTools.Constraints.ConstraintExtensions)
    $extension(UnityEngine.Light, DG.Tweening.ShortcutExtensions)
    $extension(UnityEngine.LineRenderer, DG.Tweening.ShortcutExtensions)
    $extension(UnityEngine.TrailRenderer, DG.Tweening.ShortcutExtensions)
    $extension(DG.Tweening.Sequence, DG.Tweening.TweenSettingsExtensions)
    $extension(SqlCipher4Unity3D.SQLiteConnection, SQLiteNetExtensions.Extensions.WriteOperations)
    $extension(PuertsTest.BaseClass, PuertsTest.BaseClassExtension)

}