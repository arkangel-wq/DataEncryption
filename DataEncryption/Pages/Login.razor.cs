using DataEncryption.Core.ViewModels;

namespace DataEncryption.Pages
{
    public partial class Login
    {
        DataVM VM = new DataVM();

        protected override void OnInitialized()
        {
            VM.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case "MostrarEncryptacion":
                        StateHasChanged();
                        break;
                    case "MostrarDesencryptacion":
                        StateHasChanged();
                        break;
                    default:
                        break;
                }
            };
        }


    }

}

