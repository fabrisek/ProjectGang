using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class GroupePlateformeManager : MonoBehaviour
    {
        [SerializeField] List<GroupesPlateforme> groupePlateforms;

        List<GroupesPlateforme> groupesPlateformeGo;

        [SerializeField] float distToGo;

        [SerializeField] Transform playerpPos;

        public Transform Player
        {
            set
            {
                playerpPos = value;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            groupesPlateformeGo = new List<GroupesPlateforme>();
            for (int i = 0; i < groupePlateforms.Count; i++)
            {
                groupePlateforms[i].InitGroupesBugs();
            }
            for (int i = 0; i < groupePlateforms.Count; i++)
            {
                groupePlateforms[i].ResetAllTargetSet();
            }


        }

        // Update is called once per frame
        void Update()
        {
           
            if (playerpPos != null)
            {
                if (groupePlateforms.Count != 0)
                {
                    SetTargetToGroupeBug();
                }
                if (groupesPlateformeGo.Count != 0)
                {
                    DestroyToGroupeBug();
                }
            }
        }

        void ChangeToOtherList(int index)
        {
            groupesPlateformeGo.Add(groupePlateforms[index]);
            List<GroupesPlateforme> groupesBugsTemps = groupePlateforms;
            groupePlateforms = new List<GroupesPlateforme>();
            for (int i = 0; i < groupesBugsTemps.Count; i++)
            {

                if (i != index)
                {
                    groupePlateforms.Add(groupesBugsTemps[i]);
                }
            }
        }

        void SetTargetToGroupeBug()
        {
            if (playerpPos != null && Timer.Instance.GetTimer() > 0)
            {
                int index = checkTheDistancePlayerRef();
                if(Timer.Instance != null)
                {
                    if (Timer.Instance.TimerIsLaunch())
                    {
                        if (index != -1)
                        {
                            groupePlateforms[index].SetTarget(0);
                            ChangeToOtherList(index);
                        }
                    }
                }
                else
                {
                    if (index != -1)
                    {
                        groupePlateforms[index].SetTarget(0);
                        ChangeToOtherList(index);
                    }
                }
                
            }
        }

        int checkTheDistancePlayerRef()
        {
            for (int i = 0; i < groupePlateforms.Count; i++)
            {
                if (distToGo >= Vector3.Distance(new Vector3(groupePlateforms[i].RefPositionDistPlayer.position.x, 0, groupePlateforms[i].RefPositionDistPlayer.position.z), new Vector3(playerpPos.position.x, 0, playerpPos.position.z)))
                {
                    return i;
                }

            }

            return -1;
        }

        void DestroyToGroupeBug()
        {
            if (playerpPos != null)
            {
                int index = checkTheDistancePlayerRefDisable();
                if (index != -1)
                {
                    groupesPlateformeGo[index].DestroyAllPlateforme();
                }
            }
        }

        int checkTheDistancePlayerRefDisable()
        {
            for (int i = 0; i < groupesPlateformeGo.Count; i++)
            {
                if (distToGo * 2 <= Vector3.Distance(new Vector3(groupesPlateformeGo[i].RefPositionDistPlayer.position.x, 0, groupesPlateformeGo[i].RefPositionDistPlayer.position.z), new Vector3(playerpPos.position.x, 0, playerpPos.position.z)))
                {
                    return i;
                }

            }

            return -1;
        }
    }
}
