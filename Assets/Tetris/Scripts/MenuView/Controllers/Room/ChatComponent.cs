//using ChatApp.Shared.Hubs;
//using ChatApp.Shared.MessagePackObjects;
//using ChatApp.Shared.Services;
//using Grpc.Core;
//using MagicOnion.Client;
//using MainScene.Grpc.Filters;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace MainScene.ChatScene.RoomPage
//{
//    public class ChatComponent : MonoBehaviour, IChatHubReceiver
//    {
//        CancellationTokenSource shutdownCancellation = new CancellationTokenSource();
//        Channel channel;
//        IChatHub streamingClient;
//        IChatService client;
//        bool isJoin;
//        bool isSelfDisConnected;
//        public Text ChatText;
//        public Button JoinOrLeaveButton;
//        public Text JoinOrLeaveButtonText;
//        public Button SendMessageButton;
//        public InputField Input;
//        public InputField ReportInput;
//        public Button SendReportButton;
//        public Button DisconnectButon;
//        public Button ExceptionButton;
//        public Button UnaryExceptionButton;
//
//        async void Start()
//        {
//            await InitializeClientAsync();
//            InitializeUi();
//        }
//
//        async void OnDestroy()
//        {
//            // Clean up Hub and channel
//            shutdownCancellation.Cancel();
//            if (streamingClient != null) await streamingClient.DisposeAsync();
//            if (channel != null) await channel.ShutdownAsync();
//        }
//
//        async Task InitializeClientAsync()
//        {
//            // Initialize the Hub
//            channel = new Channel("api.joycraft.mobi", 5000, ChannelCredentials.Insecure);
//
//            // for SSL/TLS connection
//            //var cred = new SslCredentials(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "server.crt")));
//            //this.channel = new Channel("dummy.example.com", 5000, cred); // local tls
//            //this.channel = new Channel("your-nlb-domain.com", 5000, new SslCredentials()); // aws nlb tls
//            while (!shutdownCancellation.IsCancellationRequested) {
//                try {
//                    Debug.Log($"Connecting to the server...");
//                    streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(channel, this,
//                        cancellationToken: shutdownCancellation.Token);
//                    RegisterDisconnectEvent(streamingClient);
//                    Debug.Log($"Connection is established.");
//                    break;
//                }
//                catch (Exception e) {
//                    Debug.LogError(e);
//                }
//
//                Debug.Log($"Failed to connect to the server. Retry after 5 seconds...");
//                await Task.Delay(5 * 1000);
//            }
//
//            // todo: 添加过滤器
//            client = MagicOnionClient.Create<IChatService>(channel, new IClientFilter[] {
//                //new LoggingFilter(),
//                //new AppendHeaderFilter(),
//                //new RetryFilter()
//                new EncryptFilter()
//            });
//        }
//
//        void InitializeUi()
//        {
//            isJoin = false;
//            SendMessageButton.interactable = false;
//            ChatText.text = string.Empty;
//            Input.text = string.Empty;
//            Input.placeholder.GetComponent<Text>().text = "Please enter your name.";
//            JoinOrLeaveButtonText.text = "Enter the room";
//            ExceptionButton.interactable = false;
//        }
//
//        async void RegisterDisconnectEvent(IChatHub streamingClient)
//        {
//            try {
//                // you can wait disconnected event
//                await streamingClient.WaitForDisconnect();
//            }
//            catch (Exception e) {
//                Debug.LogError(e);
//            }
//            finally {
//                // try-to-reconnect? logging event? close? etc...
//                Debug.Log($"disconnected from the server.: {channel.State}");
//                if (isSelfDisConnected) {
//                    // there is no particular meaning
//                    await Task.Delay(2000);
//
//                    // reconnect
//                    await ReconnectServerAsync();
//                }
//            }
//        }
//
//        public async void DisconnectServer()
//        {
//            isSelfDisConnected = true;
//            JoinOrLeaveButton.interactable = false;
//            SendMessageButton.interactable = false;
//            SendReportButton.interactable = false;
//            DisconnectButon.interactable = false;
//            ExceptionButton.interactable = false;
//            UnaryExceptionButton.interactable = false;
//            if (isJoin) JoinOrLeave();
//            await streamingClient.DisposeAsync();
//        }
//
//        public async void ReconnectInitializedServer()
//        {
//            if (channel != null) {
//                var chan = channel;
//                if (chan == Interlocked.CompareExchange(ref channel, null, chan)) {
//                    await chan.ShutdownAsync();
//                    channel = null;
//                }
//            }
//
//            if (streamingClient != null) {
//                var streamClient = streamingClient;
//                if (streamClient == Interlocked.CompareExchange(ref streamingClient, null, streamClient)) {
//                    await streamClient.DisposeAsync();
//                    streamingClient = null;
//                }
//            }
//
//            if (channel == null && streamingClient == null) {
//                await InitializeClientAsync();
//                InitializeUi();
//            }
//        }
//
//        async Task ReconnectServerAsync()
//        {
//            Debug.Log($"Reconnecting to the server...");
//            streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(channel, this);
//            RegisterDisconnectEvent(streamingClient);
//            Debug.Log("Reconnected.");
//            JoinOrLeaveButton.interactable = true;
//            SendMessageButton.interactable = false;
//            SendReportButton.interactable = true;
//            DisconnectButon.interactable = true;
//            ExceptionButton.interactable = true;
//            UnaryExceptionButton.interactable = true;
//            isSelfDisConnected = false;
//        }
//
//        #region Client -> Server (Streaming)
//
//        public async void JoinOrLeave()
//        {
//            Debug.Log("JoinOrLevel");
//            if (isJoin) {
//                await streamingClient.LeaveAsync();
//                InitializeUi();
//            }
//            else {
//                var request = new JoinRequest {RoomName = "SampleRoom", UserName = Input.text};
//                await streamingClient.JoinAsync(request);
//                isJoin = true;
//                SendMessageButton.interactable = true;
//                JoinOrLeaveButtonText.text = "Leave the room";
//                Input.text = string.Empty;
//                Input.placeholder.GetComponent<Text>().text = "Please enter a comment.";
//                ExceptionButton.interactable = true;
//            }
//        }
//
//        public async void SendMessage()
//        {
//            if (!isJoin) return;
//            await streamingClient.SendMessageAsync(Input.text);
//            Input.text = string.Empty;
//        }
//
//        public async void GenerateException()
//        {
//            // hub
//            if (!isJoin) return;
//            await streamingClient.GenerateException("client exception(streaminghub)!");
//        }
//
//        public void SampleMethod()
//        {
//            throw new NotImplementedException();
//        }
//
//        #endregion
//
//        #region Server -> Client (Streaming)
//
//        public void OnJoin(string name)
//        {
//            ChatText.text += $"\n<color=grey>{name} entered the room.</color>";
//        }
//
//        public void OnLeave(string name)
//        {
//            ChatText.text += $"\n<color=grey>{name} left the room.</color>";
//        }
//
//        public void OnSendMessage(MessageResponse message)
//        {
//            ChatText.text += $"\n{message.UserName}：{message.Message}";
//        }
//
//        public void OnLogin(ReceiveLogin msg) { }
//        public void OnRegister(ReceiveRegister msg) { }
//        public void OnLostPassword(ReceiveLostPwd msg) { }
//
//        #endregion
//
//        #region Client -> Server (Unary)
//
//        public async void SendReport()
//        {
//            await client.SendReportAsync(ReportInput.text);
//            ReportInput.text = string.Empty;
//        }
//
//        public async void UnaryGenerateException()
//        {
//            // unary
//            await client.GenerateException("client exception(unary)！");
//        }
//
//        #endregion
//    }
//}