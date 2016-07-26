using UnityEngine;
using System.Collections;

namespace UnityPlatformer {
  [RequireComponent (typeof (BoxCollider2D))]
  public class Ladder : MonoBehaviour {
    // cache
    BoxCollider2D body;
    public bool topDismount = true;
    public bool bottomDismount = true;

    virtual public void Start() {
      body = GetComponent<BoxCollider2D>();
    }

    virtual public Vector3 GetTop() {
      return body.bounds.center + new Vector3(0, body.bounds.size.y * 0.5f, 0);
    }

    virtual public Vector3 GetBottom() {
      return body.bounds.center - new Vector3(0, body.bounds.size.y * 0.5f, 0);
    }

    virtual public bool IsAboveTop(Character c, Vector2 pos) {
      return pos.y > (GetTop().y - c.pc2d.minDistanceToEnv) + 0.001f;
    }

    virtual public bool IsAtTop(Character c, Vector2 pos) {
      return Mathf.Abs(pos.y - GetTop().y) - c.pc2d.minDistanceToEnv < 0.001f;
    }

    virtual public bool IsBelowBottom(Character c, Vector2 pos) {
      return pos.y < (GetBottom().y + c.pc2d.minDistanceToEnv) - 0.001f;
    }

    virtual public bool IsAtBottom(Character c, Vector2 pos) {
      float bottomY = GetBottom().y;

      return Mathf.Abs(pos.y - bottomY) < c.pc2d.skinWidth;
    }

    virtual public void EnableLadder(Character p) {
      p.ladder = this;
      p.EnterArea(Areas.Ladder);
    }

    virtual public void Dismount(Character p) {
      p.ExitState(States.Ladder);
    }

    virtual public void DisableLadder(Character p) {
      Dismount(p);
      p.ExitArea(Areas.Ladder);
      p.ladder = null;
    }

    public virtual void OnTriggerEnter2D(Collider2D o) {
      HitBox h = o.GetComponent<HitBox>();
      if (h && h.type == HitBoxType.EnterAreas) {
        EnableLadder(h.owner.GetComponent<Character>());
      }
    }

    public virtual void OnTriggerExit2D(Collider2D o) {
      HitBox h = o.GetComponent<HitBox>();
      if (h && h.type == HitBoxType.EnterAreas) {
        DisableLadder(h.owner.GetComponent<Character>());
      }
    }
  }
}
