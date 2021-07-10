using UnityEngine;

namespace GameEngine.Utils.Sound {

public class Sound : SingletonView<Sound> {

    AudioSource m_bgSound;
    AudioSource m_effectSound;
    public string ResourceDir = "";

    //音乐大小
    public float BgVolume { get => m_bgSound.volume; set => m_bgSound.volume = value; }

    //音效大小
    public float EffectVolume { get => m_effectSound.volume; set => m_effectSound.volume = value; }

    protected override void Awake()
    {
        base.Awake();
        m_bgSound = gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;
        m_effectSound = gameObject.AddComponent<AudioSource>();
    }

    //播放音乐
    public void PlayBg(string audioName)
    {
        //当前正在播的音乐
        string oldName;

        if (m_bgSound.clip == null) {
            oldName = "";
        } else {
            oldName = m_bgSound.clip.name;
        }

        if (oldName != audioName) {
            //音乐路径
            string path;

            if (string.IsNullOrEmpty(ResourceDir)) {
                path = "";
            } else {
                path = ResourceDir + "/" + audioName;
            }

            //加载音乐
            var clip = Resources.Load<AudioClip>(path);

            //播放
            if (clip != null) {
                m_bgSound.clip = clip;
                m_bgSound.Play();
            }
        }
    }

    //停止音乐
    public void StopBg()
    {
        m_bgSound.Stop();
        m_bgSound.clip = null;
    }

    //播放特效
    public void PlayEffect(string audioName)
    {
        //音乐路径
        string path;

        if (string.IsNullOrEmpty(ResourceDir)) {
            path = "";
        } else {
            path = ResourceDir + "/" + audioName;
        }

        //加载音乐
        var clip = Resources.Load<AudioClip>(path);

        //播放
        if (clip != null) {
            m_effectSound.PlayOneShot(clip);
        }
    }

}

}