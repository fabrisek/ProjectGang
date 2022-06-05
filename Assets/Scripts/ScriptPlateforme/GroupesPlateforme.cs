using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    [System.Serializable]
    public class GroupesPlateforme
    {
        [SerializeField] List<PlateformeControler> groupePlateforme;
        [SerializeField] List<Transform> target;
        [SerializeField] Transform refPositionDistPlayer;

        public Transform RefPositionDistPlayer
        {
            get
            {
                return refPositionDistPlayer;
            }
        }
        public void InitGroupesBugs()
        {
            for (int i = 0; i < groupePlateforme.Count; i++)
            {
                groupePlateforme[i].enabled = true;
                //  Debug.Log(groupeBugs[i].isActiveAndEnabled);
                groupePlateforme[i].IndexGroupesBug = i;
                groupePlateforme[i].GroupePlateformeAssign = this;
               


            }

            SetTarget(0);

        }


        public void SetTarget(int index)
        {
            if (index > -1 && index < target.Count)
            {
                for (int i = 0; i < groupePlateforme.Count; i++)
                {
                    groupePlateforme[i].enabled = true;
                    groupePlateforme[i].Target = target[index];
                    groupePlateforme[i].IndexTarget = index;

                }
            }

        }

        public void ResetAllTargetSet()
        {
            for (int i = 0; i < groupePlateforme.Count; i++)
            {

                groupePlateforme[i].Target = null;
                groupePlateforme[i].IndexTarget = 0;

                groupePlateforme[i].enabled = false;
            }


        }


        public void ChangeTarget(int actualIndex, int indexGroupes)
        {
            if (indexGroupes > -1 && indexGroupes < groupePlateforme.Count)
            {
                if (actualIndex < 0)
                {
                    actualIndex = 0;
                }

                if (actualIndex + 1 < target.Count)
                {
                    groupePlateforme[indexGroupes].IndexTarget = actualIndex + 1;
                    groupePlateforme[indexGroupes].Target = target[actualIndex + 1];
                }
                else
                {
                    groupePlateforme[indexGroupes].Target = null;
                    groupePlateforme[indexGroupes].enabled = false;

                }
            }

        }

        public void DestroyAllPlateforme()
        {
            for (int i = 0; i < groupePlateforme.Count; i++)
            {
                groupePlateforme[i].DestroyMe();
            }
            groupePlateforme = new List<PlateformeControler>();
        }
    }
}
