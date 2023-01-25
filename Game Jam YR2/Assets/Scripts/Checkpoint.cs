
using UnityEngine;

/// <summary>
/// > Checkpoints will be claimed when the player touches it's trigger collider <br></br>
/// > Order is defined and automatically set up in the Game Manager <br></br>
/// </summary>

[RequireComponent(typeof(BoxCollider2D))]
[ExecuteAlways]
public class Checkpoint : MonoBehaviour
{
    public Vector2 SpawnOffset = Vector2.zero;
    public bool Claimed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.ClaimCheckPoint(this);
        }
    }

    #region Editor & Auto-Config

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2) transform.position + SpawnOffset, new Vector2(1, 2));
    }

    GameManager GM;
    private bool GMAdded = false;

    private void GMAddCheckpoint()
    {
        if (GM.Checkpoints.Exists(x => x.gameObject == gameObject)) return; //Don't add if this gameobject already has a checkpoint added.
        GM.AddCheckpoint(this);
        GMAdded = true;
        Debug.Log($"Automatically added the {gameObject.name} checkpoint to the GameManager");
    }

    private void GMRemoveCheckpoint()
    {
        if (GM && GM.Checkpoints.Contains(this))
        {
            GM.RemoveCheckpoint(this);
            GMAdded = false;
            Debug.Log($"Automatically removed the {gameObject.name} checkpoint from the GameManager");
        }
    }

    /* Commented out because it calls incorrectly sometimes
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!GMAdded)
        {
            GM = FindObjectOfType<GameManager>();
            GMAddCheckpoint();
        }
#endif
    }
    */

    private void Reset()
    {
#if UNITY_EDITOR
        GM = FindObjectOfType<GameManager>(); //this is slow but it works so whatever
        GMAddCheckpoint();
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        GMRemoveCheckpoint();
#endif
    }

    #endregion
}
