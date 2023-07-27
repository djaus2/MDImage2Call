using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace MDImage2Call
{
    internal class Program
    {
        static string repFolder = "C:\\Users\\david\\repos\\DJzBlog";     // Jekyll Blog Posts root folder
        static string postsFolder = "_posts";                             // Source folder of posts in MarkDown format
        static string targetFolder = "_postsTemp";                        // Folder to place modified files in
        static string mask = "2023-0*.md";                                // File search mask
        static void Main(string[] args)
        {

            if (args.Length >0)
            {
                if (args[0]!="")
                {

                    if ((args[0]=="/?") || (args[0].ToLower() == "-help"))
                    {
                        Console.WriteLine("About:");
                        Console.WriteLine("-------");
                        Console.WriteLine("With Jekyll blog posts convert MarkDown image tags to use auto resized image include file.");
                        Console.WriteLine("Usage:");
                        Console.WriteLine("-------");
                        Console.WriteLine("MSImage2Call [File Mask] [Target Folder wrt to:] [Repository Folder]");
                        Console.WriteLine("Target Folder can be a single character in which case _posts is used but Repository Folder as specified is used.");

                        ReadImageFile();

                        Console.WriteLine();
                        return;
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
                        // Use a single character to skip 2nd parameter
                        if (args[1].Length>1)
                        {
                            postsFolder = args[1];
                            targetFolder = args[1] + "Temp";
                        }
                        if (args.Length >2)
                        {
                            repFolder = args[2];
                        }
                    }
                }
            }
            string PostsDir = $"{repFolder}\\{postsFolder}";
            var files = Directory.GetFiles(PostsDir,mask);

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
            Console.WriteLine("Convert Marhkdown Image tag to call included Jekyl image.html!");
            int imgNo = 0;
            foreach (string line in File.ReadAllLines(pathToFile))
            {
                bool imgFound = false;

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
                                    imgNo++;
                                    imgFound = true;
                                    string img = line.Substring(indx1,1+ indx5 - indx1);
                                    //Console.WriteLine(img);
                                    string alt = line.Substring(indx2 + 1, indx3 - indx2 - 1).Trim(); ;
                                    string filePath =  line.Substring(indx4+1,  indx5 - indx4-1).Trim();
                                    //Console.WriteLine(alt);
                                    //Console.WriteLine(filePath);
                                    if(filePath.Substring(0, "/images".Length)== "/images")
                                        filePath = filePath.Substring("/images".Length);
                                           
                                    string lin = $"{{% include image.html imagefile = \"{filePath}\" tag = \"QWERTY{imgNo}\" alt = \"{alt}\" %}}";
                                    if (indx1 != 0)
                                        lin = line.Substring(0, indx1 - 1) + lin;
                                    if (indx5 < line.Length-1)
                                        lin += line.Substring(indx5);
                                    Console.WriteLine("{% comment %}");
                                    linesOut = linesOut.Append("{% comment %}").ToArray();
                                    Console.WriteLine(line);
                                    linesOut = linesOut.Append(line).ToArray();
                                    Console.WriteLine("{% endcomment %}");
                                    linesOut = linesOut.Append("{% endcomment %}").ToArray();
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