using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DncyTemplate.BlazorServer.Components.Pages.Account
{
    public partial class Login
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string ReturnUrl { get; set; }


        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }


        [Inject]
        public required NavigationManager NavigationManager { get; init; }
        [Inject]
        public required IJSRuntime JS { get; set; }



        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationState is { } authStateTask)
            {
                var state = await authStateTask;
                if (state.User.Identity?.IsAuthenticated ?? false)
                {
                    NavigationManager.NavigateTo(GetRedirectUrl(), forceLoad: true);
                    return;
                }
            }
        }




        private string GetRedirectUrl()
        {
            return ReturnUrl ?? "/";
        }
    }
}
