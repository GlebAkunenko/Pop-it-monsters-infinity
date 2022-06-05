public abstract class OldWindow : Window
{
    public override void Open()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetBool("opened", true);
    }

    public override void Close()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetBool("opened", false);
    }
}
