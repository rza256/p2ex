# p2ex
A CLI that converts P2 Panasonic .MXF files to H265 / H264 .MP4

## Purpose
MXF straight off a P2 card is large and ships without muxed audio. This fixes the problem by muxing in the L & R channels provided by the `AUDIO` folder.

## Prerequisites
A recent version of `dotnet`.

## Usage
Compile it yourself using `dotnet publish -r win-x64` while being in the project directory.

Place the build into your P2 folder with `LASTCLIP.TXT`. You are able to choose between H264 or H265 in the terminal, and the quality preset of the video. ffmpeg is automatically downloaded for you.

The transcoded videos should be in the same folder as where the binary is placed (usually with `LASTCLIP.txt`). 

## Tested cameras
- HVX200P
- HVX200
- HVX200A

## Quirks of the P2 format
```
- CONTENTS
  - AUDIO (Audio MXF)
  - CLIP (XML, Metadata)
  - ICON (Thumbnails for in-camera clip viewer)
  - PROXY
  - VOICE
  - VIDEO (Video MXF)
```

Panasonic stores MXFs in a weird way. 

They do not combine the audio and video tracks together, and instead leaves the 4 corresponding `.MXF` files in the `AUDIO` folder.
For example, let's take a file from `VIDEO`. `"00004NH.MXF"`: there should be a `00004NH(00)`.MXF (L) file, and a `00004NH(01).MXF` (R) file. The two trailing digits in the filename correspond to the channel.

Panasonic also decided to store a third and fourth channel and made it silence. I don't know why they did this, as the audio format they used was `PCMle16`. This **balloons** the file usage on the P2 card. Even if an audio file is silent, it will still take up as much storage as a file that isn't silent.

The `CLIP` directory is not of much importance for our project, but it is useful for discerning the model of the camera.
