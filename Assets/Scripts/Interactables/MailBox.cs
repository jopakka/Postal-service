namespace Interactables
{
    public class MailBox : Interactable
    {
        private void Start()
        {
            OnInteractCallbacks += MailBoxInteract;
        }

        private void MailBoxInteract()
        {
            gameObject.SetActive(false);
        }
    }
}
