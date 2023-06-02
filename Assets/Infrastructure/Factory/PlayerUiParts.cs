using ChobiAssets.PTM;

namespace Infrastructure.Factory
{
    public class PlayerUiParts
    {
        public Aiming_Control_CS Aiming { get; set; }
        public Bullet_Generator_CS BulletGenerator { get; set; }
        public Cannon_Fire_CS CannonFire { get; set; }
        public Gun_Camera_CS GunCamera { get; set; }
        public DamageReciviersManager DamageReceiver { get; set; }
        public Drive_Control_CS DriveControl { get; set; }
        public CameraViewSetup CameraView { get; set; }
        public ID_Settings_CS IdSettings{ get; set; }
    }
}