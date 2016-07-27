using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;

namespace Hanzo
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
       // static BotData userData;
       // static StateClient stateClient;

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            /*
            if(userData == null)
            {
                userData = await activity.GetStateClient().BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                // Initialization
                stateClient = activity.GetStateClient();
                userData.SetProperty<int>("AuthProcess", 0);
                stateClient.BotState.SetUserData(activity.ChannelId, activity.From.Id, userData);
            }*/
//            StateClient stateClient = activity.GetStateClient();
//            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);


            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new HanzoDialog(activity));
                // TODO: This app use HanzoAuthDialog before setup Subscriptionid and AccessToken. Just after setup them, bot enumlator sometimes mistake here "if" process.
/*                if (string.IsNullOrEmpty(subscriptionId) == false && string.IsNullOrEmpty(accessToken) == false)
                {
                    flg = true;
                    await Conversation.SendAsync(activity, () => new HanzoDialog(activity));
                } else
                {
                    if(flg == false) await Conversation.SendAsync(activity, HanzoAuthDialog.MakeDialog);
                    else await Conversation.SendAsync(activity, () => new HanzoDialog(activity));
                }
*/
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}