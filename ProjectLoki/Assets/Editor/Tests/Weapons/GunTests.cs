using NUnit.Framework;
using ProjectLoki.Weapons;

namespace ProjectLoki.Tests.Weapons
{
    [TestFixture]
    [Category("Weapon Tests")]
    internal class GunTests
    {
        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasAmmoAndIsIdle_ReturnsTrue()
        {
            Gun testGun = new Gun();
            Assert.IsTrue(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasNoAmmoAndIsIdle_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 0;
            Assert.IsFalse(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasAmmoAndIsReloading_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.State = WeaponState.Reloading;
            Assert.IsFalse(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasNoAmmoAndIsReloading_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 0;
            testGun.State = WeaponState.Reloading;
            Assert.IsFalse(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasAmmoAndIsFiring_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.State = WeaponState.Firing;
            Assert.IsFalse(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanFire_HasNoAmmoAndIsFiring_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 0;
            testGun.State = WeaponState.Firing;
            Assert.IsFalse(testGun.CanFire);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoLessThanMaxAndIdle_ReturnsTrue()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 1;
            Assert.IsTrue(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoEqualToMaxAndIdle_ReturnsFalse()
        {
            Gun testGun = new Gun();
            Assert.IsFalse(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoLessThanMaxAndFiring_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 9;
            testGun.State = WeaponState.Firing;
            Assert.IsFalse(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoEqualToMaxAndFiring_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.State = WeaponState.Firing;
            Assert.IsFalse(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoLessThanMaxAndReloading_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 9;
            testGun.State = WeaponState.Reloading;
            Assert.IsFalse(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void CanReload_AmmoEqualToMaxAndReloading_ReturnsFalse()
        {
            Gun testGun = new Gun();
            testGun.State = WeaponState.Reloading;
            Assert.IsFalse(testGun.CanReload);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void Reload_CanReload_SetAmmoToMax()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 0;

            testGun.Reload();

            Assert.AreEqual(testGun.ClipSize, testGun.CurrentAmmo);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void Reload_CannotReload_NoChange()
        {
            Gun testGun = new Gun();
            testGun.CurrentAmmo = 4;
            testGun.State = WeaponState.Firing;

            testGun.Reload();

            Assert.AreEqual(4, testGun.CurrentAmmo);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void ReduceCooldowns_DeltaLessThanCurrentCooldown_ReduceCooldown()
        {
            Gun testGun = new Gun();
            testGun.CurrentCooldown = 1.0f;

            testGun.ReduceCooldowns(0.4f);

            Assert.AreEqual(0.6f, testGun.CurrentCooldown);
        }

        [Test]
        [Category("Base Gun Tests")]
        public void ReduceCooldowns_DeltaMoreThanCurrentCooldown_CooldownTo0()
        {
            Gun testGun = new Gun();
            testGun.CurrentCooldown = 0.2f;

            testGun.ReduceCooldowns(0.4f);

            Assert.AreEqual(0.0f, testGun.CurrentCooldown);
        }
    }
}
