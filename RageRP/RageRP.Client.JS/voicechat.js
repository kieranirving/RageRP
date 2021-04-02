const Use3d = true;
const UseAutoVolume = false;

const MaxShoutingRange = 40.0;
const MaxRange = 20.0;
const MaxWhisperingRange = 5.0;


//mp.keys.bind(0x73, true, function() {
//    mp.voiceChat.muted = !mp.voiceChat.muted;
//    //mp.game.graphics.notify("Voice Chat: " + ((!mp.voiceChat.muted) ? "~g~enabled" : "~r~disabled"));
//});

mp.events.add('setIsMuted', (_isChatMuted) => {
    mp.gui.chat.push('isChatMuted =' + _isChatMuted);
    mp.voiceChat.muted = _isChatMuted;
});

let g_voiceMgr =
{
	listeners: [],
	
	add: function(player)
	{
		this.listeners.push(player);
		
		player.isListening = true;		
		mp.events.callRemote("add_voice_listener", player);
		
		if(UseAutoVolume)
		{
			player.voiceAutoVolume = true;
		}
		else
		{
			player.voiceVolume = 1.0;
		}
		
		if(Use3d)
		{
			player.voice3d = true;
		}
	},
	
	remove: function(player, notify)
	{
		let idx = this.listeners.indexOf(player);
			
		if(idx !== -1)
			this.listeners.splice(idx, 1);
			
		player.isListening = false;		
		
		if(notify)
		{
			mp.events.callRemote("remove_voice_listener", player);
		}
	}
};

mp.events.add("playerQuit", (player) =>
{
	if(player.isListening)
	{
		g_voiceMgr.remove(player, false);
	}
});

mp.events.add("mutedPlayer", (player) => {
    if (player.isListening) {
        g_voiceMgr.remove(player, true);
    }
});

setInterval(() =>
{
	let localPlayer = mp.players.local;
	let localPos = localPlayer.position;

	mp.players.forEachInStreamRange(player =>
	{
		if(player !== localPlayer)
        {
            let isMuted = player.getVariable("isMuted");
            let chatDistance = player.getVariable("VoiceDistance");
            mp.gui.chat.push('isMutedstream =' + isMuted);
            if (!player.isListening && isMuted === 'False') {
                const playerPos = player.position;
                let dist = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z);

                let currentRange = 0;
                if (chatDistance === "Talking")
                    currentRange = MaxRange;
                else if (chatDistance === "Shouting")
                    currentRange = MaxShoutingRange;
                else if (chatDistance === "Whisper")
                    currentRange = MaxWhisperingRange;
                if (dist <= currentRange) {
                    g_voiceMgr.add(player);
                }
            }
            else
            {
                if (player.isListening) {
                    g_voiceMgr.remove(player, true);
                }
            }
		}
	});
	
	g_voiceMgr.listeners.forEach((player) =>
    {
        let isMuted = player.getVariable("isMuted");
		if(player.handle !== 0)
        {
            mp.gui.chat.push('isMutedlisten =' + isMuted);
            if (isMuted === 'False') {
                let chatDistance = player.getVariable("VoiceDistance");
                const playerPos = player.position;
                let dist = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z);

                let currentRange = 0;
                if (chatDistance === "Talking")
                    currentRange = MaxRange;
                else if (chatDistance === "Shouting")
                    currentRange = MaxShoutingRange;
                else if (chatDistance === "Whisper")
                    currentRange = MaxWhisperingRange;
                if (dist > currentRange) {
                    g_voiceMgr.remove(player, true);
                }
                else if (!UseAutoVolume) {
                    player.voiceVolume = 1 - (dist / currentRange);
                }
            } else {
                if (player.isListening) {
                    g_voiceMgr.remove(player, true);
                }
            }
		}
	});
}, 500);