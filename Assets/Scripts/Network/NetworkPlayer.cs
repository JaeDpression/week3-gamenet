using System;
using Fusion;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private TextMeshPro _nameLabel;

    [Header("Networked Properties")]
    [Networked] public Vector3 NetworkedPosition { get; set; }
    [Networked] public Color PlayerColor { get; set; }
    [Networked] public NetworkString<_32> PlayerName { get; set; }

    

    #region Fusion Callbacks
    public override void Spawned()
    {
        if (HasInputAuthority) 
        {
            RPC_SetPlayerName(NetworkManager.SelectedName);
            RPC_SetPlayerColor(NetworkManager.SelectedColor);
            
            Camera.main.gameObject.AddComponent<SimpleFollow>().target = this.transform;
        }

        if (HasStateAuthority) 
        {
            PlayerColor = Random.ColorHSV();
        }
        
        if (HasInputAuthority)
        {
            
            RPC_SetPlayerName(MenuManager.LocalPlayerName);
            RPC_SetPlayerColor(MenuManager.LocalPlayerColor);

            
            Camera.main.GetComponent<CameraFollow>().SetTarget(this.transform);
        }
    }
    
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (!GetInput(out NetworkInputData input)) return;
        
        
        this.transform.position +=
            new Vector3(input.InputVector.normalized.x,
                0,
                input.InputVector.normalized.y)
            * Runner.DeltaTime;
            
            
        NetworkedPosition = this.transform.position;
    }

    public override void Render()
    {
        this.transform.position = NetworkedPosition;
        if (_meshRenderer != null && _meshRenderer.material.color != PlayerColor)
        {
            _meshRenderer.material.color = PlayerColor;
        }
        
        this.transform.position = NetworkedPosition;
        
        if (_meshRenderer != null)
            _meshRenderer.material.color = PlayerColor;
        
        if (_nameLabel != null)
        {
            _nameLabel.text = PlayerName.ToString();
            _nameLabel.color = PlayerColor; 
            
            
            _nameLabel.transform.rotation = Quaternion.LookRotation(_nameLabel.transform.position - Camera.main.transform.position);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetPlayerColor(Color color)
    {
        if (HasStateAuthority)
        {
            this.PlayerColor = color;
        }
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetPlayerName(string color)
    {
        if (HasStateAuthority)
        {
            this.PlayerName = color;
        }
        //example of how to use string
        //this.PlayerName.ToString();
    }

    #endregion
    
    #region Unity Callbacks

    private void Update()
    {
        if(!HasInputAuthority) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var randColor = Random.ColorHSV();
            RPC_SetPlayerColor(randColor);
        }
    }
    
    #endregion
    
}
