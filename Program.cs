using System;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MDImage2Call
{
    internal class Program
    {
        static string repFolder = "C:\\Users\\david\\repos\\DjsBlog2\\DJzBlog";     // Jekyll Blog Posts root folder
        static string postsFolder = "_postsBak";                             // Source folder of posts in MarkDown format
        static string targetFolder = "_postsTemp";                        // Folder to place modified files in
        static string imgFolders = "images,media,grove";                  // Folders in which images are placed
        static string mask = "2023-0*.md";                                // File search mask
        static int numFilesToShoOnConsole = 0;                            // Show target file contents for this number of files. (First ...)
        static bool ShowContents = false;
        static string imgFolder = "images";
        static string[] ImageFolders = new string[0];
        static void Main(string[] args)
        {
            if (args.Length >0)
            {
                if (args[0]!="")
                {

                    if ((args[0] == "/?") || (args[0].ToLower() == "-help") || (args[0].ToLower() == "--help"))
                    {
                        Console.WriteLine("About:");
                        Console.WriteLine("-------");
                        Console.WriteLine("With Jekyll blog posts convert MarkDown image tags to use auto resized image include file (imageMulti.html).");
                        Console.WriteLine();
                        Console.WriteLine("Usage:");
                        Console.WriteLine("-------");
                        Console.WriteLine("MSImage2Call [File Mask] [Target Folder wrt to:] [Repository Folder] {Images folders csv list]");
                        Console.WriteLine("Parameters can be - in which case they are skipped but parameters to right will be interogated.");
                        Console.WriteLine("Images folders is images,media,grove.");
                        Console.WriteLine();
                        Console.WriteLine("Alternative Usages:");
                        Console.WriteLine("-------------------");
                        Console.WriteLine("MSImage2Call /? | -help | --help   to get this");
                        Console.WriteLine("\tOR");
                        Console.WriteLine("MSImage2Call -img to display image.html file");
                        Console.WriteLine();
                        return;
                    }
                    else if ((args[0].Substring(0, "-img".Length) == "-img"))
                    {
                        ReadImageFile();
                        Console.WriteLine();
                        return;
                    }
                    else if (args[0].Length==1)
                    {
                        //Skip
                    }
                    else if (args[0].Contains(".md"))
                    {
                        // First parameter is the complete mask.
                        mask = args[0];
                    }
                    else
                    {
                        // The first parameter is a partial maask
                        mask = args[0] + "*.md";
                    }
                    if (args.Length>1)
                    {
                        if (args[1].Length == 1)
                        {
                            //Skip
                        }
                        // Use a single character to skip 2nd parameter
                        else if (args[1].Length>1)
                        {
                            postsFolder = args[1];
                            targetFolder = args[1] + "Temp";
                        }
                        if (args.Length >2)
                        {
                            if (args[2].Length == 1)
                            {
                                //Skip
                            }
                            else if(args[2].Length > 1)
                            {
                                repFolder = args[2];
                            }
                            if (args.Length > 3)
                            {
                                if (args[3].Length == 1)
                                {
                                    //Skip
                                }
                                else if (args[3].Length > 1)
                                {
                                    imgFolders = args[3];
                                }
                            }
                        }
                    }
                }
            }
            ImageFolders = imgFolders.Split(',');
            string PostsDir = $"{repFolder}\\{postsFolder}";
            var files = Directory.GetFiles(PostsDir,mask);
            if (files.Count() > numFilesToShoOnConsole)
                ShowContents = false;
            else
                ShowContents = true;

            // Call doFile() for each post in the PostsDir directory
            foreach (var file in files) {
                string filename = Path.GetFileName(file);
                doFile(filename);
            }

        }

        /// <summary>
        /// Search a MD file line by line.
        /// Look for images in MarkDown format
        /// ie ![alt text](relative path to image)
        /// Images are in /images
        /// Typical processing
        /// ![Twitter Card Large Image Summary](/images/twitter_card_large_image_summary_ok.png)
        /// becomes:
        /// {% include image.html imagefile="twitter_card_large_image_summary_ok.png" tag="QWERTY3" alt="Twitter Card Large Image Summary" %} 
        /// </summary>
        /// <param name="file">Filename of poste</param>
        static void doFile (string file)
        { 
            string[] linesOut = new string[0];
            string pathToFile = $"{repFolder}\\{postsFolder}\\{file}";
            if(ShowContents)
                Console.WriteLine("Convert Marhkdown Image tag to call included Jekyl image.html!");
            int imgNo = 0;
            string[] lines = File.ReadAllLines(pathToFile);
            if (lines.Length == 0)
                return;
            foreach (string line in lines )
            {
                bool imgFound = false;
                if (line.Contains("{% comment %}"))
                {
                    // Skip file as has been previously processed.
                    // A safeguard!
                    Console.Write("This file was previously processed. Cntrl-C now to kill this.");
                    Console.ReadLine();
                    return;
                }

                //MarkDown image starts with "!["
                int indx1 = line.IndexOf("![");
                if (indx1>-1)
                {
                    //Starts with ![ for MD
                    int indx2 = indx1 + 1;
                    int indx3 = line.IndexOf(']', indx2);
                    if (indx3 > -1)
                    {
                        int indx4 = line.IndexOf('(', indx3);
                        if (indx4 > -1)
                        {
                            //Should be ](
                            if (indx4 == indx3 + 1)
                            {
                                int indx5 = line.IndexOf(')', indx4);
                                if (indx5 > -1)
                                {

                                    string img = line.Substring(indx1,1+ indx5 - indx1);
                                    string alt = line.Substring(indx2 + 1, indx3 - indx2 - 1).Trim(); ;
                                    string filePath =  line.Substring(indx4+1,  indx5 - indx4-1).Trim();
                                    //Remove /images from start of image file path, as assumed by the image,.html "macro"
                                    bool validFolder = false;
                                    foreach (var fol in ImageFolders)
                                    {
                                        string ffol = "/" + fol;
                                        if (filePath.Substring(0,ffol.Length) == ffol)
                                        {
                                            imgFolder = fol;
                                            validFolder = true;
                                            break;
                                        }
                                    }
                                    if(!validFolder)
                                    {
                                        // Skip for now
                                        if (ShowContents)
                                            Console.WriteLine(line);
                                        linesOut = linesOut.Append(line).ToArray();
                                        continue;
                                    }
                                    imgNo++;
                                    imgFound = true;
                                    string lin = $"{{% include image.html imagefile = \"{filePath}\" tag = \"QWERTY{imgNo}\" alt = \"{alt}\"  %}}";
                                    if (indx1 != 0)
                                        lin = line.Substring(0, indx1 - 1) + lin;
                                    if (indx5 < line.Length)
                                        //Skip closing bracket so only append characters from one more than end of MD image construct
                                        lin += line.Substring(indx5+1);
                                    if (ShowContents)
                                        Console.WriteLine("{% comment %}");
                                    linesOut = linesOut.Append("{% comment %}").ToArray(); 
                                    if (ShowContents)
                                        Console.WriteLine(line);
                                    linesOut = linesOut.Append(line).ToArray();
                                    if (ShowContents)
                                        Console.WriteLine("{% endcomment %}");
                                    linesOut = linesOut.Append("{% endcomment %}").ToArray();
                                    if (ShowContents)
                                        Console.WriteLine(lin);
                                    linesOut = linesOut.Append(lin).ToArray();
                                }
                            }
                        }
                    }
                }
                if(!imgFound)
                {
                    // No MarkDown image in the line so just add the unadulterated line
                    if (ShowContents)
                        Console.WriteLine(line);
                    linesOut = linesOut.Append(line).ToArray();
                }
            }
            if(imgNo>0)
            {
                //if more than one image in file using MarkDown syntax then write new version of file to Target folder.
              
                // Check if target exists and iff not create it
                string TargetDir = $"{repFolder}\\{targetFolder}";
                if (!Directory.Exists(TargetDir))
                    Directory.CreateDirectory(TargetDir);

                // If file exists in Target Folder delete it
                string fileNameOut = $"{TargetDir}\\{file}";
                Console.WriteLine(fileNameOut);
                if (File.Exists(fileNameOut))
                    File.Delete(fileNameOut);

                // Write new version of file to Target Folder
                File.WriteAllLines(fileNameOut, linesOut);
            }
        }

        static void ReadImageFile()
        {
            string result = "";
            Console.WriteLine();
            Console.WriteLine("image.html in /include");
            Console.WriteLine("For Jekyll blog site");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Parameters: imagefile divTag altText");
            Console.WriteLine("------------------------------------");

            try
            {
                result = MDImage2Call.Resource1.image_html;
                Console.WriteLine(result);
            }
            catch
            {
                Console.WriteLine("Error accessing resources!");
            }

        }
    }


}