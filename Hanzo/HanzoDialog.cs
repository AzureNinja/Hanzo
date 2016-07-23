using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;
using Microsoft.Bot.Connector;

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

        public async Task StartAsync(IDialogContext context)
        {
            // context.Wait(MessageReceivedAsync);// See the AuthBot sample on GitHub
        }

        //private void 

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
                await context.PostAsync($"Here you go. See the list below:");
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
            string message = string.Empty;
            string query = result.Query;

            if (!string.IsNullOrEmpty(query))
            {
                // login process are written in 
                //if (query.Equals("login"))
                //{
                //    message = $"[Go to login procedure...]";
                //}
                //else
                if (query.Equals("logout"))
                {
                    message = $"You are sucessfully logged out.";
                }
                else
                {
                    message = $"Sorry; I do not get what you want me to do. Try again.";
                }
            }

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}
