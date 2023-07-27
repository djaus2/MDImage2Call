# MDImage2Call

***For MarkDown files in a a Jekyll blog site (source), translate 
MarkDown image constructs to a call to included file image.html 
with same meta-info***

This is useful for the display of blog posts on a phone where text may be rendered in 
small columns becuase of wide images. On a phone in portrait mode, all images a rendered
 with the same width but can be clicked upon to expand.

## About image.html

File: image.html in /include 

- Display an image from file in /images in fullscreen mode, for desktop or 360 width for phone.
- In phone mode can click on it and show in full width mode.

Parameters:
- imagefile:        The image file in /images
- alt:              The image alt text

Site Setting:
- in _config.yml 
- site.phonewidth:  The document width when to switch between full screen and phone mode
  - typical value 680, found by trial an error.

Notes:
  - Jekyll values are rendered BEFORE any Javascript runs.
  - Same file is used whether in desktop mode or phone mode.
