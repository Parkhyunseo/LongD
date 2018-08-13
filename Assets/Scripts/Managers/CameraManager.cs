using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CameraManager : Singletons.Singleton<CameraManager>
    {
        /// <summary>
        /// 카메라가 상대 행성쪽으로 조금 이동하며, 상대 행성의 크기가 조금 더 커 보이게 연출
        /// </summary>
        public void MovetToOppoentPlanet()
        {
            GameManager.Instance.Opponent.transform.localScale = Vector3.one*2;
        }

        IEnumerator CCameraMove()
        {
            while(Camera.main.transform.position.y != 20)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GameManager.Instance.Opponent.transform.position, Time.deltaTime * 10);
                yield return 0;
            }

        }

        public void MovetToMyPlanet()
        {

        }


    }

}
