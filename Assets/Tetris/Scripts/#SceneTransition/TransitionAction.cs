using System.Collections;
using UnityEngine;

namespace Tetris
{
    public interface ISceneTransition
    {
        Shader GetShader();
        IEnumerator OnScreenObscured(SceneTransitionMgr transitionMgr);

    }
}

