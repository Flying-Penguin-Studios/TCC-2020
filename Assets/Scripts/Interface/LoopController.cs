using UnityEngine;

public class LoopController : MonoBehaviour
{
    public GameObject GameObjVP;
    public GameObject RawImageOther;
    int NormalLoopDuration = 2030;
    int AddedFrames = 48;

    void Start() {
        var ownVP = GetComponent<UnityEngine.Video.VideoPlayer>();
        ownVP.sendFrameReadyEvents = true;
        ownVP.frameReady += Transition;
        ownVP.Prepare();
    }

    void Transition(UnityEngine.Video.VideoPlayer vp, long frame) {
        var ownVP = GetComponent<UnityEngine.Video.VideoPlayer>();
        var secondVP = GameObjVP.GetComponent<UnityEngine.Video.VideoPlayer>();

        if(frame < AddedFrames && secondVP.isPlaying) {
            frame = secondVP.frame - NormalLoopDuration;

        } else if(frame == NormalLoopDuration) {
            secondVP.Play();
            //Debug.Log("transition p1 raised");

        } else if(frame == (long)ownVP.frameCount-1) {
            RectTransform order = RawImageOther.GetComponent<RectTransform>();
            order.SetAsLastSibling();
            ownVP.Stop();
            ownVP.Prepare();
            //Debug.Log("transition p2 raised");
        }
    }
}
