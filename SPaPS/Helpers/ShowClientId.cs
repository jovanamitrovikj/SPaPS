using Microsoft.AspNetCore.Identity;
using SPaPS.Data;

namespace SPaPS.Helpers
{
    public class ShowClientId
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SPaPSContext context;

        public ShowClientId (UserManager<IdentityUser> userManager, SPaPSContext context)
        {
            this.userManager=userManager;
            this.context=context;
        }

        public async Task<long?> ShowCliendIdByEmailAsynce(string email)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user != null)
            {
                var userID = user.Id;
                var client = context.Clients.FirstOrDefault(x=> x.UserId == userID);
                if(client != null)
                {
                    return client.ClientId;
                }

            }
            return null;
        }
    }
}
