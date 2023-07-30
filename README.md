# MDImage2Call

***For MarkDown files in a a Jekyll blog site (source), translate 
MarkDown image constructs to a call to included file imageMulti.html 
with same meta-info***

> This is useful for the display of blog posts on a phone where text may be rendered in 
small columns becuase of wide images. On a phone in portrait mode, all images a rendered
 with the same width but can be clicked upon to expand.

```
MSImage2Call [File Mask] [Target Folder wrt to:] [Repository Folder] {Images folders csv list]
```

- The app reads all MD files in ```_posts``` subject to the ```mask```, and writes 
modified files to ```_postsTemp```.
- Run with  ```/?``` ```-help``` or ```--help```` to get a list of the app parameters.
- Run with ```img``` to get a listing of the include file ```image.html```


## Further
[Jekyll: Rendering on a Mobile](https://davidjones.sportronics.com.au/web/Jekyll-Rendering_on_a_Mobile-rel-web.html) blog post.

## About image.html

File: image.html in /include 

- Display an image from file in /images /media or /grove in fullscreen mode, for desktop or 360 width for phone.
- In phone mode can click on it and show in full width mode.

Parameters:
- imagefile:        The image file in /images, /media or /grove or as in imgFolders
  - Any image file path can be used but this app only accepts the above 3 locations
  - The 3 locations are an optionally csv list parameter to the app
- tag               The div Id that the image is placed in
  - div tag Id for display in phone has small appended
- alt:              The image alt text

Site Setting:
- in _config.yml 
- site.phonewidth:  The document width when to switch between full screen and phone mode
  - typical value 680, found by trial an error.

Notes:
  - Jekyll values are rendered BEFORE any Javascript runs.
  - Same include file is used whether in desktop mode or phone mode.
