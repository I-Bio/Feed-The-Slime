using Players;

namespace Abilities
{
    public class AbilityPresenter : IPresenter
    {
        private readonly AbilityCaster _caster;
        private readonly PlayerAnimation _animation;
        private readonly IMover _mover;

        public AbilityPresenter(AbilityCaster abilityCaster, PlayerAnimation animation, IMover mover)
        {
            _caster = abilityCaster;
            _animation = animation;
            _mover = mover;
        }

        public void Enable()
        {
            _caster.Hid += OnHid;
            _caster.Showed += OnShowed;
            _caster.SpitCasted += OnSpitCasted;
        }

        public void Disable()
        {
            _caster.Hid -= OnHid;
            _caster.Showed -= OnShowed;
            _caster.SpitCasted -= OnSpitCasted;
        }

        private void OnHid()
        {
            _mover.ProhibitMove();
            _animation.PlayHide();
        }

        private void OnShowed()
        {
            _mover.AllowMove();
            _animation.PlayIdle();
        }
        
        private void OnSpitCasted()
        {
            _animation.PlayAttack();
        }
    }
}