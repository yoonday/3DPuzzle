using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superliminal : MonoBehaviour
{
    [Header("Components")]
    public Transform target;            // 타겟 오브젝트

    [Header("Parameters")]
    public LayerMask targetMask;        // 레이캐스트 대상 레이어
    public LayerMask ignoreTargetMask;  // 레이케스트 시 무시할 대상 레이어 (플레이어, 타겟 오브젝트 제외)
    public LayerMask groundMask;        // 바닥 확인용 레이어
    public float offsetFactor;          // 오브젝트가 벽에 겹치지 않도록 위치 조정

    float originalDistance;             // 카메라와 대상 간의 원래 거리
    float originalScale;                // 크기 조정 전 타겟 오브젝트의 원래 크기
    Vector3 targetScale;                // 각 프레임에서 설정할 오브젝트 목표 크기

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금 상태로 설정
    }

    void Update()
    {
        HandleInput();
        ResizeTarget();
    }

    void HandleInput()
    {
        // 마우스 좌 클릭 확인
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 타겟이 설정되지 않은 경우
            if (target == null)
            {
                // 레이어 마스크로 잠재적 대상에만 맞는 레이 캐스트 발사
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    // 레이캐스트에 맞은 Transform 오브젝트를 타겟으로 설정
                    target = hit.transform;

                    // 타겟의 물리 효과 비활성화
                    target.GetComponent<Rigidbody>().isKinematic = true;

                    // 카메라와 타겟 오브젝트의 거리 계산
                    originalDistance = Vector3.Distance(transform.position, target.position);

                    // 타겟의 원래 스케일 값을 저장
                    originalScale = target.localScale.x;

                    // 타겟의 크기가 원래 크기와 동일함
                    targetScale = target.localScale;
                }
                Debug.Log("선택됨");
            }
            // 타겟이 설정되어 있다면 (더이상 사이즈 조정 X)
            else
            {
                // 타겟 오브젝트 물리 효과 재활성화
                target.GetComponent<Rigidbody>().isKinematic = false;

                // 타겟 변수 초기화
                target = null;
                Debug.Log("사이즈 변경 완료");
            }
        }
    }

    void ResizeTarget()
    {
        // 타겟이 없을 경우 리턴
        if (target == null)
        {
            return;
        }

        // 카메라 위치에서 레이캐스트 발사, 무시할 오브젝트 제외하고 충돌 검출
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            // 충돌 지점 기준으로 오프셋 적용하여 타겟 위치 설정
            target.position = hit.point - transform.forward * offsetFactor * targetScale.x;

            // 카메라와 타겟 간의 현재 거리 계산
            float currentDistance = Vector3.Distance(transform.position, target.position);

            // 현재 거리와 원래 거리 간의 비율 계산
            float s = currentDistance / originalDistance;

            // 거리 비율에 따라 크기 벡터 설정
            targetScale.x = targetScale.y = targetScale.z = s;

            // 원래 크기와 비율 곱해서 타겟 크기 설정(변화)
            target.localScale = targetScale * originalScale;

            // 중앙을 기준으로 커지므로, 크기 변화에 따라 높이를 보정
            float heightAdjustment = (target.localScale.y * originalScale - originalScale) / 2;

            RaycastHit floorHit;
            if (Physics.Raycast(target.position + Vector3.up * 0.1f, Vector3.down, out floorHit, Mathf.Infinity, groundMask))
            {
                target.position = new Vector3(target.position.x, floorHit.point.y + heightAdjustment + offsetFactor, target.position.z);
            }
        }
    }
}
