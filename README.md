# MDImage2Call

***For MarkDown files in a a Jekyll blog site (source), translate 
MarkDown image constructs to a call to included file imageMulti.html 
with same meta-info***

This is useful for the display of blog posts on a phone where text may be rendered in 
small columns becuase of wide images. On a phone in portrait mode, all images a rendered
 with the same width but can be clicked upon to expand.

## Further
[Jekyll: Rendering on a Mobile](https://davidjones.sportronics.com.au/web/Jekyll-Rendering_on_a_Mobile-rel-web.html) blog post.

## About imageMulti.html

File: imageMulti.html in /include 

- Display an image from file in /images /media or /grove in fullscreen mode, for desktop or 360 width for phone.
- In phone mode can click on it and show in full width mode.

Parameters:
- imagefile:        The image file in /images, /media or /grove or as in imgFolders
- tag               The div Id that the image is placed in
- alt:              The image alt text
- imgFolders        A csv list of folders containing images

Site Setting:
- in _config.yml 
- site.phonewidth:  The document width when to switch between full screen and phone mode
  - typical value 680, found by trial an error.

Notes:
  - Jekyll values are rendered BEFORE any Javascript runs.
  - Same file is used whether in desktop mode or phone mode.
