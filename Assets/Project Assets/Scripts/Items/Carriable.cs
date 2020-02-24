/*
 * Carriable.cs
 * Created by: Kaelan Bartlett
 * Created on: 24/2/2020 (dd/mm/yy)
 * Created for: Items that can be carried by other objects
 */

using UnityEngine;

public abstract class Carriable : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [HideInInspector] public Rigidbody _Rigidbody;


    [Header("Settings")]
    protected bool _IsEquipped;
    public Sprite _Icon;

    [Header("Positioning")]
    [SerializeField] protected Vector3 _DefaultPosition;
    [SerializeField] protected float _PositionSpeed;
    protected Vector3 _TargetPosition;

    [SerializeField] protected Quaternion _DefaultRotation;
    [SerializeField] protected float _RotationSpeed;
    protected Quaternion _TargetRotation;


    private void Start()
	{
        _Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (_IsEquipped)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _TargetPosition, _PositionSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _DefaultRotation, _RotationSpeed * Time.deltaTime);
        }
    }

    //Types: 
    //1 -> Down
    //2 -> Up
    //3 -> Hold
    public abstract void UseOne(int type);
    public abstract void UseTwo(int type);
    public abstract void UseThree(int type);
    public abstract void Drop();
    public void OnInteractStart(GameObject interactingParent)
    {
        if (interactingParent.CompareTag("Player"))
        {
            var player = interactingParent.GetComponent<Player>();

            // If the player has a gun in their hand, then drop it
            if (player._CurrentItem != null)
                player.Drop();

            // Put ourself onto the player
            Pickup(Camera.main.transform);
            player._CurrentItem = this;
        }
    }
    public void Pickup(Transform parent)
    {
        // Turns the colliders off so we don't interact with the player or environment
        Global.RecursiveSetColliders(transform, false);
        transform.parent = parent;

        _Rigidbody.isKinematic = true;
        _IsEquipped = true;
    }
}
