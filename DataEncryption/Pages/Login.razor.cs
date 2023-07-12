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
                    case "VerMensaje":
                        StateHasChanged();
                        break;
                    case "Mensaje":
                        StateHasChanged();
                        break;
                    default:
                        break;
                }
            };
        }

        public void GetValueInput(string valor) => VM.DataSelected.UrlServidorVM = valor;
        public void GetValueDesenc(string valor) => VM.DataSelected.DesencryptionKey = valor;
    }
}

