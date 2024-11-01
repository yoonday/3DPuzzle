using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superliminal : MonoBehaviour
{
    [Header("Components")]
    public Transform target;            // Ÿ�� ������Ʈ

    [Header("Parameters")]
    public LayerMask targetMask;        // ����ĳ��Ʈ ��� ���̾�
    public LayerMask ignoreTargetMask;  // �����ɽ�Ʈ �� ������ ��� ���̾� (�÷��̾�, Ÿ�� ������Ʈ ����)
    public LayerMask groundMask;        // �ٴ� Ȯ�ο� ���̾�
    public float offsetFactor;          // ������Ʈ�� ���� ��ġ�� �ʵ��� ��ġ ����

    float originalDistance;             // ī�޶�� ��� ���� ���� �Ÿ�
    float originalScale;                // ũ�� ���� �� Ÿ�� ������Ʈ�� ���� ũ��
    Vector3 targetScale;                // �� �����ӿ��� ������ ������Ʈ ��ǥ ũ��

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ��� ���·� ����
    }

    void Update()
    {
        HandleInput();
        ResizeTarget();
    }

    void HandleInput()
    {
        // ���콺 �� Ŭ�� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            // ���� Ÿ���� �������� ���� ���
            if (target == null)
            {
                // ���̾� ����ũ�� ������ ��󿡸� �´� ���� ĳ��Ʈ �߻�
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    // ����ĳ��Ʈ�� ���� Transform ������Ʈ�� Ÿ������ ����
                    target = hit.transform;

                    // Ÿ���� ���� ȿ�� ��Ȱ��ȭ
                    target.GetComponent<Rigidbody>().isKinematic = true;

                    // ī�޶�� Ÿ�� ������Ʈ�� �Ÿ� ���
                    originalDistance = Vector3.Distance(transform.position, target.position);

                    // Ÿ���� ���� ������ ���� ����
                    originalScale = target.localScale.x;

                    // Ÿ���� ũ�Ⱑ ���� ũ��� ������
                    targetScale = target.localScale;
                }
                Debug.Log("���õ�");
            }
            // Ÿ���� �����Ǿ� �ִٸ� (���̻� ������ ���� X)
            else
            {
                // Ÿ�� ������Ʈ ���� ȿ�� ��Ȱ��ȭ
                target.GetComponent<Rigidbody>().isKinematic = false;

                // Ÿ�� ���� �ʱ�ȭ
                target = null;
                Debug.Log("������ ���� �Ϸ�");
            }
        }
    }

    void ResizeTarget()
    {
        // Ÿ���� ���� ��� ����
        if (target == null)
        {
            return;
        }

        // ī�޶� ��ġ���� ����ĳ��Ʈ �߻�, ������ ������Ʈ �����ϰ� �浹 ����
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            // �浹 ���� �������� ������ �����Ͽ� Ÿ�� ��ġ ����
            target.position = hit.point - transform.forward * offsetFactor * targetScale.x;

            // ī�޶�� Ÿ�� ���� ���� �Ÿ� ���
            float currentDistance = Vector3.Distance(transform.position, target.position);

            // ���� �Ÿ��� ���� �Ÿ� ���� ���� ���
            float s = currentDistance / originalDistance;

            // �Ÿ� ������ ���� ũ�� ���� ����
            targetScale.x = targetScale.y = targetScale.z = s;

            // ���� ũ��� ���� ���ؼ� Ÿ�� ũ�� ����(��ȭ)
            target.localScale = targetScale * originalScale;

            // �߾��� �������� Ŀ���Ƿ�, ũ�� ��ȭ�� ���� ���̸� ����
            float heightAdjustment = (target.localScale.y * originalScale - originalScale) / 2;

            RaycastHit floorHit;
            if (Physics.Raycast(target.position + Vector3.up * 0.1f, Vector3.down, out floorHit, Mathf.Infinity, groundMask))
            {
                target.position = new Vector3(target.position.x, floorHit.point.y + heightAdjustment + offsetFactor, target.position.z);
            }
        }
    }
}
