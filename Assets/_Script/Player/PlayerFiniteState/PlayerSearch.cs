using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSearch : NetworkBehaviour
{
    [SerializeField]
    private float angle = 45.0f;

    private PlayerController player;
    private string blindLayer;
    private string showLayer;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
        blindLayer = player.GetPlayerBlindLayerName();
        showLayer = player.GetPlayerShowLayerName();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsOwner)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer(blindLayer) || other.gameObject.layer == LayerMask.NameToLayer(showLayer))
        {
            Vector3 posDelta = other.transform.position - this.transform.position;
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);
            if (target_angle < angle)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Debug.DrawRay(pos, posDelta, Color.red, 0.5f);
                //Ray���g�p����target�ɓ������Ă��邩����
                if (Physics.Raycast(pos, posDelta, out RaycastHit hit))
                {
                    if (hit.collider == other)
                    {
                        //���E���Ɏ��܂��Ă��鏈��
                        other.gameObject.layer = LayerMask.NameToLayer(showLayer);
                    }
                    else
                    {
                        //�^�[�Q�b�g�ƃv���C���[�̊Ԃɕʂ̃I�u�W�F�N�g���������ꍇ
                        other.gameObject.layer = LayerMask.NameToLayer(blindLayer);
                    }
                }
            }
            else
            {
                //�p�x���Ɏ��܂��Ă��Ȃ��ꍇ
                other.gameObject.layer = LayerMask.NameToLayer(blindLayer);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(showLayer))
        {
            other.gameObject.layer = LayerMask.NameToLayer(blindLayer);
        }
    }
}
