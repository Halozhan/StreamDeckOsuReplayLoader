using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace streamdeckosureplayloader
{
    [PluginActionId("streamdeckosureplayloader.pluginaction")]
    public class PluginAction : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.OutputFileName = String.Empty;
                instance.InputString = String.Empty;
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "outputFileName")]
            public string OutputFileName { get; set; }

            [JsonProperty(PropertyName = "inputString")]
            public string InputString { get; set; }
        }

        #region Private Members

        private PluginSettings settings;

        #endregion
        public PluginAction(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = PluginSettings.CreateDefaultSettings();
                SaveSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<PluginSettings>();
            }
        }

        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override void KeyPressed(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");

            PressEvent();
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion

        private async void PressEvent()
        {
            await Task.Run(() =>
            {
                // ref: http://www.gisdeveloper.co.kr/?p=2044

                string replaypath = "F:\\osu!\\Replays\\";
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(replaypath);
                string LatestReplay = string.Empty;
                DateTime LatestReplayTime = new DateTime();

                foreach (System.IO.FileInfo file in di.GetFiles())
                {
                    if (file.Extension.ToLower().CompareTo(".osr") == 0)
                    {
                        if (LatestReplayTime <= file.LastWriteTimeUtc)
                        {
                            LatestReplay = file.FullName;
                            LatestReplayTime = file.LastWriteTimeUtc;
                        }
                    }
                }
                Process LatestOsr = new Process();
                LatestOsr.StartInfo.FileName = LatestReplay;
                LatestOsr.Start();
            });
        }
    }
}