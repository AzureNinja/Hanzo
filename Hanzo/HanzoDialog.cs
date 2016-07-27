using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;
using Microsoft.Bot.Connector;
using System.Web.Http;
using HanzoProxyClient;

namespace Hanzo
{
    [LuisModel("200bc5a7-1d1b-4ab3-90bc-b6ad514c1b2e", "67fbb8113f4743aca18c19565baa5544")]
    [Serializable]
    public class HanzoDialog : LuisDialog<object>
    {
        private const string SERVICES_ENTITY = "Services";
        private const string BUILTIN_DATE = "builtin.datetime.date";
        private const string BUILTIN_TIME = "builtin.datetime.time";
        private const string BUILTIN_SET = "builtin.datetime.set";
        private Activity activity;
        private StateClient stateClient = null;
        //private BotData userData = null;
        //private IDialogContext context = null;
        private static string subscriptionId = string.Empty;
        private static string accessToken = string.Empty;
        private static int num = 0;
        private string _apiVersion = ConfigurationManager.AppSettings["apiVersion"] ?? "";
        private string _apiKey = ConfigurationManager.AppSettings["apiKey"] ?? "";
        private string _proxyServerUrl = ConfigurationManager.AppSettings["proxyServerUrl"] ?? "";
        private static Client client = null;

        public HanzoDialog(Activity ac)
        {
            activity = ac;
            if (activity != null) stateClient = activity.GetStateClient();
            //stateClient = sc;
            //userData = ud;
            //userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            // userData.SetProperty<bool>("SentGreeting", false);
            //userData.SetProperty<int>("AuthProcess", 0); // 0: Not authenticated, 1: in the mid of auth process, 2: auth completed
            //stateClient.BotState.SetUserData(activity.ChannelId, activity.From.Id, userData);
        }

        public async Task StartAsync(IDialogContext con)
        {

        }

        private async Task sendMessage(IDialogContext context, string msg)
        {
            await context.PostAsync(msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Start")]
        public async Task Start(IDialogContext context, LuisResult result)
        {
            EntityRecommendation ent;
            string entity = result.TryFindEntity(SERVICES_ENTITY, out ent) ? ent.Entity : string.Empty;
            string date = result.TryFindEntity(BUILTIN_DATE, out ent) ? ent.Resolution["date"] : string.Empty;
            string time = result.TryFindEntity(BUILTIN_TIME, out ent) ? ent.Resolution["time"] : string.Empty;
            string set = result.TryFindEntity(BUILTIN_TIME, out ent) ? ent.Resolution["set"] : string.Empty;

            string msg = "Please try again. #Start";

            if (!string.IsNullOrEmpty(entity))
            {
                string date_time = "now";
                if (!string.IsNullOrEmpty(date)) date_time = "on " + date;
                if (!string.IsNullOrEmpty(time)) date_time += " " + time;

                if (date_time.Equals("now"))
                {
                    msg = $"OK. I will start the {entity} {date_time}. It will take a little while. I will let you know when it is done.";
                }
                else
                {
                    msg = $"I will start the {entity} {date_time}. I will let you know when it is done.";
                }
            }

            await context.PostAsync(msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("List")]
        public async Task List(IDialogContext context, LuisResult result)
        {
            string entity = string.Empty;

            EntityRecommendation ent;
            if (result.TryFindEntity(SERVICES_ENTITY, out ent))
            {
                entity = ent.Entity;
            }

            if (!string.IsNullOrEmpty(entity))
            {
                string msg = HanzoCmd.GetVmListCommand(client);
                await context.PostAsync(msg);
            }
            else
            {
                await context.PostAsync("Please try again. List");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("Stop")]
        public async Task Stop(IDialogContext context, LuisResult result)
        {
            EntityRecommendation ent;
            string entity = result.TryFindEntity(SERVICES_ENTITY, out ent) ? ent.Entity : string.Empty;
            string date = result.TryFindEntity(BUILTIN_DATE, out ent) ? ent.Resolution["date"] : string.Empty;
            string time = result.TryFindEntity(BUILTIN_TIME, out ent) ? ent.Resolution["time"] : string.Empty;

            string msg = "Please try again. #Stop";

            if (!string.IsNullOrEmpty(entity))
            {
                string date_time = "now";
                if (!string.IsNullOrEmpty(date)) date_time = "on " + date;
                if (!string.IsNullOrEmpty(time)) date_time += " " + time;

                if (date_time.Equals("now"))
                {
                    msg = $"I will stop the {entity} {date_time}. It will take a little while. I will let you know when it is completed.";
                }
                else
                {
                    msg = $"I successfully set the {entity} to stop {date_time}. I will let you know when it is completed.";
                }
            }

            await context.PostAsync(msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Sorry. I do not understand what you asked me. Please try again.";
            string query = result.Query;

            if(string.IsNullOrEmpty(query))
            {
                await context.PostAsync(message);
                context.Wait(MessageReceived);
                return;
            }

            //BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            //int num = userData.GetProperty<int>("AuthProcess");
            // Check authentication status -> 0: Not authenticated, 1: waiting for subscription id, 2: waiting for access token, 3: auth completed

            switch (num)
            {
                case 0:
                    message = "Welcome to Hanzo! Please enter your subscription id to get started.";
                    break;
                case 1:
                    subscriptionId = query;
                    message = $"Your subscription id is {subscriptionId}. Then, you should enter the associated access token with the subscription id.";
                    break;
                case 2:
                    accessToken = query;
                    message = "Thank you. You are now ready to access to your account.";

                    client = ClientBuilder.GetClientInstance(subscriptionId, accessToken,
                        new Config(
                            _apiKey,
                            _apiVersion,
                            _proxyServerUrl)
                            );

                    break;
                case 3:
                    break;
                default: break;
            }
            num++;

            if(query.Equals("ls"))
            {
                message = subscriptionId + ": " + accessToken;
            }

/*            if(num < 3)
            {
                userData.SetProperty<int>("AuthProcess", num++);
                //stateClient.BotState.SetUserData(activity.ChannelId, activity.From.Id, userData);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
*/

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}
