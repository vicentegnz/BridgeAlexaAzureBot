# Bridge between Alexa (hosted in AWS) and Bot (hosted in Azure)

# 🔍 Introduction

This is a simple restify-based server that acts as a bridge between an Alexa Skill and the Azure Bot Service. Utterances originating at an Alexa endpoint e.g. an Echo device are received by this bridge, translated and forwarded to the Microsoft Bot Framework. Replies coming back from the Bot are then returned to the originating endpoint. Alexa will read out the text in the Text field of the Bot reply.
