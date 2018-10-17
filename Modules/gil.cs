using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    public class gil : ModuleBase<SocketCommandContext>
    {
        
        [Command("join")]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();

            await SendAsync(audioClient, "audio/hello.mp3");
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }
        }
    }

