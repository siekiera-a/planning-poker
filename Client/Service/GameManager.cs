using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Server.Models.Server;

namespace Client.Service
{
    public interface IGameManager
    {
        Task Submit(double cardValoue);
        Task<JoinResponse> Connect(int meetingId);
        event EventHandler<ClientResponse> TaskChangedEvent;
        event EventHandler<OrganizerResponse> SubmittedEvent;
        event EventHandler CloseEvent;
        HubConnection Connection { get; set; }
        int MeetingId { get; set; }

    }
    public class GameManager : IGameManager
    {
        private readonly ITokenManager _token;
        public HubConnection Connection { get; set; }
        public int MeetingId { get; set; }
        public event EventHandler<ClientResponse> TaskChangedEvent;
        public event EventHandler<OrganizerResponse> SubmittedEvent;
        public event EventHandler CloseEvent;

        public GameManager(ITokenManager token)
        {
            _token = token;
        }

        public async Task Submit(double cardValue)
        {
            await Connection.InvokeAsync("submit", MeetingId,
                new ClientRequest {EstimatedTime = Convert.ToInt16(cardValue)});
        }

        public async Task<JoinResponse> Connect(int meetingId)
        {
            MeetingId = meetingId;
            Connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.73:5000/game",
                    options => { options.Headers.Add("Authorization", $"Bearer {_token.Token}"); })
                .Build();
            await Connection.StartAsync();

            Connection.On<ClientResponse>("TaskChange", OnTaskChanged);
            Connection.On<OrganizerResponse>("Submitted", OnSubmitted);
            Connection.On("CloseWindow", OnCLoseWindow);

            return await Connection.InvokeAsync<JoinResponse>("join", MeetingId);
        }

        protected virtual void OnTaskChanged(ClientResponse e)
        {
            TaskChangedEvent?.Invoke(this, e);
        }

        protected virtual void OnSubmitted(OrganizerResponse e)
        {
            SubmittedEvent?.Invoke(this, e);
        }

        protected virtual void OnCLoseWindow()
        {
            CloseEvent?.Invoke(this, new EventArgs());
        }


    }
}
