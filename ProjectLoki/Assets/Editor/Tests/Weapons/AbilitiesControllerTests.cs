using System;
using NUnit.Framework;
using ProjectLoki.Weapons;
using UnityEngine;

namespace ProjectLoki.Tests.Weapons
{
    [TestFixture]
    [Category("Weapon Tests")]
    internal class AbilitiesControllerTests
    {
        [Test]
        [Category("Base AbilitiesController Tests")]
        public void CurrentWeapon_SelectedWeaponIsPrimary_ReturnPrimaryWeapon()
        {
            AbilitiesController controller = new AbilitiesController();
            TestWeapon weapon1 = new TestWeapon() { Name = "Weapon1" };
            TestWeapon weapon2 = new TestWeapon() { Name = "Weapon2" };
            controller.PrimaryWeapon = weapon1;
            controller.SecondaryWeapon = weapon2;
            controller.SelectedWeapon = WeaponSlot.Primary;

            TestWeapon result = controller.CurrentWeapon as TestWeapon;
            Assert.AreEqual("Weapon1", result.Name);
        }

        internal class TestWeapon : IWeapon
        {
            public string Name { get; set; }

            public void Activate(Vector3 position, Vector3 rotation)
            {
                throw new NotImplementedException();
            }

            public void ReduceCooldowns(float delta)
            {
                throw new NotImplementedException();
            }
        }
    }
}
