using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectLoki.Weapons
{
    public class AbilitiesController : BaseClass
    {
        private PhotonView m_PhotonView;

        void Awake()
        {
            m_PhotonView = GetComponent<PhotonView>();
            this.PrimaryWeapon = new Gun();
            this.SecondaryWeapon = new Gun();
            List<IAbility> abList = new List<IAbility>();
            abList.Add(new BaseAbility("Ability 1"));
            abList.Add(new BaseAbility("Ability 2"));
            abList.Add(new BaseAbility("Ability 3"));
            Abilities = abList;
        }

        public WeaponSlot SelectedWeapon;

        public IWeapon PrimaryWeapon;
        public IWeapon SecondaryWeapon;

        public IEnumerable<IAbility> Abilities;

        public IWeapon CurrentWeapon
        {
            get
            {
                IWeapon result = null;
                if (this.SelectedWeapon == WeaponSlot.Primary)
                {
                    result = this.PrimaryWeapon;
                }
                else if (this.SelectedWeapon == WeaponSlot.Secondary)
                {
                    result = this.SecondaryWeapon;
                }

                return result;
            }
        }

        public void Update()
        {
            if(m_PhotonView.isMine)
            {
                PrimaryWeapon.ReduceCooldowns(Time.deltaTime);
                SecondaryWeapon.ReduceCooldowns(Time.deltaTime);
                foreach(IAbility ability in Abilities)
                {
                    ability.Tick(Time.deltaTime);
                }
            }
        }


        public void FireWeapon(Vector3 position, Vector3 rotation)
        {
            this.m_PhotonView.RPC("FireWeaponOnServer", PhotonTargets.MasterClient, position, rotation, PhotonNetwork.time);
            this.CurrentWeapon.DisplayAnimation(position, rotation);
        }

        [PunRPC]
        public void FireWeaponOnServer(Vector3 position, Vector3 rotation, double timeTriggered)
        {
            if (this.CurrentWeapon.Activate(position, rotation, timeTriggered))
            {
                this.m_PhotonView.RPC("DisplayAnimationOthers", PhotonTargets.All, position, rotation);
            }
        }

        [PunRPC]
        public void DisplayAnimationOthers(Vector3 position, Vector3 rotation)
        {
            if (!m_PhotonView.isMine)
            {
                this.CurrentWeapon.DisplayAnimation(position, rotation);
            }
        }

        public void SecondaryAction()
        {
            this.m_PhotonView.RPC("SecondaryActionOnClients", PhotonTargets.All);
        }

        [PunRPC]
        public void SecondaryActionOnClients()
        {

            IReloadable reloadingWeapon = this.CurrentWeapon as IReloadable;
            if (reloadingWeapon != null)
            {
                reloadingWeapon.Reload();
            }
        }

        public void SwitchWeapon()
        {
            if (this.SelectedWeapon == WeaponSlot.Primary)
            {
                this.SelectedWeapon = WeaponSlot.Secondary;
            }
            else
            {
                this.SelectedWeapon = WeaponSlot.Primary;
            }
        }

    }

    public enum WeaponSlot
    {
        None = 0,
        Primary = 1,
        Secondary = 2
    }
}
