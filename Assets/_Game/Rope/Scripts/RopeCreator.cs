using Dt.Attribute;
using UnityEditor;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    [SerializeField, Required]
    private Joint2D chuckPrefab;

    [SerializeField, Required]
    private Joint2D lastChuckPrefab;

    [SerializeField, Required]
    private Joint2D root;

    [SerializeField]
    private float yOffset = -1f;

    [Title("Colors")]
    [SerializeField]
    private Color oddColor;

    [SerializeField]
    private Color evenColor;

    [Line]
    [SerializeField, Required]
    private Rope rope;

    [Line]
    [SerializeField]
    private bool linkToBall;

    [Button]
    private void Create(int quantity)
    {
        Clean();
        Joint2D lastJoint = this.root;
        this.rope.AddChunk(this.root.GetComponent<Chunk>());
        for (int i = 0; i < quantity; i++)
        {
            Joint2D chunk = Instantiate(this.chuckPrefab, transform);
            lastJoint.connectedBody = chunk.GetComponent<Rigidbody2D>();
            bool isFirstChunk = i == 0;
            Vector3 pos = lastJoint.transform.localPosition;
            if (isFirstChunk)
            {
                pos = Vector3.down * -0.5f;
            }

            chunk.transform.localPosition = pos - Vector3.down * this.yOffset;
            lastJoint = chunk;
            bool isOdd = i % 2 == 1;
            chunk.GetComponentInChildren<SpriteRenderer>().color =
                isOdd ? this.oddColor : this.evenColor;
            this.rope.AddChunk(chunk.GetComponent<Chunk>());
        }

        Joint2D lastChunk = Instantiate(this.lastChuckPrefab, transform);
        lastJoint.connectedBody = lastChunk.GetComponent<Rigidbody2D>();
        lastChunk.transform.localPosition =
            lastJoint.transform.localPosition - Vector3.down * this.yOffset;
        Color color = quantity % 2 == 0 ? this.oddColor : this.evenColor;
        lastChunk.GetComponentInChildren<SpriteRenderer>().color = color;
        this.rope.AddChunk(lastChunk.GetComponent<Chunk>());

        LinkToBall(lastChunk);
        EditorUtility.SetDirty(gameObject);
    }

    private void LinkToBall(Joint2D lastChunk)
    {
        if (this.linkToBall)
        {
            lastChunk.connectedBody =
                FindObjectsByType<Ball>(FindObjectsSortMode.None)[0]
                    .GetComponent<Rigidbody2D>();
        }
    }

    [Button]
    private void Clean()
    {
        this.rope.CleanChucks();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i) == this.root.transform) continue;
            if (transform.GetChild(i).GetComponent<Chunk>() != null)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}