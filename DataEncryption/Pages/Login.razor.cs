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
        bool ShowInfoUsuario = false;
        string Mensaje = "";
        string Imagen = "";
        
       private void OpenCloseInfoUsuario()
        {
            Mensaje = "Se establecio correctamente la url de las Apis publicas ❤️";
            Imagen = "/img/computer.svg";
            if (ShowInfoUsuario) ShowInfoUsuario = false;
            else ShowInfoUsuario = true;

        }

        private async void login()
        {
            await VM.ReadUrlCommand.ExecuteAsync();
            OpenCloseInfoUsuario();
            await Task.Delay(2000);
            
        }

    }


}

