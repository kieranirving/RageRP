//Our Javascript Shite
require('watermark.js');
require('customTags.js');// Custom Nametags
require('fingerpoint.js');// Fingerpointing (To be replaced with a custom animation handler)
require('voicechat.js');// VoiceChat
mp.gui.execute("window.location = 'package://chat/index.html'"); //Execute the chat window
require('./chat/chat.js');// Chat

// Debug Stuff
require('fly.js');